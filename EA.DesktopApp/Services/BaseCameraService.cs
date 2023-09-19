using System;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    public class BaseCameraService : IDisposable
    {
        private VideoCapture _videoCapture;
        private CancellationTokenSource cancellationToken;
        private CancellationToken token;

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
            cancellationToken = new CancellationTokenSource();
            token = cancellationToken.Token;
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
            cancellationToken?.Cancel();
            Dispose();
        }

        /// <summary>
        ///     Grab image processing method
        /// </summary>
        private void WebCameraWorker()
        {
            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var image = _videoCapture?.QueryFrame().ToImage<Bgr, byte>();
                    ImageChanged?.Invoke(image);
                }
            }, token).ConfigureAwait(false);
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