using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Event;
using EA.DesktopApp.Models;
using EA.RecognizerEngine.Contracts;
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
        private readonly ILbphFaceRecognition _faceRecognitionService;

        /// <summary>
        ///     Capture stream from camera
        ///     And init background workers
        /// </summary>
        public FaceDetectionService(ILbphFaceRecognition faceRecognitionService)
        {
            _faceRecognitionService = faceRecognitionService;
            InitializeClassifier();
            ImageChanged -= OnFaceDetected;
            ImageChanged += OnFaceDetected;
        }

        public IReadOnlyList<EmployeeModel> Employees { get; set; }

        public event ImageChangedEventHandler FaceDetectionImageChanged;

        private void OnFaceDetected(Image<Bgr, byte> image)
        {
            DetectFaces(image);
        }

        private void DetectFaces(Image<Bgr, byte> image)
        {
            var grayFrame = image.Convert<Gray, byte>();
            var faces = GetRectangles(FaceCascadeClassifier, grayFrame);
            var eyes = GetRectangles(EyeCascadeClassifier, grayFrame);

            foreach (var face in faces)
            {
                // Recognize the face right after detection
                var faceImage = grayFrame.GetSubRect(face);
                var idPredict = _faceRecognitionService.Predict(faceImage);
                var employee = Employees.Single(s => s.Id == idPredict); // Ensure employees list is accessible
                var detectedEmployeeName = $"{employee?.Name} {employee?.LastName}";

                image.Draw(face, ImageProcessingConstants.RectanglesColor,
                    ImageProcessingConstants.RectangleThickness);
                SetBackgroundText(image, detectedEmployeeName,
                    face.Location,
                    ImageProcessingConstants.TextColor);

                foreach (var eye in eyes)
                {
                    image.Draw(eye, ImageProcessingConstants.RectanglesColor,
                        ImageProcessingConstants.RectangleThickness);
                }
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

        private static void SetBackgroundText(IInputOutputArray image, string text, Point point, Bgr textColor,
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