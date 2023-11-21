using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminViewModel : BaseViewModel
    {
        private readonly ISoundPlayerService _soundPlayer;
        private readonly IAdminGatewayService _adminGatewayService;
        private readonly CancellationToken _token;
        private string _oldPasswordValue;

        public AdminViewModel(ISoundPlayerService soundPlayer, 
            IAdminGatewayService adminGatewayService,
            CancellationToken token)
        {
            _soundPlayer = soundPlayer;
            _adminGatewayService = adminGatewayService;
            _token = token;
            InitializeCommands();
        }

        public string RegistrationHint => UiErrorResource.RegisterButtonHint;
        public string ClearFieldsHint => UiErrorResource.RegisterClearFieldsHint;

        public ICommand ClearFieldsCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }

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

        private async void ToggleRegistrationExecute()
        {
            var isLogin = await _adminGatewayService.Login(_token);

            if (isLogin)
            {
                await _adminGatewayService.CreateAsync(new AdministratorModel()
                {

                }, _token);
            }
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