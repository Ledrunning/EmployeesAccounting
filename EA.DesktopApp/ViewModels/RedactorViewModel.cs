using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    public class RedactorViewModel : BaseViewModel, IAsyncInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmployeeGatewayService _employeeService;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;
        private ObservableCollection<EmployeeModel> _employees;
        private EmployeeModel _selectedEmployee;
        private readonly ISoundPlayerService _soundPlayerHelper;

        public RedactorViewModel(IWindowManager windowManager, IEmployeeGatewayService employeeService,
            CancellationToken token, ISoundPlayerService soundPlayerHelper)
        {
            _windowManager = windowManager;
            _employeeService = employeeService;
            _token = token;
            _soundPlayerHelper = soundPlayerHelper;
            InitializeCommands();
        }

        public ObservableCollection<EmployeeModel> AllEmployees
        {
            get => _employees;
            set => SetField(ref _employees, value);
        }

        public EmployeeModel SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetField(ref _selectedEmployee, value);
                // Handle the selection change
                OnProductSelected();
            }
        }

        public string UpdateHint => ProgramResources.UpdateEmployee;
        public string DeleteHint => ProgramResources.DeleteEmployee;
        public string ClearHint => ProgramResources.ClearFields;

        public ICommand ToggleDeleteCommand { get; private set; }

        public ICommand ToggleUpdateToDbCommand { get; private set; }

        public ICommand ToggleClearFormCommand { get; private set; }

        public async Task InitializeDataAsync()
        {
            await ExecuteAsync(LoadData);
        }

        private void OnProductSelected()
        {
            if (SelectedEmployee == null)
            {
                return;
            }

            PersonName = SelectedEmployee.Name;
            PersonLastName = SelectedEmployee.LastName;
            PersonDepartment = SelectedEmployee.Department;
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
                var loadedData = await _employeeService.GetAllEmployeeAsync(_token);
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
                _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
                await ExecuteAsync(() => _employeeService.DeleteAsync(SelectedEmployee.Id, _token));
                await LoadData();
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
                _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

                var updatedEmployeeData = new EmployeeModel
                {
                    Id = SelectedEmployee.Id,
                    DateTime = DateTimeOffset.UtcNow,
                    Name = PersonName,
                    LastName = PersonLastName,
                    Department = PersonDepartment,
                    PhotoName = string.Format(ProgramResources.FileName, PersonName, PersonLastName, DateTime.UtcNow)
                };

                await ExecuteAsync(() => _employeeService.UpdateAsync(updatedEmployeeData, _token));

                await LoadData();
            }
            catch (Exception e)
            {
                Logger.Info("Failed to update employee data {e}", e);
                _windowManager.ShowModalWindow("Failed to update employee data");
            }
        }

        private void ToggleClearFields()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            ClearFields();
        }
    }
}