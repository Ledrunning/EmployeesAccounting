using System.Windows;
using EA.TestClientForm.Contracts;
using Microsoft.Win32;

namespace EA.TestClientForm.Services
{
    internal class DialogService : IDialogService
    {
        private const string MessageBoxTitle = "Alert!";
        private const string PhotoSendDone = "Photo has been sent!";
        private const string PhotoSendFailure = "Photo did not send!";

        /// <summary>
        ///     Get file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Bolean method which returned is openfile dialog window
        /// </summary>
        /// <returns>ShowDialog true or false</returns>
        public bool IsDialogWindowOpened()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*"
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return false;
            }

            FilePath = openFileDialog.FileName;
            return true;
        }

        /// <summary>
        ///     Boolean method which returned is savefile dialog window
        /// </summary>
        /// <returns></returns>
        public bool SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
            {
                MessageBox.Show(PhotoSendFailure, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            FilePath = saveFileDialog.FileName;
            MessageBox.Show(PhotoSendDone, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}