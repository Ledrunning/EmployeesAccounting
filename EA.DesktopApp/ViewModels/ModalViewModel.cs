using System.Windows.Input;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class ModalViewModel : BaseViewModel
    {
        private readonly IWindowManager _windowManager;
        private string _warningText;

        /// <summary>
        ///     .ctor
        /// </summary>
        public ModalViewModel()
        {
            InitializeCommands();
        }

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="windowManager"></param>
        public ModalViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            InitializeCommands();
        }

        /// <summary>
        ///     Property for message binding to text box
        ///     in View
        /// </summary>
        public string WarningText
        {
            get => _warningText;
            set
            {
                _warningText = value;
                SetField(ref _warningText, value);
            }
        }

        /// <summary>
        ///     Relay command for OK button execute
        /// </summary>
        public ICommand ToggleOkButtonCommand { get; set; }

        /// <summary>
        ///     Initialize relay command
        /// </summary>
        private void InitializeCommands()
        {
            ToggleOkButtonCommand = new RelayCommand(CloseWindowExecute);
        }

        /// <summary>
        ///     this.Close
        /// </summary>
        private void CloseWindowExecute()
        {
            var soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            _windowManager.CloseWindow<ModalWindow>();
        }

        /// <summary>
        ///     Show modal window
        /// </summary>
        public void ShowWindow()
        {
            _windowManager.ShowWindow<ModalWindow>();
        }

        /// <summary>
        ///     Set text for message
        /// </summary>
        /// <param name="message">Your text for message</param>
        public void SetMessage(string message)
        {
            WarningText = message;
        }
    }
}