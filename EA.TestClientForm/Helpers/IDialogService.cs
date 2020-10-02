using System.Windows;

namespace EA.TestClientForm.Helpers
{
    internal interface IDialogService
    {
        /// <summary>
        /// File path prop
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Method for show modal attention window
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);

        /// <summary>
        /// Overloaded method for show modal attention window
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="msButton"></param>
        /// <param name="msImage"></param>
        void ShowMessage(string message, string title, MessageBoxButton msButton, MessageBoxImage msImage);

        /// <summary>
        /// Method for creating open file dialog
        /// </summary>
        /// <returns></returns>
        bool OpenFileDialog();

        /// <summary>
        /// Method for creating save file dialog
        /// </summary>
        /// <returns></returns>
        bool SaveFileDialog();
    }
}