using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class which realize photoshoot logic
    /// </summary>
    public class PhotoShootService : WebCamService
    {
        public delegate void ImageWithDetectionChangedEventHandler(object sender, Image<Bgr, byte> image);

        /// <summary>
        ///     Init for face detection method
        /// </summary>
        public PhotoShootService()
        {
            InitializeServices();
        }

        public event ImageWithDetectionChangedEventHandler ImageWithDetectionChanged;

        /// <summary>
        ///     Event handler from web cam services
        /// </summary>
        private void InitializeServices()
        {
            ImageChanged += WebCamServiceImageChanged;
        }

        /// <summary>
        ///     Calling Image changet event delegate
        /// </summary>
        /// <param name="image"></param>
        private void RaiseImageWithDetectionChangedEvent(Image<Bgr, byte> image)
        {
            try
            {
                ImageWithDetectionChanged?.Invoke(this, image);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        ///     Image changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void WebCamServiceImageChanged(object sender, Image<Bgr, byte> image)
        {
            RaiseImageWithDetectionChangedEvent(image);
        }
    }
}