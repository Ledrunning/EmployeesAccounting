using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminViewModel : BaseViewModel
    {
        private readonly ISoundPlayerService _soundPlayer;
        private bool _isReady;
        private bool _isRunning;
        private string _oldPasswordValue;
        private string _userMessage;

        public AdminViewModel(ISoundPlayerService soundPlayer)
        {
            _soundPlayer = soundPlayer;
            InitializeCommands();
        }

        public string RegistrationHint => UiErrorResource.RegisterButtonHint;
        public string ClearFieldsHint => UiErrorResource.RegisterClearFieldsHint;

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
        protected override string ValidateProperty(string columnName)
        {
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
                    case nameof(OldPasswordField):
                        if (string.IsNullOrEmpty(PasswordField))
                        {
                            error = UiErrorResource.OldPassword;
                        }
                        else if (OldPasswordField.Contains(" "))
                        {
                            error = UiErrorResource.EmptyPassword;
                        }

                        break;
                }

                return error;
            }
        }

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
            _soundPlayer.PlaySound(SoundPlayerService.ButtonSound);
            LoginField = string.Empty;
            PasswordField = string.Empty;
            OldPasswordField = string.Empty;
        }
    }
}