using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
    public class FaceDetectionService : BaseCameraService, IFaceDetectionService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private CascadeClassifier _eyeCascadeClassifier;
        private CascadeClassifier _faceCascadeClassifier;

        /// <summary>
        ///     Capture stream from camera
        ///     And init background workers
        /// </summary>
        public FaceDetectionService()
        {
            InitializeClassifier();
            ImageChanged -= OnFaceDetectionFound;
            ImageChanged += OnFaceDetectionFound;
        }

        public string EmployeeName { get; set; }

        public event ImageChangedEventHandler FaceDetectionImageChanged;

        //TODO Put EmployeeName instead of hardcoded literal 
        private void OnFaceDetectionFound(Image<Bgr, byte> image)
        {
            DetectFaces(image);
        }

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

                SetBackgroundText(image, EmployeeName,
                    face.Location,
                    ImageProcessingConstants.TextColor);
            }

            FaceDetectionImageChanged?.Invoke(image);
        }

        private static Rectangle[] GetRectangles(CascadeClassifier classifier, IInputArray grayFrame)
        {
            var rectangles = classifier.DetectMultiScale(grayFrame,
                ImageProcessingConstants.ScaleFactor,
                ImageProcessingConstants.MinimumNeighbors,
                Size.Empty);
            return rectangles;
        }

        private void SetBackgroundText(IInputOutputArray image, string text, Point point, Bgr textColor,
            double fontScale = 1.0)
        {
            CvInvoke.PutText(
                image,
                text,
                point,
                FontFace.HersheyComplex,
                fontScale,
                textColor.MCvScalar);
        }
    }
}