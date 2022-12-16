namespace EA.DesktopApp.Contracts
{
    /// <summary>
    ///     Interface for sound effects playing
    /// </summary>
    public interface ISoundPlayer
    {
        /// <summary>
        ///     Play sound method.
        ///     parametr is a file name witout .wav
        /// </summary>
        /// <param name="sound"></param>
        void PlaySound(string sound);
    }
}