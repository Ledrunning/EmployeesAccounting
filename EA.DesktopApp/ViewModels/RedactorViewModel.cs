using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.ViewModels.Commands;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    // TODO: Dont forget about cancellation token
    /// <summary>
    ///     If your data set is relatively small and doesn't change frequently
    ///     (especially by external systems or other users),
    ///     updating the ObservableCollection directly is a good choice
    ///     due to the performance and immediate feedback benefits.
    /// </summary>
    public class RedactorViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmployeeGatewayService _employeeService;
        private readonly IWindowManager _windowManager;

        private ObservableCollection<EmployeeModel> _employees;

        private EmployeeModel _selectedProduct;

        public RedactorViewModel(IWindowManager windowManager, IEmployeeGatewayService employeeService)
        {
            _windowManager = windowManager;
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
                Logger.Info("Failed to load employees from server {e}", e);
                _windowManager.ShowModalWindow("Failed to load employees from server");
            }
        }

        private async Task ToggleDeleteExecute()
        {
            try
            {
                await _employeeService.DeleteAsync(SelectedProduct.Id, CancellationToken.None);
                AllEmployees.Remove(SelectedProduct);
            }
            catch (Exception e)
            {
                Logger.Info("Failed to delete employee {e}", e);
                _windowManager.ShowModalWindow("Failed to delete employee");
            }
        }

        private async Task ToggleUpdateExecute()
        {
            try
            {
                var updatedEmployeeData = new EmployeeModel
                {
                    DateTime = DateTimeOffset.UtcNow,
                    Name = SelectedProduct.Name,
                    LastName = SelectedProduct.LastName,
                    Department = SelectedProduct.Department
                };

                await _employeeService.UpdateAsync(updatedEmployeeData, CancellationToken.None);

                UpdateGridCollection(updatedEmployeeData);
            }
            catch (Exception e)
            {
                Logger.Info("Failed to update employee data {e}", e);
                _windowManager.ShowModalWindow("Failed to update employee data");
            }
        }

        private void UpdateGridCollection(EmployeeModel updatedEmployeeData)
        {
            // Find the employee in the ObservableCollection and update its properties
            var employeeToUpdate = AllEmployees.FirstOrDefault(e => e.Id == SelectedProduct.Id);
            if (employeeToUpdate != null)
            {
                employeeToUpdate.DateTime = updatedEmployeeData.DateTime;
                employeeToUpdate.Name = updatedEmployeeData.Name;
                employeeToUpdate.LastName = updatedEmployeeData.LastName;
                employeeToUpdate.Department = updatedEmployeeData.Department;
            }
        }

        private void ToggleClearFields()
        {
            ClearFields();
        }
    }
}