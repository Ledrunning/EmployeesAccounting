using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Threading;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Services;
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
        private const int OneSecondForTimeSpan = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IFaceDetectionService _faceDetectionService;
        private readonly LoginViewModel _loginViewModel;

        private readonly ModalViewModel _modalWindow = new ModalViewModel();
        private readonly ISoundPlayerService _soundPlayerHelper;

        private string _currentTimeDate;

        private string _detectionHint;

        private Bitmap _frame;

        private bool _isRunning;

        private bool _isStreaming;

        /// <summary>
        ///     Timer
        /// </summary>
        public DispatcherTimer Timer;

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainViewModel(
            LoginViewModel loginViewModel,
            IFaceDetectionService faceDetectionService,
            ISoundPlayerService soundPlayerHelper)
        {
            _loginViewModel = loginViewModel;
            _faceDetectionService = faceDetectionService;
            InitializeServices();
            InitializeCommands();
            TimeTicker();
            _soundPlayerHelper = soundPlayerHelper;
            DetectionHint = ProgramResources.StartDetectorTooltipMessage;
        }

        /// <summary>
        ///     Get start tooltip
        /// </summary>
        public string DetectionHint
        {
            get => _detectionHint;
            set
            {
                _detectionHint = value;
                OnPropertyChanged(nameof(DetectionHint));
            }
        }

        /// <summary>
        ///     For main xaml Take a photo tooltip message
        /// </summary>
        public string PhotoHint => ProgramResources.PhotoTooltipMessage;

        /// <summary>
        ///     Help tooltip message
        /// </summary>
        public string HelpHint => ProgramResources.HelpTooltipMessage;

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
            _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Service From WebCamService
        /// </summary>
        private void FaceDetectionServiceExecute()
        {
            DetectionHint = IsStreaming
                ? ProgramResources.StartDetectorTooltipMessage
                : ProgramResources.StopDetectorTooltipMessage;

            // Playing sound effect for button
            // _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            if (_faceDetectionService != null && !_faceDetectionService.IsRunning)
            {
                _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;
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
            //_faceDetectionService.Dispose();
        }

        /// <summary>
        ///     PhotoShoot service execute
        /// </summary>
        private void TogglePhotoShootServiceExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            // True - button is pushed - Working!
            IsRunning = false;

            // if (_loginForm == null || _loginForm.IsClosed)
            // {
            //     if (_loginForm != null)
            //     {
            //         _loginForm.DataContext = _loginViewModel;
            //         _loginForm.Owner = Application.Current.MainWindow;
            //     }

            IsStreaming = false;
            StopFaceDetectionService();
            _loginViewModel.ShowLoginWindow();
            //}


            if (!_faceDetectionService.IsRunning)
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
                Logger.Error("An error occuried in opening Help file! {e}", e);
                _modalWindow.SetMessage("An error occurred in opening the Help file!");
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
                Interval = TimeSpan.FromSeconds(OneSecondForTimeSpan)
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