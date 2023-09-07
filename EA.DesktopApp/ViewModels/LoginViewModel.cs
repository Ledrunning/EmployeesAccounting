using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly ISoundPlayerService _soundPlayerHelper;
        private readonly IWindowManager _windowManager;
        private string _login;

        private string _password;


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

        private string LocalPasswordDisplayed => new string('*', _password?.Length ?? 0);

        [Required(AllowEmptyStrings = false)]
        public string LoginField
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string PasswordField
        {
            get => LocalPasswordDisplayed;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Error indexer
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;

                switch (columnName)
                {
                    case nameof(LoginField):
                        if (string.IsNullOrEmpty(LoginField))
                        {
                            error = UiErrorResource.EmptyLogin;
                        }
                        else if (LoginField.Contains(" "))
                        {
                            error = UiErrorResource.SpaceInlogin;
                        }

                        break;

                    case nameof(PasswordField):
                        if (string.IsNullOrEmpty(PasswordField))
                        {
                            error = UiErrorResource.EmptyPassword;
                        }
                        else if (PasswordField.Contains(" "))
                        {
                            error = UiErrorResource.SpaceInPassword;
                        }

                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => UiErrorResource.DataError;

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
            _windowManager.CloseLoginWindow();
            _windowManager.ShowRegistrationWindow();
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
            _windowManager.ShowAdminWindow();
        }

        public void ShowLoginWindow()
        {
            _windowManager.ShowLoginWindow();
        }
    }
}