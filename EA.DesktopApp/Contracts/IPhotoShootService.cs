using EA.DesktopApp.Event;

namespace EA.DesktopApp.Contracts
{
    public interface IPhotoShootService
    {
        event ImageChangedEventHandler PhotoImageChanged;
        void Dispose();
        event ImageChangedEventHandler ImageChanged;

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