using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Input;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.Models;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EA.DesktopApp.ViewModels
{
    /// <summary>
    ///     View model class for registration form
    /// </summary>
    public class RegistrationFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private static readonly string _trainerDataPath = Path.GetDirectoryName(
                                                              Assembly.GetExecutingAssembly().Location) +
                                                          "\\Traineddata";

        /// <summary>
        ///     Readonly fields
        /// </summary>
        private readonly string fileExtension = ".jpg";

        private readonly int photoHeight = 400;

        private readonly int photoWidth = 500;
        private readonly string urlAddress = ConfigurationManager.AppSettings["serverUriString"];

        private bool _isReady;
        private ModalWindowViewModel _modalView;
        private ModalWindow _modalWindow;

        /// <summary>
        ///     PhotoShoot Service needed
        /// </summary>
        private PhotoShootService _photoShootService;

        private byte[] _picture;

        private SoundPlayerHelper _soundPlayerHelper;

        private bool _takePhotoflag;

        /// <summary>
        ///     Image training section
        /// </summary>
        private Image<Gray, byte> result, trainedFace = null;

        private List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();

        /// <summary>
        ///     .ctor
        /// </summary>
        public RegistrationFormViewModel()
        {
            InitializeServices();
            InitializeCommands();
            CreateFolder();
        }

        /// <summary>
        ///     Start webCam service button toogle
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

        private void CreateFolder()
        {
            try
            {
                if (!Directory.Exists(_trainerDataPath)) Directory.CreateDirectory("Traineddata");
            }
            catch (Exception e)
            {
                var modal = new ModalWindowViewModel();
                modal.SetMessage("Ошибка создания папки");
                modal.ShowWindow();
            }
        }

        /// <summary>
        ///     Initialize all services
        /// </summary>
        private void InitializeServices()
        {
            _photoShootService = new PhotoShootService();
            // Run image capture from WebCam
            _photoShootService.RunServiceAsync();
            _photoShootService.ImageWithDetectionChanged -= PhotoShootServiceImageChanged;
            _photoShootService.ImageWithDetectionChanged += PhotoShootServiceImageChanged;
        }

        /// <summary>
        ///     Initiaalize all commands
        /// </summary>
        private void InitializeCommands()
        {
            _toggleCameraCaptureCommand = new RelayCommand(ToggleGetImageExecute);
            //_toggleSavePhotoCommand = new RelayCommand(ToggleSaveImageExecute); 
            _toggleSavePhotoCommand = new RelayCommand(ToogleSaveFaceExecute);
            _toggleAddToDbCommand = new RelayCommand(ToogleAddImageToDataBase);
        }

        /// <summary>
        ///     Event handler for image changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void PhotoShootServiceImageChanged(object sender, Image<Bgr, byte> image)
        {
            PhotoShootFrame = image.Bitmap;
            // New grayscale image for recognition
            PhotoShootGray = image.Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
        }

        #region ToolTip properties

        /// <summary>
        ///     For main xaml take a photo
        /// </summary>
        public string TakePicture { get; } = "Сфотографировать";

        /// <summary>
        ///     For main xaml add person from DB tooltip message
        /// </summary>
        public string AddPerson { get; } = "Нажмите, что бы добавить сотрудника";

        /// <summary>
        ///     For main xaml delete person from DB tooltip message
        /// </summary>
        public string DeletePerson { get; } = "Нажмите, что бы удалить сотрудника";

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string SavePicture { get; } = "Нажмите, что бы сохранить фотографию";

        /// <summary>
        ///     For main xaml enter name tooltip message
        /// </summary>
        public string EnterPersonName { get; } = "Введите имя сотрудника в поле";

        /// <summary>
        ///     For main xaml enter last name tooltip message
        /// </summary>
        public string EnterPersonLastName { get; } = "Введите имя сотрудника в поле";

        /// <summary>
        ///     For main xaml enter department tooltip message
        /// </summary>
        public string EnterPersonDepartment { get; } = "Введите департамент";

        #endregion ToolTip properties

        #region TextBox properties

        /// Binding person name to TextBox
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
                        if (PersonName == null || PersonName == "") error = "Введите имя!";
                        break;

                    case "PersonLastName":
                        if (PersonLastName == null || PersonLastName == "") error = "Введите Фамилию!";
                        break;

                    case "PersonDepartment":
                        if (PersonDepartment == null || PersonDepartment == "") error = "Введите название отдела!";
                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => "Введите данные!";

        #endregion TextBox properties

        #region Image fields

        private Bitmap _grayScaleImage;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap GrayScaleImage
        {
            get => _grayScaleImage;

            set =>
                // From INotifyPropertyChanged
                SetField(ref _grayScaleImage, value);
        }

        private Bitmap _photoShootFrame;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap PhotoShootFrame
        {
            get => _photoShootFrame;

            set =>
                // From INotifyPropertyChanged
                SetField(ref _photoShootFrame, value);
        }

        private Image<Gray, byte> _photoShootGray;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Image<Gray, byte> PhotoShootGray
        {
            get => _photoShootGray;

            set =>
                // From INotifyPropertyChanged
                SetField(ref _photoShootGray, value);
        }

        #endregion Image fields

        #region Command properties

        private ICommand _toggleCameraCaptureCommand;

        /// <summary>
        ///     Toogle to photoshoot command
        /// </summary>
        public ICommand ToggleCameraCaptureCommand
        {
            get => _toggleCameraCaptureCommand;

            private set { }
        }

        private ICommand _toggleSavePhotoCommand;

        /// <summary>
        ///     Toogle to photoshoot save by open file dialog
        /// </summary>
        public ICommand ToggSavePhotoCommand
        {
            get => _toggleSavePhotoCommand;

            private set { }
        }

        private ICommand _toggleAddToDbCommand;

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleAddToDbCommand
        {
            get => _toggleAddToDbCommand;

            private set { }
        }

        private ICommand _toogleEditFormCommand;

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleEditFormCommand
        {
            get => _toogleEditFormCommand;

            private set { }
        }

        #endregion Command properties

        #region Toggles Execute methods

        /// <summary>
        ///     Send image to Data base
        /// </summary>
        private void ToogleAddImageToDataBase()
        {
            _modalView = new ModalWindowViewModel(new ModalWindow());
            //_modalView.ShowWindow();

            _soundPlayerHelper = new SoundPlayerHelper();

            _soundPlayerHelper.PlaySound("button");

            _picture = PhotoShootGray.Bytes;

            Image resultImage = PhotoShootGray.Bitmap;
            var converter = new ImageConverter();
            var arr = (byte[]) converter.ConvertTo(resultImage, typeof(byte[]));

            var person = new Person
            {
                Name = PersonName,
                LastName = PersonLastName,
                Department = PersonDepartment,
                DateTime = DateTimeOffset.Now,
                //Photo = Convert.ToBase64String(_picture)
                Photo = Convert.ToBase64String(arr)
            };

            if (person.Name == null || person.LastName == null
                                    || person.Department == null)
            {
                _modalView.SetMessage("Введите данные!");
                _modalView.ShowWindow();
            }
            else
            {
                try
                {
                    var client = new WebApiHelper(urlAddress);

                    if (client != null)
                    {
                        client.AddPerson(person);
                        _modalView.SetMessage("Данные успешно загружены в базу данных.");
                        _modalView.ShowWindow();
                    }
                    else
                    {
                        _modalView.SetMessage("Ошибка связи с базой данных!");
                        _modalView.ShowWindow();
                    }
                }
                catch (CommunicationException err)
                {
                    _modalView.SetMessage("Ошибка связи с базой данных!");
                    _modalView.ShowWindow();
                }
                catch (Exception err)
                {
                    _modalView.SetMessage("Ошибка связи с базой данных!");
                    _modalView.ShowWindow();
                }
            }
        }

        /// <summary>
        ///     Get grayscale image method
        /// </summary>
        private void ToggleGetImageExecute()
        {
            _soundPlayerHelper = new SoundPlayerHelper();

            _soundPlayerHelper.PlaySound("camera");

            // Get grayscale and send into BitmapToImageSourceConverter
            GrayScaleImage = PhotoShootGray.ToBitmap();
            _takePhotoflag = true;
        }

        /// <summary>
        ///     Save grayscale image method
        /// </summary>
        private void ToggleSaveImageExecute()
        {
            var dialogService = new DialogService();

            _modalView = new ModalWindowViewModel(new ModalWindow());

            _soundPlayerHelper = new SoundPlayerHelper();
            _soundPlayerHelper.PlaySound("button");

            if (_takePhotoflag)
            {
                if (dialogService.SaveFileDialog())
                {
                    // New Bitmap and save to file
                    PhotoShootFrame = new Bitmap(PhotoShootFrame, photoWidth, photoHeight);
                    PhotoShootFrame.Save($"{dialogService.FilePath}{fileExtension}", ImageFormat.Jpeg);
                }

                _takePhotoflag = false;
            }
            else
            {
                _modalView.SetMessage("Фото не сделано!");
                _modalView.ShowWindow();
            }
        }

        //Тестовый 
        public void ToogleSaveFaceExecute()
        {
            _modalView = new ModalWindowViewModel(new ModalWindow());

            try
            {
                var fileName = PersonName + " " + PersonLastName + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                               fileExtension;
                PhotoShootGray.Resize(200, 200, INTER.CV_INTER_CUBIC).Save("Traineddata\\" + fileName);
            }
            catch (Exception ex)
            {
                _modalView.SetMessage("Ошибка сохранения файла");
                _modalView.ShowWindow();
            }
        }

        #endregion Toggles Execute methods
    }
}