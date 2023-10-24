using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.Services
{
    public class BaseCameraService : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(true);
        private VideoCapture _videoCapture;
        private CancellationTokenSource _cancellationToken;
        private CancellationToken _token;
        public const int CamerasQuantity = 10;
        protected CascadeClassifier EyeCascadeClassifier;
        protected CascadeClassifier FaceCascadeClassifier;

        public bool IsRunning { get; set; }

        public void Dispose()
        {
            _videoCapture?.Dispose();
            _videoCapture = null;
        }

        public event ImageChangedEventHandler ImageChanged;

        /// <summary>
        ///     Async method for background work
        /// </summary>
        public void RunServiceAsync(int cameraIndex = 0)
        {
            _cancellationToken = new CancellationTokenSource();
            _token = _cancellationToken.Token;
            IsRunning = true;
            InitializeVideoCapture(cameraIndex);
            WebCameraWorker();
        }

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        public void CancelServiceAsync()
        {
            IsRunning = false;
            _cancellationToken?.Cancel();

            // Wait for WebCameraWorker to finish its operation
            _resetEvent.WaitOne();

            Dispose();
        }


        /// <summary>
        ///     Grab image processing method
        /// </summary>
        private void WebCameraWorker()
        {
            _resetEvent.Reset();

            Task.Run(() =>
            {
                try
                {
                    while (!_token.IsCancellationRequested)
                    {
                        var image = _videoCapture?.QueryFrame().ToImage<Bgr, byte>();
                        ImageChanged?.Invoke(image);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Web camera worker error {ex}", ex);
                }
                finally
                {
                    _resetEvent.Set();
                }
            }, _token).ConfigureAwait(false);
        }


        private void InitializeVideoCapture(int cameraIndex)
        {
            if (_videoCapture != null)
            {
                return;
            }

            _videoCapture = new VideoCapture(cameraIndex);
        }

        protected void InitializeClassifier()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var path = Path.GetDirectoryName(assembly.Location);

                if (path != null)
                {
                    FaceCascadeClassifier =
                        new CascadeClassifier(Path.Combine(path, "haarcascade_frontalface_default.xml"));
                    EyeCascadeClassifier = new CascadeClassifier(Path.Combine(path, "haarcascade_eye.xml"));
                }
                else
                {
                    Logger.Error("Could not find haarcascade xml file");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not find clissifier files");
            }
        }
    }
}