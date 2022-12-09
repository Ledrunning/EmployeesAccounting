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
using static EA.DesktopApp.Event.ImageEvent;

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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly VideoCapture videoCapture;
        private CascadeClassifier eyeCascadeClassifier;
        private CascadeClassifier faceCascadeClassifier;
        private BackgroundWorker webCamWorker;

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

        public bool IsRunning => webCamWorker?.IsBusy ?? false;

        private void InitializeClassifier()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var path = Path.GetDirectoryName(assembly.Location);

                if (path != null)
                {
                    faceCascadeClassifier =
                        new CascadeClassifier(Path.Combine(path, "haarcascade_frontalface_default.xml"));
                    eyeCascadeClassifier = new CascadeClassifier(Path.Combine(path, "haarcascade_eye.xml"));
                }
                else
                {
                    logger.Error("Could not find haarcascade xml file");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not find clissifier files");
            }
        }

        public event ImageChangedEventHandler ImageChanged;

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
            webCamWorker?.CancelAsync();
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
                ImageChanged?.Invoke(image);
            }
        }

        private void DetectFaces(Image<Bgr, byte> image)
        {
            var grayFrame = image.Convert<Gray, byte>();
            var faces = GetRectangles(faceCascadeClassifier, grayFrame);
            var eyes = GetRectangles(eyeCascadeClassifier, grayFrame);

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