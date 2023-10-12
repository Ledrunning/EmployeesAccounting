using EA.DesktopApp.ViewModels;

namespace EA.DesktopApp.Contracts.ViewContracts
{
    public interface IModalViewModelFactory
    {
        ModalViewModel Create(string initialMessage);
    }
}