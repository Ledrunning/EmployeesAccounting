using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class RedactorViewModel : BaseViewModel
    {
        private readonly IEmployeeGatewayService _eployeeService;

        public RedactorViewModel(IEmployeeGatewayService eployeeService)
        {
            _eployeeService = eployeeService;
            InitializeCommands();
        }

        public ICommand ToggleDeleteCommand { get; private set; }

        public ICommand ToggleUpdateToDbCommand { get; private set; }

        public ICommand ToggleClearFormCommand { get; private set; }

        private void InitializeCommands()
        {
            ToggleDeleteCommand = new RelayCommand(ToggleDeleteExecute);
            ToggleUpdateToDbCommand = new RelayCommand(ToggleUpdateExecute);
            ToggleClearFormCommand = new RelayCommand(ToggleClearFields);
        }

        private void ToggleDeleteExecute()
        {
            
        }

        private void ToggleUpdateExecute()
        {
        }

        private void ToggleClearFields()
        {
        }
    }
}