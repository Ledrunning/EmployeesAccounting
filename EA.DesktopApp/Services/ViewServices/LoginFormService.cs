using Autofac;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.View;

namespace EA.DesktopApp.Services.ViewServices
{
    public class LoginFormService : ILoginFormService
    {
        private readonly IContainer _container;
        private LoginWindow _loginWindow;

        public LoginFormService()
        {
           _loginWindow = _container.Resolve<LoginWindow>();
        }

        public void ShowLoginWindow()
        {
            _loginWindow = _container.Resolve<LoginWindow>();
            _loginWindow.ShowDialog();
        }

        public void Close()
        {
            _loginWindow = new LoginWindow();
            _loginWindow.Close();
        }
    }
}