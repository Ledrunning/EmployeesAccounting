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
        private bool _isReady;
        private bool _isRunning;
        private string _login;

        private string _password;
        private RegistrationForm _registrationFormPage;
        private ISoundPlayerService _soundPlayerHelper;


        public LoginViewModel(ISoundPlayerService soundPlayerHelper,
            IWindowFactory windowFactory,
            IPhotoShootService photoShootService)
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

        /// <summary>
        ///     Start webCam service button toggle
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Button state
        /// </summary>
        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;
                OnPropertyChanged();
            }
        }

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
            // Playing sound effect for button
            _soundPlayerHelper.PlaySound("button");

            if (_registrationFormPage == null)
            {
                return;
            }

            _windowFactory.CreateLoginWindow().Close();
            _registrationFormPage.ShowDialog();
        }

        private void ToggleCancelExecute()
        {
            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");

            LoginField = string.Empty;
            PasswordField = string.Empty;
        }

        private void ToggleAdminWindowShowExecute()
        {
            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");
            _windowFactory.CreateAdminWindow().Owner = Application.Current.MainWindow;
            _windowFactory.CreateAdminWindow().Show();
        }

        public void ShowLoginWindow()
        {
            _windowFactory.CreateModalWindow().Owner = Application.Current.MainWindow;
            _windowFactory.CreateLoginWindow().Show();
        }
    }
}