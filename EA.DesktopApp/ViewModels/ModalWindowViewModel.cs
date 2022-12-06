using System.Windows;
using System.Windows.Input;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    internal class ModalWindowViewModel : BaseViewModel
    {
        private readonly ModalWindow modalWindow;
        private RegistrationForm registrationForm;

        private ICommand toggleOkButtonCommand;

        private string warningText;

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
            this.modalWindow = modalWindow;
            InitializeCommands();
        }

        /// <summary>
        ///     Property for message binding to text box
        ///     in View
        /// </summary>
        public string WarningText
        {
            get => warningText;
            set
            {
                warningText = value;
                // From INotifyPropertyChanged
                SetField(ref warningText, value);
            }
        }

        /// <summary>
        ///     Relay command for OK button execute
        /// </summary>
        public ICommand ToggleOkButtonCommand
        {
            get => toggleOkButtonCommand;
            set { }
        }

        /// <summary>
        ///     Initialize relay command
        /// </summary>
        private void InitializeCommands()
        {
            toggleOkButtonCommand = new RelayCommand(CloseWindowExecute);
        }

        /// <summary>
        ///     this.Close
        /// </summary>
        private void CloseWindowExecute()
        {
            var soundPlayerHelper = new SoundPlayerService();
            soundPlayerHelper.PlaySound("button");
            modalWindow.Close();
        }

        /// <summary>
        ///     Show modal windoww
        /// </summary>
        public void ShowWindow()
        {
            modalWindow.DataContext = this;
            modalWindow.Owner = Application.Current.MainWindow;
            modalWindow.ShowDialog();
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