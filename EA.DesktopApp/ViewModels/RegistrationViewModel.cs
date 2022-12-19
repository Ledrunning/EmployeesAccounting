using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Input;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Models;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    /// <summary>
    ///     View model class for registration form
    /// </summary>
    public class RegistrationViewModel : BaseViewModel, IDataErrorInfo
    {
        /// <summary>
        ///     Readonly fields
        /// </summary>
        private const string FileExtension = ".jpg";

        private static readonly string TrainerDataPath = Path.GetDirectoryName(
                                                             Assembly.GetExecutingAssembly().Location) +
                                                         "\\Traineddata";

        private readonly string _urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _isReady;
        private ModalViewModel _modalView;
        private ModalWindow _modalWindow;

        /// <summary>
        ///     PhotoShoot Service needed
        /// </summary>
        private PhotoShootService _photoShootService;

        private SoundPlayerService _soundPlayerHelper;

        private bool _takePhotoFlag;

        /// <summary>
        ///     .ctor
        /// </summary>
        public RegistrationViewModel()
        {
            InitializeServices();
            InitializeCommands();
            CreateFolder();
        }

        /// <summary>
        ///     Start webCam service button toggle
        /// </summary>
        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;
                OnPropertyChanged();
            }
        }

        private static void CreateFolder()
        {
            try
            {
                if (!Directory.Exists(TrainerDataPath))
                {
                    Directory.CreateDirectory("Traineddata");
                }
            }
            catch (Exception e)
            {
                var modal = new ModalViewModel();
                modal.SetMessage("Error to creating the folder");
                modal.ShowWindow();
                Logger.Error(e, "Error to creating the folder");
            }
        }

        /// <summary>
        ///     Initialize all services
        /// </summary>
        private void InitializeServices()
        {
            _photoShootService = PhotoShootService.GetInstance();
            _photoShootService.RunServiceAsync();
            _photoShootService.PhotoImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Initialize all commands
        /// </summary>
        private void InitializeCommands()
        {
            ToggleCameraCaptureCommand = new RelayCommand(ToggleGetImageExecute);
            ToggleSavePhotoCommand = new RelayCommand(ToggleSaveImageExecute); 
            ToggleAddToDbCommand = new RelayCommand(ToggleAddImageToDataBase);
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
        public string TakePicture => "Take a photo";

        /// <summary>
        ///     For main xaml add person from DB tooltip message
        /// </summary>
        public string AddPerson => "Press to add an employee";

        /// <summary>
        ///     For main xaml delete person from DB tooltip message
        /// </summary>
        public string DeletePerson => "Press to delete employee";

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string SavePicture => "Press to save photo";

        /// <summary>
        ///     For main xaml enter name tooltip message
        /// </summary>
        public string EnterPersonName => "Enter an employee name";

        /// <summary>
        ///     For main xaml enter last name tooltip message
        /// </summary>
        public string EnterPersonLastName => "Enter an employee last name";

        /// <summary>
        ///     For main xaml enter department tooltip message
        /// </summary>
        public string EnterPersonDepartment => "Enter the department name";

        #endregion ToolTip properties

        #region TextBox properties

        /// <summary>
        ///     Binding person name to TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonName { get; set; }

        /// <summary>
        ///     Binding person last name TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonLastName { get; set; }

        /// <summary>
        ///     Binding person department TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonDepartment { get; set; }

        /// <summary>
        ///     Error indexer
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;

                switch (columnName)
                {
                    case "PersonName":
                        if (string.IsNullOrEmpty(PersonName))
                        {
                            error = "Enter the name!";
                        }

                        break;

                    case "PersonLastName":
                        if (string.IsNullOrEmpty(PersonLastName))
                        {
                            error = "Enter the last name";
                        }

                        break;

                    case "PersonDepartment":
                        if (string.IsNullOrEmpty(PersonDepartment))
                        {
                            error = "Enter the department name";
                        }

                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => "Enter the data!";

        #endregion TextBox properties

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
        ///     Toogle to photoshoot save by open file dialog
        /// </summary>
        public ICommand ToggleSavePhotoCommand { get; private set; }

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleAddToDbCommand { get; private set; }

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleEditFormCommand => null;

        #endregion Command properties

        #region Toggles Execute methods

        /// <summary>
        ///     Send image to Data base
        /// </summary>
        private void ToggleAddImageToDataBase()
        {
            _modalView = new ModalViewModel(new ModalWindow());
            //_modalView.ShowLoginWindow();

            _soundPlayerHelper = new SoundPlayerService();

            _soundPlayerHelper.PlaySound("button");

            var picture = PhotoShootGray.Bytes;

            Image resultImage = PhotoShootGray.ToBitmap();
            var converter = new ImageConverter();
            var imageArray = (byte[])converter.ConvertTo(resultImage, typeof(byte[]));

            var person = new Person
            {
                Name = PersonName,
                LastName = PersonLastName,
                Department = PersonDepartment,
                DateTime = DateTimeOffset.Now,
                //Photo = Convert.ToBase64String(_picture)
                Photo = Convert.ToBase64String(imageArray)
            };

            if (person.Name == null || person.LastName == null
                                    || person.Department == null)
            {
                _modalView.SetMessage("Enter the data");
                _modalView.ShowWindow();
            }
            else
            {
                try
                {
                    var client = new WebServerApi(_urlAddress);

                    client.AddPerson(person);
                    _modalView.SetMessage("Data has been successfully loaded to database.");
                    _modalView.ShowWindow();
                }
                catch (CommunicationException ex)
                {
                    _modalView.SetMessage("Error database connection.");
                    _modalView.ShowWindow();
                    Logger.Error(ex, "Error database connection.");
                }
                catch (Exception ex)
                {
                    _modalView.SetMessage("Error to save data into database");
                    _modalView.ShowWindow();
                    Logger.Error(ex, "Error to save data into database");
                }
            }
        }

        /// <summary>
        ///     Get grayscale image method
        /// </summary>
        private void ToggleGetImageExecute()
        {
            _soundPlayerHelper = new SoundPlayerService();

            _soundPlayerHelper.PlaySound("camera");

            // Get grayscale and send into BitmapToImageSourceConverter
            GrayScaleImage = PhotoShootGray.ToBitmap();
            _takePhotoFlag = true;
        }

        /// <summary>
        ///     Save grayscale image method
        /// </summary>
        private void ToggleSaveImageExecute()
        {
            var dialogService = new DialogService();

            _modalView = new ModalViewModel(new ModalWindow());

            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");

            if (_takePhotoFlag)
            {
                if (dialogService.SaveFileDialog())
                {
                    // New Bitmap and save to file
                    PhotoShootFrame = new Bitmap(PhotoShootFrame, 
                        ImageProcessingConstants.PhotoWidth, 
                        ImageProcessingConstants.PhotoHeight);
                    PhotoShootFrame.Save($"{dialogService.FilePath}{FileExtension}", ImageFormat.Jpeg);
                }

                _takePhotoFlag = false;
            }
            else
            {
                _modalView.SetMessage("Photo does not exist");
                _modalView.ShowWindow();
            }
        }
        #endregion Toggles Execute methods
    }
}