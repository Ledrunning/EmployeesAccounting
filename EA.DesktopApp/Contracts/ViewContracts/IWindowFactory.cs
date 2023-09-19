using System.Windows;

namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IWindowFactory
    {
        Window GetWindow<TWindow>() where TWindow : Window;

        TWindow GetWindow<TWindow>(string message) where TWindow : Window;
    }
}