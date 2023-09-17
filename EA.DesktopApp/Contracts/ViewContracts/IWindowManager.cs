using System.Windows;

namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IWindowManager
    {
        void ShowWindow<T>() where T : Window, new();
        void CloseWindow<T>() where T : Window;
        void ShowModalWindow(string message);
    }
}