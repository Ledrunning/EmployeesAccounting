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

        public TWindow GetWindow<TWindow>(string message) where TWindow : Window
        {
            var viewModelFactory = _scope.Resolve<IModalViewModelFactory>();
            var viewModel = viewModelFactory.Create(message);
            return _scope.Resolve<TWindow>(new NamedParameter("viewModel", viewModel));
        }
    }
}