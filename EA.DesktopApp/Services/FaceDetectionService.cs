using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;
using NLog;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class for camera calls face and eyes detection
    ///     EMGU version 4.6.0.5131
    ///     Libs:
    ///     1.Emgu.CV
    ///     2.Emgu.CV.Bitmap
    ///     6.nvcuda.dll needed if have not Nvidia GPU on computer
    ///     All libs must to be copied into the bin folder
    /// </summary>
    public class FaceDetectionService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly VideoCapture _videoCapture;
        private CascadeClassifier _eyeCascadeClassifier;
        private CascadeClassifier _faceCascadeClassifier;
        private BackgroundWorker _webCamWorker;

        /// <summary>
        ///     Capture stream from camera
        ///     And init background workers
        /// </summary>
        public FaceDetectionService()
        {
            _videoCapture = new VideoCapture();
            InitializeWorkers();
            InitializeClassifier();
        }

        public bool IsRunning => _webCamWorker?.IsBusy ?? false;

        private void InitializeClassifier()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var path = Path.GetDirectoryName(assembly.Location);

                if (path != null)
                {
                    _faceCascadeClassifier =
                        new CascadeClassifier(Path.Combine(path, "haarcascade_frontalface_default.xml"));
                    _eyeCascadeClassifier = new CascadeClassifier(Path.Combine(path, "haarcascade_eye.xml"));
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
        private void OnWebCamWorker(object sender, DoWorkEventArgs e)
        {
            while (!_webCamWorker.CancellationPending)
            {
                var image = _videoCapture.QueryFrame().ToImage<Bgr, byte>();
                DetectFaces(image);
                ImageChanged?.Invoke(image);
            }
        }

        private void DetectFaces(Image<Bgr, byte> image)
        {
            var grayFrame = image.Convert<Gray, byte>();
            var faces = GetRectangles(_faceCascadeClassifier, grayFrame);
            var eyes = GetRectangles(_eyeCascadeClassifier, grayFrame);

            foreach (var face in faces)
            {
                image.Draw(face, ImageProcessingConstants.RectanglesColor,
                    ImageProcessingConstants.RectangleThickness);

                foreach (var eye in eyes)
                {
                    image.Draw(eye, ImageProcessingConstants.RectanglesColor,
                        ImageProcessingConstants.RectangleThickness);
                }
            }
        }

        private static Rectangle[] GetRectangles(CascadeClassifier classifier, IInputArray grayFrame)
        {
            var rectangles = classifier.DetectMultiScale(grayFrame,
                ImageProcessingConstants.ScaleFactor,
                ImageProcessingConstants.MinimumNeighbors,
                Size.Empty);
            return rectangles;
        }
    }
}