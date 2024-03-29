﻿using System;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;
using Emgu.CV;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    /// <summary>
    ///     View model class for registration form
    ///     Send employee data and photo to the server
    /// </summary>
    public class RegistrationViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmployeeGatewayService _employeeGatewayService;

        private readonly IPhotoShootService _photoShootService;
        private readonly ISoundPlayerService _soundPlayerHelper;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;

        private Bitmap _grayScaleImage;

        private Bitmap _photoShootFrame;

        /// <summary>
        ///     .ctor
        /// </summary>
        public RegistrationViewModel(IPhotoShootService photoShootService,
            ISoundPlayerService soundPlayerHelper,
            IEmployeeGatewayService employeeGatewayService,
            IWindowManager windowManager,
            CancellationToken token)
        {
            _photoShootService = photoShootService;
            _soundPlayerHelper = soundPlayerHelper;
            _employeeGatewayService = employeeGatewayService;
            _windowManager = windowManager;
            _token = token;
            InitializeServices();
            InitializeCommands();
            WindowClosingBehavior.WindowClose += OnWindowClosingBehavior;
        }

        /// <summary>
        ///     For main xaml take a photo
        /// </summary>
        public string TakePicture => UiErrorResource.TakePhoto;

        /// <summary>
        ///     For main xaml add person from DB tooltip message
        /// </summary>
        public string AddPerson => UiErrorResource.AddPerson;

        /// <summary>
        ///     For main xaml delete person from DB tooltip message
        /// </summary>
        public string DeletePerson => UiErrorResource.DeletePerson;

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string SavePicture => UiErrorResource.SavePicture;

        /// <summary>
        ///     For main xaml enter name tooltip message
        /// </summary>
        public string EnterPersonName => UiErrorResource.EnterPersonName;

        /// <summary>
        ///     For main xaml enter last name tooltip message
        /// </summary>
        public string EnterPersonLastName => UiErrorResource.EnterPersonLastName;

        /// <summary>
        ///     For main xaml enter department tooltip message
        /// </summary>
        public string EnterPersonDepartment => UiErrorResource.EnterPersonDepartment;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap GrayScaleImage
        {
            get => _grayScaleImage;

            set => SetField(ref _grayScaleImage, value);
        }

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap PhotoShootFrame
        {
            get => _photoShootFrame;

            set => SetField(ref _photoShootFrame, value);
        }

        /// <summary>
        ///     Toggle to photoshoot command
        /// </summary>
        public ICommand ToggleCameraCaptureCommand { get; private set; }

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleAddToDbCommand { get; private set; }

        /// <summary>
        ///     Toogle clear fields
        /// </summary>
        public ICommand ToggleClearFormCommand { get; private set; }

        private Image<Bgr, byte> CapturedImage { get; set; }

        private void OnWindowClosingBehavior(object sender, EventArgs e)
        {
            _photoShootService?.CancelServiceAsync();
        }

        /// <summary>
        ///     Initialize all services
        /// </summary>
        private void InitializeServices()
        {
            _photoShootService.RunServiceAsync();
            _photoShootService.PhotoImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Initialize all commands
        /// </summary>
        private void InitializeCommands()
        {
            ToggleCameraCaptureCommand = new RelayCommand(ToggleGetImageExecute);
            ToggleAddToDbCommand = new RelayCommand(async () => await ToggleAddImageToDataBase());
            ToggleClearFormCommand = new RelayCommand(ToggleClearFields);
        }

        /// <summary>
        ///     Event handler for image changing
        /// </summary>
        /// <param name="image"></param>
        private void OnImageChanged(Image<Bgr, byte> image)
        {
            CapturedImage = image;
            PhotoShootFrame = image.ToBitmap();
        }

        private void ToggleClearFields()
        {
            ClearFields();
        }

        protected override void ClearFields()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            PersonName = string.Empty;
            PersonLastName = string.Empty;
            PersonDepartment = string.Empty;
            GrayScaleImage = null;
        }

        /// <summary>
        ///     Send image into Data base
        /// </summary>
        private async Task ToggleAddImageToDataBase()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            var converter = new ImageConverter();
            var imageArray = (byte[])converter.ConvertTo(GrayScaleImage, typeof(byte[]));

            var employeeModel = new EmployeeModel
            {
                Name = PersonName,
                LastName = PersonLastName,
                Department = PersonDepartment,
                DateTime = DateTimeOffset.UtcNow,
                Photo = imageArray,
                PhotoName = string.Format(ProgramResources.FileName, PersonName, PersonLastName, DateTime.UtcNow)
            };
            //If something went wrong in UI side checking
            if (employeeModel.Name == null || employeeModel.LastName == null
                                           || employeeModel.Department == null)
            {
                _windowManager.ShowModalWindow("Enter the data");
            }
            else
            {
                try
                {
                    await ExecuteAsync(() => _employeeGatewayService.CreateAsync(employeeModel, _token));
                    _windowManager.ShowModalWindow("Data has been successfully loaded to database.");
                    ClearFields();
                }
                catch (CommunicationException e)
                {
                    _windowManager.ShowModalWindow("Error database connection.");
                    Logger.Error("Error database connection. {e}", e);
                }
                catch (Exception e)
                {
                    _windowManager.ShowModalWindow("Error to save data into database");
                    Logger.Error("Error to save data into database {e}", e);
                }
            }
        }

        /// <summary>
        ///     Get grayscale image method
        /// </summary>
        private void ToggleGetImageExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.CameraSound);
            GrayScaleImage = _photoShootService.CropFaceFromImage(CapturedImage).ToBitmap();
        }
    }
}