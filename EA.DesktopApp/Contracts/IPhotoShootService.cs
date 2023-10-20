using EA.DesktopApp.Event;
using Emgu.CV.Structure;
using Emgu.CV;

namespace EA.DesktopApp.Contracts
{
    public interface IPhotoShootService
    {
        event ImageChangedEventHandler PhotoImageChanged;
        void Dispose();
        event ImageChangedEventHandler ImageChanged;
        Image<Gray, byte> CropFaceFromImage(Image<Bgr, byte> image);

        /// <summary>
        ///     Async method for background work
        /// </summary>
        void RunServiceAsync(int cameraIndex = 0);

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        void CancelServiceAsync();
    }
}