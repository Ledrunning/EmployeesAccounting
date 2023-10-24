using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.ViewModels;

namespace EA.DesktopApp.Services.ViewServices
{
    public class ModalViewModelFactory : IModalViewModelFactory
    {
        private readonly IWindowManager _windowManager;
        private readonly ISoundPlayerService _soundPlayerService;

        public ModalViewModelFactory(IWindowManager windowManager, ISoundPlayerService soundPlayerService)
        {
            _windowManager = windowManager;
            _soundPlayerService = soundPlayerService;
        }

        public ModalViewModel Create(string initialMessage)
        {
            return new ModalViewModel(_windowManager, _soundPlayerService, initialMessage);
        }
    }
}