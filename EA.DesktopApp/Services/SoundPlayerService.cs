using System.Collections.Generic;
using System.IO;
using System.Media;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Properties;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class for sound effects playing extends ISoundPlayer
    /// </summary>
    public class SoundPlayerService : ISoundPlayer
    {
        private readonly Dictionary<string, UnmanagedMemoryStream> sounds =
            new Dictionary<string, UnmanagedMemoryStream>();

        /// <summary>
        ///     .ctor
        /// </summary>
        public SoundPlayerService()
        {
            sounds.Add("button", Resources.button);
            sounds.Add("camera", Resources.camera);
        }

        /// <summary>
        ///     PlaySound Method
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            using (var player = new SoundPlayer())
            {
                player.Stream = sounds[sound];
                player.Play();
            }
        }
    }
}