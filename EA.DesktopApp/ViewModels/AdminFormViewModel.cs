using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool isReady;
        private bool isRunning;

        private string loginValue;
        private string oldPasswordValue;

        private string passwordValue;
        private string userMessage;

        public AdminFormViewModel()
        {
            InitializeCommands();
        }

        public string Registration { get; } = "Нажмите для регистрации";
        public string ClearFields { get; } = "Нажмите для очистки полей";

        public ICommand ClearFieldsCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }

        /// <summary>
        ///     Start webCam service button toogle
        /// </summary>
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Button state
        /// </summary>
        public bool IsReady
        {
            get => isReady;
            set
            {
                isReady = value;
                OnPropertyChanged();
            }
        }

        public string UserMessage
        {
            get => userMessage;
            set
            {
                userMessage = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string LoginField
        {
            get => loginValue;
            set
            {
                loginValue = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string PasswordField
        {
            get => passwordValue;
            set
            {
                passwordValue = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string OldPasswordField
        {
            get => oldPasswordValue;
            set
            {
                oldPasswordValue = value;
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