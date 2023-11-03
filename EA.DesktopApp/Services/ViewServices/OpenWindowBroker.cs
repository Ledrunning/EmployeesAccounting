using EA.DesktopApp.Enum;

namespace EA.DesktopApp.Services.ViewServices
{
    public class OpenWindowBroker
    {
        public WindowType WindowType { get; }

        public OpenWindowBroker(WindowType windowType)
        {
            WindowType = windowType;
        }
    }
}