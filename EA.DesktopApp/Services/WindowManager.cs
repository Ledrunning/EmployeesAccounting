using System.Windows;
using EA.DesktopApp.Contracts.ViewContracts;

namespace EA.DesktopApp.Services
{
    public class WindowManager : IWindowManager
    {
        private readonly IWindowFactory _windowFactory;
        private Window _adminWindow;
        private Window _loginWindow;
        private Window _registrationWindow;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowLoginWindow()
        {
            if (_loginWindow == null || !_loginWindow.IsLoaded)
            {
                _loginWindow = _loginWindow ?? _windowFactory.CreateLoginWindow();
            }

            _loginWindow.Owner = Application.Current.MainWindow;
            _loginWindow.Show();
        }

        public void CloseLoginWindow()
        {
            _loginWindow?.Close();
            _loginWindow = null;
        }

        public void ShowRegistrationWindow()
        {
            if (_registrationWindow == null || !_registrationWindow.IsLoaded)
            {
                _registrationWindow = _registrationWindow ?? _windowFactory.CreateRegistrationForm();
            }

            _registrationWindow.Owner = Application.Current.MainWindow;
            _registrationWindow.Show();
        }

        public void CloseRegistrationWindow()
        {
            _registrationWindow?.Close();
            _registrationWindow = null;
        }

        public void ShowAdminWindow()
        {
            _adminWindow = _adminWindow ?? _windowFactory.CreateAdminForm();
            _adminWindow.Owner = Application.Current.MainWindow;
            _adminWindow.Show();
        }

        public void CloseAdminWindow()
        {
            _adminWindow?.Close();
            _adminWindow = null;
        }
    }
}