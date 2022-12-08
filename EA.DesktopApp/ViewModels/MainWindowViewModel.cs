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
    public class MainWindowViewModel : BaseViewModel
    {
        private const string GetPhotoTooltipMessage = "Нажмите, что бы сделать фотографию";
        private const string StartDetectorTooltipMessage = "Нажмите для запуска детектора";
        private const string HelpTooltipMessage = "Нажмите для справки";
        private const int OneSecond = 1;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private string currentTaimeDate;

        private WebServerApi dataStorage;
        private FaceDetectionService faceDetectionService;
        private Bitmap frame;

        private bool isRunning;

        private bool isStreaming;
        private PasswordWindow loginForm;
        private PhotoShootService photoShootService;
        private RegistrationForm registrationFormPage;
        private SoundPlayerService soundPlayerHelper;

        /// <summary>
        ///     Timer
        /// </summary>
        public DispatcherTimer Timer;

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainWindowViewModel()
        {
            InitializeServices();
            InitializeCommands();
            TimeTicker();
            dataStorage = new WebServerApi(urlAddress);
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
            get => currentTaimeDate;
            set => SetField(ref currentTaimeDate, value);
        }

        /// <summary>
        ///     Start webCam service button toogle
        /// </summary>
        public bool IsStreaming
        {
            get => isStreaming;
            set => SetField(ref isStreaming, value);
        }

        /// <summary>
        ///     Start webCam service button toogle
        /// </summary>
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                SetField(ref isRunning, value);
            }
        }

        /// <summary>
        ///     Property for View Image component notification
        /// </summary>
        public Bitmap Frame
        {
            get => frame;

            set => SetField(ref frame, value);
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
            logger.Info("Initialize of all services.....");
            photoShootService = new PhotoShootService();
            faceDetectionService = new FaceDetectionService();
            faceDetectionService.ImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Service From WebCamService
        /// </summary>
        private void ToggleWebServiceExecute()
        {
            // Playing sound effect for button
            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");

            if (!faceDetectionService.IsRunning)
            {
                IsStreaming = true;
                faceDetectionService.RunServiceAsync();
            }
            else
            {
                IsStreaming = false;
                faceDetectionService.CancelServiceAsync();
            }
        }

        /// <summary>
        ///     PhotoShoot service execute
        /// </summary>
        private void TogglePhotoShootServiceExecute()
        {
            // Playing sound effect for button
            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");

            // True - button is pushed - Working!
            IsRunning = false;

            if (loginForm == null || loginForm.IsClosed)
            {
                loginForm = new PasswordWindow();
                var loginFormViewModel = new LoginFormViewModel(loginForm);

                loginForm.DataContext = loginFormViewModel;
                loginForm.Owner = Application.Current.MainWindow;
                IsStreaming = false;
                faceDetectionService.CancelServiceAsync();
                loginFormViewModel.ShowWindow();
            }


            if (faceDetectionService.IsRunning)
            {
                return;
            }

            IsStreaming = true;
            faceDetectionService.RunServiceAsync();
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
            ToggleWebServiceCommand = new RelayCommand(ToggleWebServiceExecute);
            TogglePhotoShootServiceCommand = new RelayCommand(TogglePhotoShootServiceExecute);
            ToggleHelpCallCommand = new RelayCommand(ToogleHelpServiceExecute);
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