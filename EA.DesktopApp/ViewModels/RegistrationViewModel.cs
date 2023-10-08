using System;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Windows.Input;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
        private readonly ISoundPlayerService _soundPlayerService;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;

        /// <summary>
        ///     .ctor
        /// </summary>
        public RegistrationViewModel(IPhotoShootService photoShootService, ISoundPlayerService soundPlayerService,
            IEmployeeGatewayService employeeGatewayService, IWindowManager windowManager, CancellationToken token)
        {
            _photoShootService = photoShootService;
            _soundPlayerService = soundPlayerService;
            _employeeGatewayService = employeeGatewayService;
            _windowManager = windowManager;
            _token = token;
            InitializeServices();
            InitializeCommands();
            WindowClosingBehavior.WindowClose += OnWindowClosingBehavior;
        }

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
            ToggleAddToDbCommand = new RelayCommand(ToggleAddImageToDataBase);
            ToggleClearFormCommand = new RelayCommand(ToggleClearFields);
        }

        /// <summary>
        ///     Event handler for image changing
        /// </summary>
        /// <param name="image"></param>
        private void OnImageChanged(Image<Bgr, byte> image)
        {
            PhotoShootFrame = image.ToBitmap();
            // New grayscale image for recognition
            PhotoShootGray = image.Convert<Gray, byte>().Resize(ImageProcessingConstants.GrayPhotoWidth,
                ImageProcessingConstants.GrayPhotoHeight, Inter.Cubic);
        }

        #region ToolTip properties

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

        #endregion ToolTip properties

        #region Image fields

        private Bitmap _grayScaleImage;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap GrayScaleImage
        {
            get => _grayScaleImage;

            set => SetField(ref _grayScaleImage, value);
        }

        private Bitmap _photoShootFrame;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap PhotoShootFrame
        {
            get => _photoShootFrame;

            set => SetField(ref _photoShootFrame, value);
        }

        private Image<Gray, byte> _photoShootGray;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Image<Gray, byte> PhotoShootGray
        {
            get => _photoShootGray;

            set => SetField(ref _photoShootGray, value);
        }

        #endregion Image fields

        #region Command properties

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

        #endregion Command properties

        #region Toggles Execute methods

        private void ToggleClearFields()
        {
            ClearFields();
        }

        protected override void ClearFields()
        {
            PersonName = string.Empty;
            PersonLastName = string.Empty;
            PersonDepartment = string.Empty;
            GrayScaleImage = null;
        }

        /// <summary>
        ///     Send image into Data base
        /// </summary>
        private async void ToggleAddImageToDataBase()
        {
            _soundPlayerService.PlaySound(SoundPlayerService.ButtonSound);

            var resultImage = PhotoShootGray.ToBitmap();
            var converter = new ImageConverter();
            var imageArray = (byte[])converter.ConvertTo(resultImage, typeof(byte[]));

            var employeeModel = new EmployeeModel
            {
                Name = PersonName,
                LastName = PersonLastName,
                Department = PersonDepartment,
                DateTime = DateTimeOffset.UtcNow,
                Photo = imageArray,
                PhotoName = $"Employee_{PersonName}_{PersonLastName}_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg"
            };

            if (employeeModel.Name == null || employeeModel.LastName == null
                                           || employeeModel.Department == null)
            {
                _windowManager.ShowModalWindow("Enter the data");
            }
            else
            {
                try
                {
                    await _employeeGatewayService.CreateAsync(employeeModel, _token);
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
            _soundPlayerService.PlaySound(SoundPlayerService.CameraSound);

            // Get grayscale and send into BitmapToImageSourceConverter
            GrayScaleImage = PhotoShootGray.ToBitmap();
        }

        #endregion Toggles Execute methods
    }
}