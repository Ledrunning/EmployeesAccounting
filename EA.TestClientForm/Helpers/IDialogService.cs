namespace EA.TestClientForm.Helpers
{
    internal interface IDialogService
    {
        /// <summary>
        ///     File path prop
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        ///     Method for creating open file dialog
        /// </summary>
        /// <returns></returns>
        bool IsDialogWindowOpened();

        /// <summary>
        ///     Method for creating save file dialog
        /// </summary>
        /// <returns></returns>
        bool SaveFileDialog();
    }
}