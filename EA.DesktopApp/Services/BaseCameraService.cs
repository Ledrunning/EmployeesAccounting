﻿using System.ComponentModel;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    public class BaseCameraService
    {
        private readonly VideoCapture _videoCapture;
        protected BackgroundWorker _webCamWorker;

        public BaseCameraService()
        {
            _videoCapture = new VideoCapture();
            InitializeWorkers();
        }

        public bool IsRunning => _webCamWorker?.IsBusy ?? false;

        public event ImageChangedEventHandler ImageChanged;

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

        /// <summary>
        ///     Draw image method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnWebCamWorker(object sender, DoWorkEventArgs e)
        {
            while (!_webCamWorker.CancellationPending)
            {
                var image = _videoCapture.QueryFrame().ToImage<Bgr, byte>();
                ImageChanged?.Invoke(image);
            }
        }
    }
}