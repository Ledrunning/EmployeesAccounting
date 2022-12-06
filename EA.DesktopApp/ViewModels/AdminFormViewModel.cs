using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    class AdminFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool isReady;
        private bool isRunning;
        private string userMessage;

        private string loginValue;

        private string passwordValue;
        private string oldPasswordValue;

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
                        if (string.IsNullOrEmpty(LoginField)) error = "Введите логин!";
                        if (LoginField.Contains(" ")) error = "Логин не может содержать пробел";
                        break;

                    case "PasswordField":
                        if (string.IsNullOrEmpty(PasswordField)) error = "Введите пароль!";
                        if(PasswordField.Contains(" ")) error = "Пароль не может содержать пробел";
                        break;
                    case "OldPasswordField":
                        if (string.IsNullOrEmpty(PasswordField)) error = "Введите старый пароль!";
                        if(OldPasswordField.Contains(" ")) error = "Пароль не может содержать пробел";
                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => "Введите данные!";

        public AdminFormViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ClearFieldsCommand = new RelayCommand(ToogleClearFieldsExecute);
            RegistrationCommand = new RelayCommand(ToogleRegistrationExecute);
        }

        private void ToogleRegistrationExecute()
        {

        }

        private void ToogleClearFieldsExecute()
        {
            var soundHelper = new SoundPlayerService();
            soundHelper.PlaySound("button");

            LoginField = string.Empty;
            PasswordField = string.Empty;
            OldPasswordField = string.Empty;
        }
    }
}
