using System;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Enum;
using EA.DesktopApp.Models;
using EA.DesktopApp.Resources.Messages;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;
using NLog;

namespace EA.DesktopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAdminGatewayService _adminGatewayService;
        private readonly ISoundPlayerService _soundPlayerHelper;
        private readonly CancellationToken _token;
        private readonly IWindowManager _windowManager;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LoginViewModel(ISoundPlayerService soundPlayerHelper,
            IWindowManager windowManager,
            IAdminGatewayService adminGatewayService,
            CancellationToken token)
        {
            _soundPlayerHelper = soundPlayerHelper;
            _windowManager = windowManager;
            _adminGatewayService = adminGatewayService;
            _token = token;
            InitializeCommands();
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AdminModeCommand { get; private set; }

        public string LoginHint => ProgramResources.LoginTooltipMessage;
        public string PasswordHint => ProgramResources.PasswordTooltipMessage;
        public string CancelHint => ProgramResources.CancelTooltipMessage;

        /// <summary>
        ///     Error exception throwing
        /// </summary>
        public string Error => UiErrorResource.DataError;

        protected override string ValidateProperty(string columnName)
        {
            var error = string.Empty;
            switch (columnName)
            {
                case nameof(LoginField):
                    if (string.IsNullOrWhiteSpace(LoginField))
                    {
                        error = UiErrorResource.EmptyLogin;
                    }

                    break;
                case nameof(PasswordField):
                    if (string.IsNullOrWhiteSpace(PasswordField))
                    {
                        error = UiErrorResource.EmptyPassword;
                    }

                    break;
            }

            CheckFieldErrors(columnName, error);
            return error;
        }
        private void InitializeCommands()
        {
            LoginCommand = new RelayCommand(ToggleLoginExecute);
            CancelCommand = new RelayCommand(ToggleCancelExecute);
            AdminModeCommand = new RelayCommand(ToggleAdminWindowShowExecute);
        }

        private async void ToggleLoginExecute()
        {
            try
            {
                _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);

                //Set credentials
                var reversedPass = new string(PasswordField.Reverse().ToArray());
                _adminGatewayService.SetCredentials(new Credentials
                {
                    UserName = LoginField,
                    Password = reversedPass
                });

                var isLogin = await _adminGatewayService.Login(_token);

                if (!isLogin)
                {
                    return;
                }

                _windowManager.CloseWindow<LoginWindow>();

                switch (MainViewModel.WindowType)
                {
                    case WindowType.RegistrationForm:
                        _windowManager.ShowWindow<RegistrationForm>();
                        break;
                    case WindowType.EditForm:
                        _windowManager.ShowWindow<RedactorForm>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Login to app failed! {E}", e);
            }
        }

        private void ToggleCancelExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            LoginField = string.Empty;
            PasswordField = string.Empty;
        }

        private void ToggleAdminWindowShowExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            _windowManager.ShowWindow<AdminForm>();
        }
    }
}