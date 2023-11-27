using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    internal class AdminViewModel : BaseViewModel
    {
        private readonly IAdminGatewayService _adminGatewayService;
        private readonly ISoundPlayerService _soundPlayer;
        private readonly CancellationToken _token;
        private string _oldPasswordValue;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private string _userMessage;

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
            AdministratorModel administratorModel = null;
            try
            {
                administratorModel = await _adminGatewayService.GetByLoginAsync(new Credentials
                {
                    UserName = LoginField,
                    Password = OldPasswordField
                }, _token);

                if (_adminGatewayService.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UserMessage = UiErrorResource.IncorrectPassword;
                    ClearFields();
                    return;
                }
            }
            catch(Exception e)
            {
                Logger.Error("An error occurred when changing the password! {E}", e);

                if (_adminGatewayService.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UserMessage = UiErrorResource.IncorrectPassword;
                    ClearFields();
                    return;
                }

                UserMessage = UiErrorResource.PasswordChangeError;
            }

            UserMessage = string.Empty;
            if (administratorModel == null)
            {
                return;
            }

            administratorModel.OldPassword = OldPasswordField;
            administratorModel.Password = PasswordField;
            await _adminGatewayService.UpdateAsync(administratorModel, _token);
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