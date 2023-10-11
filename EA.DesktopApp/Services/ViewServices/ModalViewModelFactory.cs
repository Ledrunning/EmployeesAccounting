using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.ViewModels;

namespace EA.DesktopApp.Services.ViewServices
{
    public class ModalViewModelFactory : IModalViewModelFactory
    {
        private readonly IWindowManager _windowManager;

        public ModalViewModelFactory(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public ModalViewModel Create(string initialMessage)
        {
            return new ModalViewModel(_windowManager, initialMessage);
        }
    }
}