using EA.DesktopApp.Event;

namespace EA.DesktopApp.Contracts
{
    public interface IFaceDetectionService
    {
        bool IsRunning { get; }
        event ImageChangedEventHandler FaceDetectionImageChanged;
        void Dispose();
        event ImageChangedEventHandler ImageChanged;

        /// <summary>
        ///     Async method for background work
        /// </summary>
        void RunServiceAsync();

        /// <summary>
        ///     Cancel Async method for background work
        /// </summary>
        void CancelServiceAsync();
    }
}