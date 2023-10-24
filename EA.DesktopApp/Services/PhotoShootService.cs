using System.Drawing;
using EA.DesktopApp.Constants;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class which realize photoshoot logic
    /// </summary>
    public class PhotoShootService : BaseCameraService, IPhotoShootService
    {
        /// <summary>
        ///     Init for face detection method
        /// </summary>
        public PhotoShootService()
        {
            InitializeServices();
            InitializeClassifier();
        }

        public event ImageChangedEventHandler PhotoImageChanged;

        public Image<Gray, byte> CropFaceFromImage(Image<Bgr, byte> image)
        {
            // Load the image
            var grayImage = image.Convert<Gray, byte>();

            // Detect faces
            var faces = FaceCascadeClassifier.DetectMultiScale(
                grayImage,
                ImageProcessingConstants.ScaleFactor,
                ImageProcessingConstants.MinimumNeighbors,
                Size.Empty); // min size

            // If no faces or multiple faces are detected, handle appropriately.
            // For simplicity, we'll just take the first detected face in this example.
            if (faces.Length == 0)
            {
                return null; // No face detected
            }

            var face = faces[0];

            // Crop the face from the image 
            var croppedFace = grayImage.Copy(face);
            var x = croppedFace.Resize(ImageProcessingConstants.GrayPhotoWidth,
                ImageProcessingConstants.GrayPhotoHeight, Inter.Cubic);
            return x;
        }

        /// <summary>
        ///     Event handler from web cam services
        /// </summary>
        private void InitializeServices()
        {
            ImageChanged += OnImageChanged;
        }

        /// <summary>
        ///     Image changed event
        /// </summary>
        /// <param name="image"></param>
        private void OnImageChanged(Image<Bgr, byte> image)
        {
            PhotoImageChanged?.Invoke(image);
        }
    }
}