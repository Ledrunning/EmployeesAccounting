using System;
using System.Diagnostics;
using EA.DesktopApp.Event;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class which realize photoshoot logic
    /// </summary>
    public class PhotoShootService : FaceDetectionService
    {
        /// <summary>
        ///     Init for face detection method
        /// </summary>
        public PhotoShootService()
        {
            InitializeServices();
        }

        public event ImageChangedEventHandler ImageChanged;

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
            ImageChanged?.Invoke(image);
        }
    }
}