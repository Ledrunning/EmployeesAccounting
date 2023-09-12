namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IWindowManager
    {
        void ShowModalWindow();
        void CloseModalWindow();
        void ShowLoginWindow();
        void CloseLoginWindow();
        void ShowRegistrationWindow();
        void CloseRegistrationWindow();
        void ShowAdminWindow();
        void CloseAdminWindow();
    }
}