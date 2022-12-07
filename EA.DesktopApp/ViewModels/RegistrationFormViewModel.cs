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
using EA.DesktopApp.Models;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
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
        private const int PhotoHeight = 400;

        private const int PhotoWidth = 500;

        private static readonly string trainerDataPath = Path.GetDirectoryName(
                                                             Assembly.GetExecutingAssembly().Location) +
                                                         "\\Traineddata";

        /// <summary>
        ///     Readonly fields
        /// </summary>
        private readonly string FileExtension = ".jpg";

        private readonly string urlAddress = ConfigurationManager.AppSettings["serverUriString"];

        private bool isReady;
        private ModalWindowViewModel modalView;
        private ModalWindow modalWindow;

        /// <summary>
        ///     PhotoShoot Service needed
        /// </summary>
        private PhotoShootService photoShootService;

        private SoundPlayerService soundPlayerHelper;

        private bool takePhotoflag;

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
            get => isReady;
            set
            {
                isReady = value;
                OnPropertyChanged();
            }
        }

        private void CreateFolder()
        {
            try
            {
                if (!Directory.Exists(trainerDataPath))
                {
                    Directory.CreateDirectory("Traineddata");
                }
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
            photoShootService = new PhotoShootService();
            // Run image capture from WebCam
            photoShootService.RunServiceAsync();
            photoShootService.ImageWithDetectionChanged -= PhotoShootServiceImageChanged;
            photoShootService.ImageWithDetectionChanged += PhotoShootServiceImageChanged;
        }

        /// <summary>
        ///     Initiaalize all commands
        /// </summary>
        private void InitializeCommands()
        {
            toggleCameraCaptureCommand = new RelayCommand(ToggleGetImageExecute);
            //_toggleSavePhotoCommand = new RelayCommand(ToggleSaveImageExecute); 
            toggleSavePhotoCommand = new RelayCommand(ToggleSaveFaceExecute);
            toggleAddToDbCommand = new RelayCommand(ToogleAddImageToDataBase);
        }

        /// <summary>
        ///     Event handler for image changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void PhotoShootServiceImageChanged(object sender, Image<Bgr, byte> image)
        {
            //PhotoShootFrame = image.Bitmap;
            // New grayscale image for recognition
            PhotoShootGray = image.Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
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
                        if (string.IsNullOrEmpty(PersonName))
                        {
                            error = "Введите имя!";
                        }

                        break;

                    case "PersonLastName":
                        if (string.IsNullOrEmpty(PersonLastName))
                        {
                            error = "Введите Фамилию!";
                        }

                        break;

                    case "PersonDepartment":
                        if (string.IsNullOrEmpty(PersonDepartment))
                        {
                            error = "Введите название отдела!";
                        }

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

        private Bitmap grayScaleImage;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap GrayScaleImage
        {
            get => grayScaleImage;

            set => SetField(ref grayScaleImage, value);
        }

        private Bitmap photoShootFrame;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Bitmap PhotoShootFrame
        {
            get => photoShootFrame;

            set => SetField(ref photoShootFrame, value);
        }

        private Image<Gray, byte> photoShootGray;

        /// <summary>
        ///     Get bitmap from frame
        /// </summary>
        public Image<Gray, byte> PhotoShootGray
        {
            get => photoShootGray;

            set => SetField(ref photoShootGray, value);
        }

        #endregion Image fields

        #region Command properties

        private ICommand toggleCameraCaptureCommand;

        /// <summary>
        ///     Toogle to photoshoot command
        /// </summary>
        public ICommand ToggleCameraCaptureCommand
        {
            get => toggleCameraCaptureCommand;

            private set { }
        }

        private ICommand toggleSavePhotoCommand;

        /// <summary>
        ///     Toogle to photoshoot save by open file dialog
        /// </summary>
        public ICommand ToggSavePhotoCommand
        {
            get => toggleSavePhotoCommand;

            private set { }
        }

        private ICommand toggleAddToDbCommand;

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleAddToDbCommand
        {
            get => toggleAddToDbCommand;

            private set { }
        }

        private ICommand toogleEditFormCommand;

        /// <summary>
        ///     Toogle to add image to data base
        /// </summary>
        public ICommand ToggleEditFormCommand
        {
            get => toogleEditFormCommand;

            private set { }
        }

        #endregion Command properties

        #region Toggles Execute methods

        /// <summary>
        ///     Send image to Data base
        /// </summary>
        private void ToogleAddImageToDataBase()
        {
            modalView = new ModalWindowViewModel(new ModalWindow());
            //_modalView.ShowWindow();

            soundPlayerHelper = new SoundPlayerService();

            soundPlayerHelper.PlaySound("button");

            var picture = PhotoShootGray.Bytes;

            Image resultImage = PhotoShootGray.ToBitmap();
            var converter = new ImageConverter();
            var arr = (byte[])converter.ConvertTo(resultImage, typeof(byte[]));

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
                modalView.SetMessage("Введите данные!");
                modalView.ShowWindow();
            }
            else
            {
                try
                {
                    var client = new WebServerApi(urlAddress);

                    client.AddPerson(person);
                    modalView.SetMessage("Данные успешно загружены в базу данных.");
                    modalView.ShowWindow();
                }
                catch (CommunicationException err)
                {
                    modalView.SetMessage("Ошибка связи с базой данных!");
                    modalView.ShowWindow();
                }
                catch (Exception err)
                {
                    modalView.SetMessage("Ошибка связи с базой данных!");
                    modalView.ShowWindow();
                }
            }
        }

        /// <summary>
        ///     Get grayscale image method
        /// </summary>
        private void ToggleGetImageExecute()
        {
            soundPlayerHelper = new SoundPlayerService();

            soundPlayerHelper.PlaySound("camera");

            // Get grayscale and send into BitmapToImageSourceConverter
            GrayScaleImage = PhotoShootGray.ToBitmap();
            takePhotoflag = true;
        }

        /// <summary>
        ///     Save grayscale image method
        /// </summary>
        private void ToggleSaveImageExecute()
        {
            var dialogService = new DialogService();

            modalView = new ModalWindowViewModel(new ModalWindow());

            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");

            if (takePhotoflag)
            {
                if (dialogService.SaveFileDialog())
                {
                    // New Bitmap and save to file
                    PhotoShootFrame = new Bitmap(PhotoShootFrame, PhotoWidth, PhotoHeight);
                    PhotoShootFrame.Save($"{dialogService.FilePath}{FileExtension}", ImageFormat.Jpeg);
                }

                takePhotoflag = false;
            }
            else
            {
                modalView.SetMessage("Фото не сделано!");
                modalView.ShowWindow();
            }
        }

        //Тестовый 
        public void ToggleSaveFaceExecute()
        {
            modalView = new ModalWindowViewModel(new ModalWindow());

            try
            {
                var fileName = PersonName + " " + PersonLastName + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                               FileExtension;
                //PhotoShootGray.Resize(200, 200, INTER.CV_INTER_CUBIC).Save("Traineddata\\" + fileName);
            }
            catch (Exception ex)
            {
                modalView.SetMessage("Ошибка сохранения файла");
                modalView.ShowWindow();
            }
        }

        #endregion Toggles Execute methods
    }
}