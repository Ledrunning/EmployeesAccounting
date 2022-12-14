using EA.DesktopApp.Contracts;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels;
using Microsoft.Win32;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class implements IDialogService
    /// </summary>
    public class DialogService : IDialogService
    {
        private const string MessageBoxTitle = "Оповещение";
        private readonly ModalWindowViewModel _modalWindow;
        private const string PhotoNonSave = "Фото не сохранено!";
        private const string PhotoSaveDone = "Фото сделано и сохранено!";

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

            if (openFileDialog.ShowDialog() != true)
            {
                return false;
            }

            FilePath = openFileDialog.FileName;
            return true;
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
                ShowMessage(PhotoNonSave);
                return false;
            }

            FilePath = saveFileDialog.FileName;
            ShowMessage(PhotoSaveDone);
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