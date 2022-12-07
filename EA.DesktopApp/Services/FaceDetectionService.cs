using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;
using NLog;

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
    public class FaceDetectionService
    {
        public delegate void ImageChangedEventHndler(object sender, Image<Bgr, byte> image);

        private readonly VideoCapture videoCapture;
        private BackgroundWorker webCamWorker;
        private CascadeClassifier cascadeClassifier;

        /// <summary>
        ///     Capture stream from camera
        ///     And init background workers
        /// </summary>
        public FaceDetectionService()
        {
            videoCapture = new VideoCapture();
            InitializeWorkers();
            InitializeClassifier();
        }

        private void InitializeClassifier()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(assembly.Location);

            if (path != null)
            {
                cascadeClassifier = new CascadeClassifier(Path.Combine(path, "haarcascade_frontalface_default.xml"));
            }
            else
            {
                //logger.Error("Could not find haarcascade xml file");
            }
        }

        public bool IsRunning => webCamWorker?.IsBusy ?? false;

        public event ImageChangedEventHndler ImageChanged;

        /// <summary>
        ///     Async method for background work
        /// </summary>
        public void RunServiceAsync()
        {
            webCamWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        public void CancelServiceAsync()
        {
            if (webCamWorker != null)
            {
                webCamWorker.CancelAsync();
            }
        }
        
        /// <summary>
        ///     Method for background worker init
        /// </summary>
        private void InitializeWorkers()
        {
            webCamWorker = new BackgroundWorker();
            webCamWorker.WorkerSupportsCancellation = true;
            webCamWorker.DoWork += OnWebCamWorker;
        }

        /// <summary>
        ///     Draw image method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebCamWorker(object sender, DoWorkEventArgs e)
        {
            while (!webCamWorker.CancellationPending)
            {
                var image = videoCapture.QueryFrame().ToImage<Bgr, byte>();
                DetectFaces(image);
            }
        }

        private void DetectFaces(Image<Bgr, byte> image)
        {
            var grayFrame = image.Convert<Gray, byte>();
            var faces = cascadeClassifier.DetectMultiScale(grayFrame,
                1.1,
                10,
                Size.Empty); //the actual face detection happens here
            foreach (var face in faces)
            {
                image.Draw(face, new Bgr(Color.Aqua),
                    2); //the detected face(s) is highlighted here using a box that is drawn around it/them
            }
        }
    }
}