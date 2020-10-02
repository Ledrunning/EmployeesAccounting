using System.Windows;
using System.Windows.Input;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.View;

namespace EA.DesktopApp.ViewModels
{
    internal class ModalWindowViewModel : BaseViewModel
    {
        private readonly ModalWindow _modalWindow;
        private RegistrationForm regForm;

        private ICommand _toogleOkButtonCommand;

        private string _warningText;

        //private ModalWindow _modelWindow;

        /// <summary>
        ///     .ctor
        /// </summary>
        public ModalWindowViewModel()
        {
            InitializeCommands();
        }

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="modalWindow"></param>
        public ModalWindowViewModel(ModalWindow modalWindow)
        {
            _modalWindow = modalWindow;
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
                // From INotifyPropertyChanged
                SetField(ref _warningText, value);
            }
        }

        /// <summary>
        ///     Relay command for OK button execute
        /// </summary>
        public ICommand ToggleOkButtonCommand
        {
            get => _toogleOkButtonCommand;
            set { }
        }

        /// <summary>
        ///     Initialize relay command
        /// </summary>
        private void InitializeCommands()
        {
            _toogleOkButtonCommand = new RelayCommand(CloseWindowExecute);
        }

        /// <summary>
        ///     this.Close
        /// </summary>
        private void CloseWindowExecute()
        {
            var _soundPlayerHelper = new SoundPlayerHelper();
            _soundPlayerHelper.PlaySound("button");
            _modalWindow.Close();
        }

        /// <summary>
        ///     Show modal windoww
        /// </summary>
        public void ShowWindow()
        {
            _modalWindow.DataContext = this;
            _modalWindow.Owner = Application.Current.MainWindow;
            _modalWindow.ShowDialog();
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