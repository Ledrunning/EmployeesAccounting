using System.Windows;
using Autofac;
using EA.DesktopApp.Contracts.ViewContracts;

namespace EA.DesktopApp.Services.ViewServices
{
    public class WindowFactory : IWindowFactory
    {
        private readonly ILifetimeScope _scope;

        public WindowFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Window GetWindow<TWindow>() where TWindow : Window
        {
            return _scope.Resolve<TWindow>();
        }
    }
}