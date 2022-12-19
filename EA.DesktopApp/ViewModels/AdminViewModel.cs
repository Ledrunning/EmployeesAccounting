using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool _isReady;
        private bool _isRunning;

        private string _loginValue;
        private string _oldPasswordValue;

        private string _passwordValue;
        private string _userMessage;

        public AdminViewModel()
        {
            InitializeCommands();
        }

        public string Registration => "Нажмите для регистрации";
        public string ClearFields => "Нажмите для очистки полей";

        public ICommand ClearFieldsCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }

        /// <summary>
        ///     Start webCam service button toogle
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

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
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

        [Required(AllowEmptyStrings = false)]
        public string OldPasswordField
        {
            get => _oldPasswordValue;
            set
            {
                _oldPasswordValue = value;
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
                            error = "Логин не может содержать пробел";
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
                    case "OldPasswordField":
                        if (string.IsNullOrEmpty(PasswordField))
                        {
                            error = "Введите старый пароль!";
                        }
                        else if (OldPasswordField.Contains(" "))
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
            ClearFieldsCommand = new RelayCommand(ToggleClearFieldsExecute);
            RegistrationCommand = new RelayCommand(ToggleRegistrationExecute);
        }

        private void ToggleRegistrationExecute()
        {
        }

        private void ToggleClearFieldsExecute()
        {
            var soundHelper = new SoundPlayerService();
            soundHelper.PlaySound("button");

            LoginField = string.Empty;
            PasswordField = string.Empty;
            OldPasswordField = string.Empty;
        }
    }
}