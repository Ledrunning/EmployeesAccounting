using System.Windows;
using Autofac;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.View;

namespace EA.DesktopApp.Services.ViewServices
{
    public class WindowFactory : IWindowFactory
    {
        private readonly ILifetimeScope _scope;

        public WindowFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Window CreateLoginWindow()
        {
            return _scope.Resolve<LoginWindow>();
        }

        public Window CreateModalWindow()
        {
            return _scope.Resolve<ModalWindow>();
        }
    }
}