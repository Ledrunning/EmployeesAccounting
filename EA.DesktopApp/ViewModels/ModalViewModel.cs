using System.Windows;
using System.Windows.Input;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class ModalViewModel : BaseViewModel
    {
        private readonly IWindowFactory _windowFactory;
        private string _warningText;

        //private ModalWindow _modelWindow;

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
        /// <param name="windowFactory"></param>
        public ModalViewModel(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
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
            soundPlayerHelper.PlaySound("button");
            _windowFactory.CreateModalWindow().Close();
        }

        /// <summary>
        ///     Show modal window
        /// </summary>
        public void ShowWindow()
        {
            _windowFactory.CreateModalWindow().DataContext = this;
            _windowFactory.CreateModalWindow().Owner = Application.Current.MainWindow;
            _windowFactory.CreateModalWindow().ShowDialog();
        }

        /// <summary>
        ///     Messge set method
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage(string message)
        {
            WarningText = message;
        }
    }
}