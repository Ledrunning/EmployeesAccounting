using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    public class BaseCameraService : IDisposable
    {
        private VideoCapture _videoCapture;
        protected BackgroundWorker _webCamWorker;

        public BaseCameraService()
        {
            _videoCapture = new VideoCapture();
            InitializeWorkers();
        }

        public void Dispose()
        {
            _webCamWorker.DoWork -= OnWebCamWorker;
            _webCamWorker.CancelAsync();
            _videoCapture?.Dispose();
            _videoCapture = null;
            _webCamWorker?.Dispose();
        }

        public bool IsRunning => _webCamWorker?.IsBusy ?? false;

        public event ImageChangedEventHandler ImageChanged;

        /// <summary>
        ///     Async method for background work
        /// </summary>
        public void RunServiceAsync()
        {
            InitializeVideoCapture();
            InitializeWorkers();
            _webCamWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        public void CancelServiceAsync()
        {
            _webCamWorker?.CancelAsync();
        }

        /// <summary>
        ///     Method for background worker init
        /// </summary>
        private void InitializeWorkers()
        {
            _webCamWorker = new BackgroundWorker();
            _webCamWorker.WorkerSupportsCancellation = true;
            _webCamWorker.DoWork += OnWebCamWorker;
        }

        // <summary>
        ///     Draw image method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnWebCamWorker(object sender, DoWorkEventArgs e)
        {
            if (_webCamWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            while (!_webCamWorker.CancellationPending)
            {
                var image = _videoCapture.QueryFrame().ToImage<Bgr, byte>();
                ImageChanged?.Invoke(image);
            }
        }

        private void InitializeVideoCapture()
        {
            try
            {
                if (_videoCapture == null)
                {
                    _videoCapture = new VideoCapture();

                    if (!_videoCapture.IsOpened)
                    {
                        _videoCapture.Start();
                    }

                    _videoCapture.Stop();
                }
            }
            catch (Exception e)
            {
                //logger.Error("Video capture failed! {e}", e);
            }
        }
    }
}
