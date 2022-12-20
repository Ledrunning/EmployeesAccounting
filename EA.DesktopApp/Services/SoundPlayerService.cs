using System.Collections.Generic;
using System.IO;
using System.Media;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Properties;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class for sound effects playing extends ISoundPlayerService
    /// </summary>
    public class SoundPlayerService : ISoundPlayerService
    {
        private readonly Dictionary<string, UnmanagedMemoryStream> _sounds =
            new Dictionary<string, UnmanagedMemoryStream>();

        public const string ButtonSound = "button";
        public const string CameraSound = "camera";

        /// <summary>
        ///     .ctor
        /// </summary>
        public SoundPlayerService()
        {
            _sounds.Add(ButtonSound, Resources.button);
            _sounds.Add(CameraSound, Resources.camera);
        }

        /// <summary>
        ///     PlaySound Method
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            using (var player = new SoundPlayer())
            {
                player.Stream = _sounds[sound];
                player.Play();
            }
        }
    }
}