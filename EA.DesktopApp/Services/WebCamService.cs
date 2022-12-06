using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class for camera call and async background works
    ///     EMGU version 2.4.2.1777
    ///     Libs:
    ///     1.Emgu.CV
    ///     2.Emgu.CV.GPU
    ///     3.Emgu.CV.ML
    ///     4.Emgu.CV.UI
    ///     5.Emgu.Util
    ///     6.nvcuda.dll needed if have not Nvidia GPU on computer
    ///     All libs must to be copied into the bin folder
    /// </summary>
    public class WebCamService
    {
        public delegate void ImageChangedEventHndler(object sender, Image<Bgr, byte> image);

        private readonly Capture _capture;
        private BackgroundWorker _webCamWorker;

        /// <summary>
        ///     Capture stream from camera
        ///     And init background workers
        /// </summary>
        public WebCamService()
        {
            _capture = new Capture();
            InitializeWorkers();
        }

        public bool IsRunning => _webCamWorker != null ? _webCamWorker.IsBusy : false;

        public event ImageChangedEventHndler ImageChanged;

        /// <summary>
        ///     Async method for background work
        /// </summary>
        public void RunServiceAsync()
        {
            _webCamWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        public void CancelServiceAsync()
        {
            if (_webCamWorker != null)
            {
                _webCamWorker.CancelAsync();
            }
        }

        /// <summary>
        ///     Method for calling ImageChanged delegate
        /// </summary>
        /// <param name="image"></param>
        private void RaiseImageChangedEvent(Image<Bgr, byte> image)
        {
            ImageChanged?.Invoke(this, image);
        }

        /// <summary>
        ///     Method for background worker init
        /// </summary>
        private void InitializeWorkers()
        {
            _webCamWorker = new BackgroundWorker();
            _webCamWorker.WorkerSupportsCancellation = true;
            _webCamWorker.DoWork += _webCamWorker_DoWork;
            _webCamWorker.RunWorkerCompleted += _webCamWorker_RunWorkerCompleted;
        }

        /// <summary>
        ///     Draw image method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _webCamWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_webCamWorker.CancellationPending)
            {
                RaiseImageChangedEvent(_capture.QueryFrame().Copy());
            }
        }

        private void _webCamWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}