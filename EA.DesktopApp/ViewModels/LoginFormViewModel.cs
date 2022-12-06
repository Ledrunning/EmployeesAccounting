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
        private readonly PasswordWindow passwordWindow;
        private bool isReady;
        private bool isRunning;
        private RegistrationForm registrationFormPage;
        private SoundPlayerService soundPlayerHelper = new SoundPlayerService();

        private string loginValue;

        private string passwordValue;


        public LoginFormViewModel()
        {
            InitializeCommands();
        }

        public LoginFormViewModel(PasswordWindow passwordWindow)
        {
            this.passwordWindow = passwordWindow;
            InitializeCommands();
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AdminModeCommand { get; private set; }

        public string Login { get; } = "Введите логин";
        public string Cancel { get; } = "Нажмите для очистки полей";

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

                        if (LoginField.Contains(" "))
                        {
                            error = "Пароль не может содержать пробел";
                        }

                        break;

                    case "PasswordField":
                        if (string.IsNullOrEmpty(PasswordField))
                        {
                            error = "Введите пароль!";
                        }

                        if (PasswordField.Contains(" "))
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
            LoginCommand = new RelayCommand(ToogleLoginExecute);
            CancelCommand = new RelayCommand(ToogleCancelExecute);
            AdminModeCommand = new RelayCommand(ToogleAdminWindowShowExecute);
        }

        private void ToogleLoginExecute()
        {
            // Playing sound effect for button
            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");

            // True - button is pushed - Working!
            IsRunning = false;
            if (registrationFormPage == null || registrationFormPage.IsClosed)
            {
                var registrationFormViewModel = new RegistrationFormViewModel();

                registrationFormPage = new RegistrationForm(IsRunning);
                registrationFormPage.DataContext = registrationFormViewModel;
                registrationFormPage.Owner = Application.Current.MainWindow;

                //_registrationFormPage.Show();
                //IsStreaming = false;
                //_faceDetectionService.CancelServiceAsync();
                registrationFormPage.ShowDialog();
            }

            //if (!_faceDetectionService.IsRunning)
            //{
            //    IsStreaming = true;
            //    _faceDetectionService.RunServiceAsync();
            //}
        }

        private void ToogleCancelExecute()
        {
            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");

            LoginField = string.Empty;
            PasswordField = string.Empty;
        }

        private void ToogleAdminWindowShowExecute()
        {
            soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");
            var adminForm = new AdminForm();
            adminForm.Owner = Application.Current.MainWindow;
            adminForm.ShowDialog();
        }

        public void ShowWindow()
        {
            passwordWindow.ShowDialog();
        }
    }
}