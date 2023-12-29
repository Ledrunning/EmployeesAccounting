using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAdminGatewayService _adminGatewayService;
        private readonly ISoundPlayerService _soundPlayer;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;
        private string _oldPasswordValue;

        private string _userMessage;

        public AdminViewModel(ISoundPlayerService soundPlayer,
            IAdminGatewayService adminGatewayService,
            CancellationToken token, IWindowManager windowManager)
        {
            _soundPlayer = soundPlayer;
            _adminGatewayService = adminGatewayService;
            _token = token;
            _windowManager = windowManager;
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
            set => SetField(ref _oldPasswordValue, value);
        }

        public string UserMessage
        {
            get => _userMessage;
            set => SetField(ref _userMessage, value);
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
                        if (string.IsNullOrEmpty(OldPasswordField))
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
            try
            {
                _soundPlayer.PlaySound(SoundPlayerService.ButtonSound);

                var credentials = SetCredentials(OldPasswordField);
                _adminGatewayService.SetCredentials(credentials);
                var result = await _adminGatewayService.ChangeLoginAsync(new Credentials
                {
                    UserName = LoginField,
                    Password = PasswordField,
                    OldPassword = OldPasswordField
                }, _token);

                if (result)
                {
                    _windowManager.CloseWindow<AdminForm>();
                    _windowManager.ShowModalWindow("Password has been changed");
                }
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred when changing the password! {E}", e);
                UserMessage = $"{UiErrorResource.PasswordChangeError}\n {e.Message}!";
                ClearFields();
                return;
            }

            UserMessage = string.Empty;
        }

        private void ToggleClearFieldsExecute()
        {
            _soundPlayer.PlaySound(SoundPlayerService.ButtonSound);
            LoginField = string.Empty;
            PasswordField = string.Empty;
            OldPasswordField = string.Empty;
        }

        protected override void ClearFields()
        {
            LoginField = string.Empty;
            PasswordField = string.Empty;
            OldPasswordField = string.Empty;
        }
    }
}