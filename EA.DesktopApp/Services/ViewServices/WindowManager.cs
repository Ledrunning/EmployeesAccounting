using System.Windows;
using Autofac;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.View;

namespace EA.DesktopApp.Services.ViewServices
{
    public class WindowManager : IWindowManager
    {
        private readonly IWindowFactory _windowFactory;
        private Window _adminWindow;
        private Window _modalWindow;
        private Window _loginWindow;
        private Window _registrationWindow;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowModalWindow()
        {
            if (_modalWindow == null || !_modalWindow.IsLoaded)
            {
                _modalWindow = _modalWindow ?? _windowFactory.GetWindow<ModalWindow>();
            }

            _modalWindow.Owner = Application.Current.MainWindow;
            _modalWindow.Show();
        }

        public void CloseModalWindow()
        {
            _modalWindow?.Close();
            _modalWindow = null;
        }

        public void ShowLoginWindow()
        {
            if (_loginWindow == null || !_loginWindow.IsLoaded)
            {
                _loginWindow = _loginWindow ?? _windowFactory.GetWindow<LoginWindow>();
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
                _registrationWindow = _registrationWindow ?? _windowFactory.GetWindow<RegistrationForm>();
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
            _adminWindow = _adminWindow ?? _windowFactory.GetWindow<AdminForm>();
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