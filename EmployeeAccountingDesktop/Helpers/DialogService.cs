using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels;
using Microsoft.Win32;

namespace EA.DesktopApp.Helpers
{
    /// <summary>
    ///     Class implements IDialogService
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly string messageBoxTitle = "Оповещение";
        private readonly string photoNonSave = "Фото не сохранено!";
        private readonly string photoSaveDone = "Фото сделано и сохранено!";
        private readonly ModalWindowViewModel _modalWindow;

        public DialogService()
        {
            _modalWindow = new ModalWindowViewModel(new ModalWindow());
        }

        /// <summary>
        ///     Get file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Bolean method which returned is openfile dialog window
        /// </summary>
        /// <returns>ShowDialog true or false</returns>
        public bool OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     ///
        ///     <summary>
        ///         Bolean method which returned is savefile dialog window
        ///     </summary>
        ///     <returns></returns>
        /// </summary>
        /// <returns></returns>
        public bool SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
            {
                ShowMessage(photoNonSave);
                return false;
            }

            FilePath = saveFileDialog.FileName;
            ShowMessage(photoSaveDone);
            return true;
        }

        /// <summary>
        ///     Method for show modal attention window
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            _modalWindow.SetMessage(message);
            _modalWindow.ShowWindow();
        }
    }
}