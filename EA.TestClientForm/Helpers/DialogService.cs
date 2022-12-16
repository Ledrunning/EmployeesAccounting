using Microsoft.Win32;
using System.Windows;

namespace EA.TestClientForm.Helpers
{
    internal class DialogService : IDialogService
    {
        private readonly string _photoSendDone = "Фото отправлено!";
        private readonly string _photoSendFailure = "Фото не отправлено!";
        private readonly string _messageBoxTitle = "Оповещение";

        /// <summary>
        /// Get file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Getting file name / Not used
        /// </summary>
        //public static string FilePath { get; set; }

        /// <summary>
        /// Bolean method which returned is openfile dialog window
        /// </summary>
        /// <returns>ShowDialog true or false</returns>
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// /// <summary>
        /// Bolean method which returned is savefile dialog window
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        public bool SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
            {
                ShowMessage(_photoSendFailure, _messageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            FilePath = saveFileDialog.FileName;
            ShowMessage(_photoSendDone, _messageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        /// <summary>
        /// Method for show modal attention window
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Overloaded method for show modal attention window
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="msButton"></param>
        /// <param name="msImage"></param>
        public void ShowMessage(string message, string title, MessageBoxButton msButton, MessageBoxImage msImage)
        {
            MessageBox.Show(message, title, msButton, msImage);
        }
    }
}