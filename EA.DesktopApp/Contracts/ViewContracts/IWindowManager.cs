namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IWindowManager
    {
        void ShowLoginWindow();
        void CloseLoginWindow();
        void ShowRegistrationWindow();
        void CloseRegistrationWindow();
        void ShowAdminWindow();
        void CloseAdminWindow();
    }
}