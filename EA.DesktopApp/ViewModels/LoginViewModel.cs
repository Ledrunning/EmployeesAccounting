using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ISoundPlayerService _soundPlayerHelper;
        private readonly IWindowManager _windowManager;

        public LoginViewModel(ISoundPlayerService soundPlayerHelper, IWindowManager windowManager)
        {
            _soundPlayerHelper = soundPlayerHelper;
            _windowManager = windowManager;
            InitializeCommands();
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AdminModeCommand { get; private set; }

        public string LoginHint => ProgramResources.LoginTooltipMessage;
        public string PasswordHint => ProgramResources.PasswordTooltipMessage;
        public string CancelHint => ProgramResources.CancelTooltipMessage;

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => UiErrorResource.DataError;

        protected override string ValidateProperty(string columnName)
        {
            var error = string.Empty;
            var validationErrors = new List<ValidationResult>();
            var validationContext = new ValidationContext(this) { MemberName = columnName };

            if (!Validator.TryValidateProperty(GetType().GetProperty(columnName)?.GetValue(this, null), validationContext, validationErrors) && validationErrors.Any())
            {
                return validationErrors.First().ErrorMessage;
            }

            switch (columnName)
            {
                case nameof(LoginField):
                    error = UiErrorResource.SpaceInlogin;
                    break;

                case nameof(PasswordField):
                    error = UiErrorResource.SpaceInPassword;
                    break;
            }

            return error;
        }
        
        private bool IsPasswordChecked(string password)
        {
            return string.Equals(PasswordField, password, StringComparison.CurrentCulture);
        }

        private void InitializeCommands()
        {
            LoginCommand = new RelayCommand(ToggleLoginExecute);
            CancelCommand = new RelayCommand(ToggleCancelExecute);
            AdminModeCommand = new RelayCommand(ToggleAdminWindowShowExecute);
        }

        //todo login check // And add registration form bitte!
        private void ToggleLoginExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            _windowManager.CloseWindow<LoginWindow>();
            _windowManager.ShowWindow<RegistrationForm>();
        }

        private void ToggleCancelExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            LoginField = string.Empty;
            PasswordField = string.Empty;
        }

        private void ToggleAdminWindowShowExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            _windowManager.ShowWindow<AdminForm>();
        }

        public void ShowLoginWindow()
        {
            _windowManager.ShowWindow<LoginWindow>();
        }
    }
}