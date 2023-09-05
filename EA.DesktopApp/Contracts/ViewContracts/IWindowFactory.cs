using System.Windows;

namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IWindowFactory
    {
        Window CreateLoginWindow();
        Window CreateModalWindow();
        Window CreateAdminWindow();
    }
}