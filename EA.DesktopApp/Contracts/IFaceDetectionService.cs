using EA.DesktopApp.Event;

namespace EA.DesktopApp.Contracts
{
    public interface IFaceDetectionService
    {
        string EmployeeName { get; set; }
        bool IsRunning { get; }
        event ImageChangedEventHandler FaceDetectionImageChanged;
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