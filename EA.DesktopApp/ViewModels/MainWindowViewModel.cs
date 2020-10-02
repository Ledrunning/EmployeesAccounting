using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EA.DesktopApp.Engine;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    /// <summary>
    ///     View model class for main screen
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        private const string GetPhotoTooltipMessage = "Нажмите, что бы сделать фотографию";
        private const string StartDetectorTooltipMessage = "Нажмите для запуска детектора";
        private const string HelpTooltipMessage = "Нажмите для справки";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string _trainerDataPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location) + "\\Traineddata";

        private readonly string urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private string _currentTaimeDate;
        private FaceDetectionService _faceDetectionService;
        private Bitmap _frame;

        private bool _isRunning;

        private bool _isStreaming;
        private PhotoShootService _photoShootService;
        private RecognizerEngine _recognizer;
        private RegistrationForm _registrationFormPage;
        private SoundPlayerHelper _soundPlayerHelper;

        /// <summary>
        ///     Timer
        /// </summary>
        public DispatcherTimer _timer;

        private WebApiHelper dataStorage;
        private PasswordWindow loginForm;

        private readonly RecognizerEngine rec = new RecognizerEngine(_trainerDataPath);

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainWindowViewModel()
        {
            InitializeServices();
            InitializeCommands();
            TimeTicker();
            dataStorage = new WebApiHelper(urlAddress);
            //Изображение для тренировки
            LoadImages();
        }

        /// <summary>
        ///     Get start tooltip
        /// </summary>
        public string GetStarted { get; } = StartDetectorTooltipMessage;

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string GetPhoto { get; } = GetPhotoTooltipMessage;

        /// <summary>
        ///     Help tooltip message
        /// </summary>
        public string GetHelpTooltip { get; } = HelpTooltipMessage;

        /// <summary>
        ///     Current date binding property
        /// </summary>
        public string CurrentTimeDate
        {
            get => _currentTaimeDate;
            set
            {
                _currentTaimeDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Start webCam service button toogle
        /// </summary>
        public bool IsStreaming
        {
            get => _isStreaming;
            set
            {
                _isStreaming = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Start webCam service button toogle
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Property for View Image component notification
        /// </summary>
        public Bitmap Frame
        {
            get => _frame;

            set => SetField(ref _frame, value);
        }

        private void InitializeServices()
        {
            Logger.Info("Initialize of all services.....");
            _faceDetectionService = new FaceDetectionService();
            _photoShootService = new PhotoShootService();
            _faceDetectionService.ImageWithDetectionChanged -= FaceDetectionServiceImageChanged;
            _faceDetectionService.ImageWithDetectionChanged += FaceDetectionServiceImageChanged;
        }

        /// <summary>
        ///     Service From WebCamService
        /// </summary>
        private void ToggleWebServiceExecute()
        {
            // Playing sound effect for button
            _soundPlayerHelper = new SoundPlayerHelper();
            _soundPlayerHelper.PlaySound("button");

            if (!_faceDetectionService.IsRunning)
            {
                IsStreaming = true;
                _faceDetectionService.RunServiceAsync();
                LoadImages();
            }
            else
            {
                IsStreaming = false;
                _faceDetectionService.CancelServiceAsync();
            }
        }

        /// <summary>
        ///     PhotoShoot service execute
        /// </summary>
        private void TogglePhotoShootServiceExecute()
        {
            // Playing sound effect for button
            _soundPlayerHelper = new SoundPlayerHelper();
            _soundPlayerHelper.PlaySound("button");

            // True - button is pushed - Working!
            IsRunning = false;

            if (loginForm == null || loginForm.IsClosed)
            {
                loginForm = new PasswordWindow();
                var loginFormVievModel = new LoginFormViewModel(loginForm);

                loginForm.DataContext = loginFormVievModel;
                loginForm.Owner = Application.Current.MainWindow;
                IsStreaming = false;
                _faceDetectionService.CancelServiceAsync();
                loginFormVievModel.ShowWindow();
            }


            if (!_faceDetectionService.IsRunning)
            {
                IsStreaming = true;
                _faceDetectionService.RunServiceAsync();
            }
        }

        /// <summary>
        ///     Execute method for relay command
        /// </summary>
        private void ToogleHelpServiceExecute()
        {
            Process.Start(@"Help\intro.html");
        }

        /// <summary>
        ///     Initialize all commands from main view model
        /// </summary>
        private void InitializeCommands()
        {
            _toggleWebServiceCommand = new RelayCommand(ToggleWebServiceExecute);
            _togglePhotoShootServiceCommand = new RelayCommand(TogglePhotoShootServiceExecute);
            _toogleHelpCallCommand = new RelayCommand(ToogleHelpServiceExecute);
        }

        private void Recognize()
        {
            try
            {
                //var predictedUserId = _recognizer.RecognizeUser(new Image<Gray, byte>(picCapturedUser.Image.Bitmap));
                //if (predictedUserId == -1)
                //{

                //}

                //else
                //{
                //    //proceed to documents library
                //    var username = dataStorage.GetAllAsync();

                //    if (username != String.Empty)
                //    {

                //    }
                //    else
                //    {

                //    }

                //}
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Get time from sys using dispatcher
        /// </summary>
        private void TimeTicker()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) => { CurrentTimeDate = DateTime.Now.ToString(); };
            _timer.Start();
        }

        /// <summary>
        ///     Draw the bitmap on control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void FaceDetectionServiceImageChanged(object sender, Image<Bgr, byte> image)
        {
            try
            {
                if (rec.IsTrained)
                {
                    var img = image.Convert<Gray, byte>();
                    var result = rec.eigenFaceRecognizer.Predict(img.Resize(100, 100, INTER.CV_INTER_CUBIC));
                    if (result.Label != -1)
                    {
                        _faceDetectionService.EmployeeData = rec.eigenlabels[result.Label];
                    }
                }

                Frame = image.Bitmap;
            }
            catch (Exception e)
            {
                Logger.Error(e, "\nFace recognizer error:");
            }
        }

        private void LoadImages()
        {
            try
            {
                var bgWorker = new BackgroundWorker();
                bgWorker.WorkerReportsProgress = true;
                bgWorker.WorkerSupportsCancellation = false;
                bgWorker.DoWork += BgWorkerDoWork;
                bgWorker.RunWorkerCompleted += BgWorkerRunWorkerCompleted;
                ;
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void BgWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (FaceDAL.bancoDados)
            //{
            //    Eigenfaces.IsTrained = Eigenfaces.TrainFromDataBase();
            //}
            //else
            //{
            // Eigenfaces.IsTrained = Eigenfaces.TrainFromFolder();
            //}
        }

        private async void BgWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            //rec.TrainFromFolder();
            await rec.TrainFromDataBase(urlAddress);
        }

        #region Action commands for buttons

        private ICommand _toggleWebServiceCommand;

        /// <summary>
        ///     Property for webCam service
        /// </summary>
        public ICommand ToggleWebServiceCommand
        {
            get => _toggleWebServiceCommand;

            private set { }
        }

        private ICommand _togglePhotoShootServiceCommand;

        /// <summary>
        ///     Property for RegForm view
        /// </summary>
        public ICommand TogglePhotoShootServiceCommand
        {
            get => _togglePhotoShootServiceCommand;

            private set { }
        }

        private ICommand _toogleHelpCallCommand;

        /// <summary>
        ///     Button for help
        /// </summary>
        public ICommand ToggleHelpCallCommand
        {
            get => _toogleHelpCallCommand;

            private set { }
        }

        #endregion Action commands for buttons
    }
}