using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    // TODO: Dont forget about cancellation token
    public class RedactorViewModel : BaseViewModel
    {
        private readonly IEmployeeGatewayService _employeeService;

        private ObservableCollection<EmployeeModel> _employees;

        private EmployeeModel _selectedProduct;

        public RedactorViewModel(IEmployeeGatewayService employeeService)
        {
            _employeeService = employeeService;
            InitializeCommands();

            //TODO need to think, how use it asynchronously
            LoadData().ConfigureAwait(false);
        }

        public ObservableCollection<EmployeeModel> AllEmployees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(AllEmployees));
            }
        }

        public EmployeeModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                // Handle the selection change
                OnProductSelected();
            }
        }

        public ICommand ToggleDeleteCommand { get; private set; }

        public ICommand ToggleUpdateToDbCommand { get; private set; }

        public ICommand ToggleClearFormCommand { get; private set; }

        private void OnProductSelected()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            // Do something with the selected product, e.g., display its details
            //MessageBox.Show($"Selected Product: {SelectedProduct.Name}");
            PersonName = SelectedProduct.Name;
            PersonLastName = SelectedProduct.LastName;
            PersonDepartment = SelectedProduct.Department;
        }

        private void InitializeCommands()
        {
            ToggleDeleteCommand = new RelayCommand(async () => await ToggleDeleteExecute());
            ToggleUpdateToDbCommand = new RelayCommand(async () => await ToggleUpdateExecute());
            ToggleClearFormCommand = new RelayCommand(ToggleClearFields);
        }

        private async Task LoadData()
        {
            try
            {
                var loadedData = await _employeeService.GetAllEmployeeAsync(CancellationToken.None);
                AllEmployees = new ObservableCollection<EmployeeModel>(loadedData.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ToggleDeleteExecute()
        {
            try
            {
                await _employeeService.DeleteAsync(SelectedProduct.Id, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ToggleUpdateExecute()
        {
            try
            {
                var updatedEmployeeData = new EmployeeModel()
                {
                    DateTime = DateTimeOffset.UtcNow,
                    Name = SelectedProduct.Name,
                    LastName = SelectedProduct.LastName,
                    Department = SelectedProduct.Department,
                };

                await _employeeService.UpdateAsync(updatedEmployeeData, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void ToggleClearFields()
        {
            ClearFields();
        }
    }
}