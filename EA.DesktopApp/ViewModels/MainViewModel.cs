using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Converters;
using EA.DesktopApp.Enum;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.Services.ViewServices;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
using EA.RecognizerEngine.Contracts;
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
        private readonly IEigenFaceRecognition _eigenRecognizer;
        private readonly IEmployeeGatewayService _employeeGatewayService;
        private readonly IFaceDetectionService _faceDetectionService;
        private readonly ISoundPlayerService _soundPlayerHelper;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;
        public event Action<OpenWindowBroker> RequestOpenWindow;

        private string _currentTimeDate;
        private string _detectionHint;

        private Bitmap _frame;

        private bool _isRunning;

        private bool _isStreaming;

        private string _selectedCamera;

        private IReadOnlyList<EmployeeModel> employees;

        /// <summary>
        ///     Timer
        /// </summary>
        public DispatcherTimer Timer;

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainViewModel(
            IFaceDetectionService faceDetectionService,
            IEigenFaceRecognition eigenRecognizer,
            IEmployeeGatewayService employeeGatewayService,
            IWindowManager windowManager,
            ISoundPlayerService soundPlayerHelper,
            CancellationToken token)
        {
            _faceDetectionService = faceDetectionService;
            _eigenRecognizer = eigenRecognizer;
            _employeeGatewayService = employeeGatewayService;
            _windowManager = windowManager;
            InitializeServices();
            LoadAvailableCameras();
            InitializeCommands();
            TimeTicker();
            _soundPlayerHelper = soundPlayerHelper;
            _token = token;
            DetectionHint = ProgramResources.StartDetectorTooltipMessage;
        }

        public ObservableCollection<string> AvailableCameras { get; } = new ObservableCollection<string>();

        public string SelectedCamera
        {
            get => _selectedCamera;
            set => SetField(ref _selectedCamera, value);
        }

        /// <summary>
        ///     Get start tooltip
        /// </summary>
        public string DetectionHint
        {
            get => _detectionHint;
            set => SetField(ref _detectionHint, value, nameof(DetectionHint));
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

        public ICommand ToggleOpenEditCommand { get; private set; }

        private async void InitializeServices()
        {
            Logger.Info("Initialize of all services.....");
            _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;

            if (await _employeeGatewayService.IsServerAvailableAsync(_token))
            {
                await FetchFacesFromDbAndTrain();
                Logger.Info("Data has been fetched!");
            }
            else
            {
                Logger.Info("Server is offline!");
                _windowManager.ShowModalWindow("Server is offline");
            }
        }

        private void LoadAvailableCameras()
        {
            for (var i = 0; i < BaseCameraService.CamerasQuantity; i++)
            {
                try
                {
                    using (var tempCapture = new VideoCapture(i))
                    {
                        if (tempCapture.IsOpened)
                        {
                            AvailableCameras.Add(i.ToString());
                        }
                    }

                    // Set the first available camera as selected
                    if (AvailableCameras.Count <= 0)
                    {
                        continue;
                    }

                    SelectedCamera = AvailableCameras[0];
                }
                catch
                {
                    Logger.Info("Failed to load cameras");
                    _windowManager.ShowModalWindow("Failed to load cameras");
                }
            }
        }

        private async Task FetchFacesFromDbAndTrain()
        {
            try
            {
                employees = await _employeeGatewayService.GetAllWithPhotoAsync(_token);
                _faceDetectionService.Employees = employees;
                foreach (var employee in employees)
                {
                    var depthImage = EmguFormatImageConverter.ByteArrayToGrayImage(employee.Photo);

                    //TODO: Maybe I'll apply it for perfomance
                    // File.WriteAllBytes($"D:\\Trash\\{employee.PhotoName}", employee.Photo);
                    // var depthImage = new Image<Gray, byte>($"D:\\Trash\\{employee.PhotoName}");

                    _eigenRecognizer.AddTrainingImage(depthImage, Convert.ToInt32(employee.Id));
                }

                _eigenRecognizer.Train();
            }
            catch (Exception e)
            {
                Logger.Info("Failed to train recognizer {e}", e);
                _windowManager.ShowModalWindow("Failed to train recognizer");
            }
        }

        /// <summary>
        ///     Service From WebCamService
        /// </summary>
        private void FaceDetectionServiceExecute()
        {
            DetectionHint = IsStreaming
                ? ProgramResources.StartDetectorTooltipMessage
                : ProgramResources.StopDetectorTooltipMessage;

            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            if (_faceDetectionService != null && !_faceDetectionService.IsRunning)
            {
                _faceDetectionService.FaceDetectionImageChanged += OnImageChanged;
                IsStreaming = true;

                var cameraIndex = int.Parse(SelectedCamera);

                _faceDetectionService.RunServiceAsync(cameraIndex);
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
        }

        /// <summary>
        ///     PhotoShoot service execute
        /// </summary>
        private void TogglePhotoShootServiceExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

            // True - button is pushed - Working!
            IsRunning = false;
            IsStreaming = false;
            StopFaceDetectionService();
            _windowManager.ShowWindow<LoginWindow>();
        }

        /// <summary>
        ///     Execute User Help method
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
                _windowManager.ShowModalWindow("An error occurred in opening the Help file!");
            }
        }

        public void ToggleOpenEditExecute()
        {
            _windowManager.ShowWindow<RedactorForm>();
        }

        /// <summary>
        ///     Initialize all commands from main view model
        /// </summary>
        private void InitializeCommands()
        {
            ToggleWebServiceCommand = new RelayCommand(FaceDetectionServiceExecute);
            TogglePhotoShootServiceCommand = new RelayCommand(TogglePhotoShootServiceExecute);
            ToggleHelpCallCommand = new RelayCommand(ToggleHelpServiceExecute);
            ToggleOpenEditCommand = new RelayCommand(ToggleOpenEditExecute);
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