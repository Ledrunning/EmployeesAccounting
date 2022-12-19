using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
using Emgu.CV;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    /// <summary>
    ///     View model class for main screen
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private const string GetPhotoTooltipMessage = "Press to take a photo and add a person details";
        private const string StartDetectorTooltipMessage = "Press to run facial detection";
        private const string HelpTooltipMessage = "Press to get a program help";
        private const int OneSecond = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private string _currentTimeDate;

        private WebServerApi _dataStorage;
        private FaceDetectionService _faceDetectionService;
        private Bitmap _frame;
        private readonly ModalViewModel _modalWindow = new ModalViewModel();

        private bool _isRunning;

        private bool _isStreaming;
        private LoginWindow _loginForm;
        private RegistrationForm _registrationFormPage;
        private SoundPlayerService _soundPlayerHelper;

        /// <summary>
        ///     Timer
        /// </summary>
        public DispatcherTimer Timer;

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainViewModel()
        {
            InitializeServices();
            InitializeCommands();
            TimeTicker();
            _dataStorage = new WebServerApi(_urlAddress);
        }

        /// <summary>
        ///     Get start tooltip
        /// </summary>
        public string GetStarted => StartDetectorTooltipMessage;

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string GetPhoto => GetPhotoTooltipMessage;

        /// <summary>
        ///     Help tooltip message
        /// </summary>
        public string GetHelpTooltip => HelpTooltipMessage;

        /// <summary>
        ///     Current date binding property
        /// </summary>
        public string CurrentTimeDate
        {
            get => _currentTimeDate;
            set => SetField(ref _currentTimeDate, value);
        }

        /// <summary>
        ///     Start webCam service button toggle
        /// </summary>
        public bool IsStreaming
        {
            get => _isStreaming;
            set => SetField(ref _isStreaming, value);
        }

        /// <summary>
        ///     Start webCam service button toggle
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                SetField(ref _isRunning, value);
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

        /// <summary>
        ///     Property for webCam service
        /// </summary>
        public ICommand ToggleWebServiceCommand { get; private set; }

        /// <summary>
        ///     Property for RegForm view
        /// </summary>
        public ICommand TogglePhotoShootServiceCommand { get; private set; }

        /// <summary>
        ///     Button for help
        /// </summary>
        public ICommand ToggleHelpCallCommand { get; private set; }

        private void InitializeServices()
        {
            Logger.Info("Initialize of all services.....");
            _faceDetectionService = FaceDetectionService.GetInstance();
            _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Service From WebCamService
        /// </summary>
        private void FaceDetectionServiceExecute()
        {
            // Playing sound effect for button
            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");

            if (_faceDetectionService == null)
            {
                _faceDetectionService = FaceDetectionService.GetInstance();
                _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;
            }

            if (!_faceDetectionService.IsRunning)
            {
                IsStreaming = true;
                _faceDetectionService.RunServiceAsync();
                Logger.Info("Face detection is started!");
            }
            else
            {
                IsStreaming = false;
                StopFaceDetectionService();
            }
        }

        private void StopFaceDetectionService()
        {
            _faceDetectionService.FaceDetectionImageChanged -= OnImageChanged;
            _faceDetectionService.CancelServiceAsync();
            Logger.Info("Face detection stopped!");
            _faceDetectionService.Dispose();
            _faceDetectionService = null;
        }

        /// <summary>
        ///     PhotoShoot service execute
        /// </summary>
        private void TogglePhotoShootServiceExecute()
        {
            // Playing sound effect for button
            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");

            // True - button is pushed - Working!
            IsRunning = false;

            if (_loginForm == null || _loginForm.IsClosed)
            {
                _loginForm = new LoginWindow();
                var loginFormViewModel = new LoginViewModel(_loginForm);

                _loginForm.DataContext = loginFormViewModel;
                _loginForm.Owner = Application.Current.MainWindow;
                IsStreaming = false;
                _faceDetectionService.CancelServiceAsync();
                StopFaceDetectionService();
                loginFormViewModel.ShowLoginWindow();
            }


            if (_faceDetectionService != null && _faceDetectionService.IsRunning)
            {
                return;
            }

            IsStreaming = true;
            _faceDetectionService?.RunServiceAsync();
        }

        /// <summary>
        ///     Execute method for relay command TODO: file does not exist
        ///     TODO and will fix modal window NRE exception 
        /// </summary>
        private void ToggleHelpServiceExecute()
        {
            try
            {
                Process.Start(@"Help\intro.html");
            }
            catch (Exception e)
            {
                Logger.Error(e, "An error occuried in opening Help file!");
                _modalWindow.SetMessage("An error occuried in opening Help file!");
                _modalWindow.ShowWindow();
            }
        }

        /// <summary>
        ///     Initialize all commands from main view model
        /// </summary>
        private void InitializeCommands()
        {
            ToggleWebServiceCommand = new RelayCommand(FaceDetectionServiceExecute);
            TogglePhotoShootServiceCommand = new RelayCommand(TogglePhotoShootServiceExecute);
            ToggleHelpCallCommand = new RelayCommand(ToggleHelpServiceExecute);
        }

        /// <summary>
        ///     Get time from sys using dispatcher
        /// </summary>
        private void TimeTicker()
        {
            Timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(OneSecond)
            };
            Timer.Tick += (sender, args) => { CurrentTimeDate = DateTime.Now.ToString(CultureInfo.CurrentCulture); };
            Timer.Start();
        }

        /// <summary>
        ///     Draw the bitmap on control
        /// </summary>
        /// <param name="image"></param>
        private void OnImageChanged(Image<Bgr, byte> image)
        {
            Frame = image.ToBitmap();
        }
    }
}