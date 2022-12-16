using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class LoginFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly LoginWindow _loginWindow;
        private bool _isReady;
        private bool _isRunning;
        private RegistrationForm _registrationFormPage;
        private SoundPlayerService _soundPlayerHelper = new SoundPlayerService();

        private string _loginValue;

        private string _passwordValue;


        public LoginFormViewModel()
        {
            InitializeCommands();
        }

        public LoginFormViewModel(LoginWindow loginWindow)
        {
            this._loginWindow = loginWindow;
            InitializeCommands();
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AdminModeCommand { get; private set; }

        public string Login { get; } = "Enter the password";
        public string Cancel { get; } = "Press for clear";

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

        [Required(AllowEmptyStrings = false)]
        public string LoginField
        {
            get => _loginValue;
            set
            {
                _loginValue = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string PasswordField
        {
            get => _passwordValue;
            set
            {
                _passwordValue = value;
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
                    case "LoginField":
                        if (string.IsNullOrEmpty(LoginField))
                        {
                            error = "Введите логин!";
                        }
                        else if (LoginField.Contains(" "))
                        {
                            error = "Пароль не может содержать пробел";
                        }

                        break;

                    case "PasswordField":
                        if (string.IsNullOrEmpty(PasswordField))
                        {
                            error = "Введите пароль!";
                        }
                        else if (PasswordField.Contains(" "))
                        {
                            error = "Пароль не может содержать пробел";
                        }

                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => "Введите данные!";

        private void InitializeCommands()
        {
            LoginCommand = new RelayCommand(ToggleLoginExecute);
            CancelCommand = new RelayCommand(ToggleCancelExecute);
            AdminModeCommand = new RelayCommand(ToggleAdminWindowShowExecute);
        }

        private void ToggleLoginExecute()
        {
            // Playing sound effect for button
            _soundPlayerHelper = new SoundPlayerService();
            _soundPlayerHelper.PlaySound("button");

            // True - button is pushed - Working!
            IsRunning = false;
            if (_registrationFormPage != null && !_registrationFormPage.IsClosed)
            {
                return;
            }

            var registrationFormViewModel = new RegistrationFormViewModel();

            _registrationFormPage = new RegistrationForm(IsRunning)
            {
                DataContext = registrationFormViewModel,
                Owner = Application.Current.MainWindow
            };

            //_registrationFormPage.Show();
            //IsStreaming = false;
            //_faceDetectionService.CancelServiceAsync();
            _registrationFormPage.ShowDialog();

            //if (!_faceDetectionService.IsRunning)
            //{
            //    IsStreaming = true;
            //    _faceDetectionService.RunServiceAsync();
            //}
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
            var adminForm = new AdminForm
            {
                Owner = Application.Current.MainWindow
            };
            adminForm.ShowDialog();
        }

        public void ShowWindow()
        {
            _loginWindow.ShowDialog();
        }
    }
}