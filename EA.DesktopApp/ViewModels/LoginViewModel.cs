using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IWindowFactory _windowFactory;
        private string _login;
        private Window loginWindow;

        private string _password;
        private ISoundPlayerService _soundPlayerHelper;


        public LoginViewModel(ISoundPlayerService soundPlayerHelper,
            IWindowFactory windowFactory)
        {
            _soundPlayerHelper = soundPlayerHelper;
            _windowFactory = windowFactory;
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
                        else
                        {
                            //if (!IsPasswordChecked("111")) //TODO: Hardcoded!
                            //{
                            //    error = "Password incorrect!";
                            //}
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
            loginWindow.Close();
            var registrationForm = _windowFactory.CreateRegistrationForm();
            registrationForm.Owner = Application.Current.MainWindow;
            registrationForm.Show();
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
            var adminForm = _windowFactory.CreateAdminForm();
            adminForm.Owner = Application.Current.MainWindow;
            adminForm.Show();
        }

        public void ShowLoginWindow()
        {
            loginWindow = _windowFactory.CreateLoginWindow();
            loginWindow.Owner = Application.Current.MainWindow;
            loginWindow.Show();
        }
    }
}