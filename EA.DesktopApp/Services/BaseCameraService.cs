using System;
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
        public void RunServiceAsync()
        {
            _cancellationToken = new CancellationTokenSource();
            _token = _cancellationToken.Token;
            IsRunning = true;
            InitializeVideoCapture();
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
                    throw;
                }
                finally
                {
                    _resetEvent.Set();
                }
            }, _token).ConfigureAwait(false);
        }


        private void InitializeVideoCapture()
        {
            if (_videoCapture != null)
            {
                return;
            }

            _videoCapture = new VideoCapture();
        }
    }
}