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
    public class PhotoShootService : BaseCameraService
    {

        private static PhotoShootService _instance;

        //TODO: before I used IoC
        public static PhotoShootService GetInstance()
        {
            return _instance ?? (_instance = new PhotoShootService());
        }

        /// <summary>
        ///     Init for face detection method
        /// </summary>
        public PhotoShootService()
        {
            InitializeServices();
        }

        public event ImageChangedEventHandler PhotoImageChanged;

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