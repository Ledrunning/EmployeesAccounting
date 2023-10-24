using EA.DesktopApp.Event;
using EA.DesktopApp.Models;
using System.Collections.Generic;

namespace EA.DesktopApp.Contracts
{
    public interface IFaceDetectionService
    {
        IReadOnlyList<EmployeeModel> Employees { get; set; }
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