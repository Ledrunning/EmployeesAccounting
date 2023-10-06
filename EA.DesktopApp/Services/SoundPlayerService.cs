using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using EA.DesktopApp.Contracts;
using NLog;

namespace EA.DesktopApp.Services
{
    /// <summary>
    ///     Class for sound effects playing extends ISoundPlayerService
    /// </summary>
    public class SoundPlayerService : ISoundPlayerService, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const string ButtonSound = "button";
        public const string CameraSound = "camera";

        private readonly Dictionary<string, UnmanagedMemoryStream> _sounds =
            new Dictionary<string, UnmanagedMemoryStream>();

        private readonly SoundPlayer player;

        /// <summary>
        ///     .ctor
        /// </summary>
        public SoundPlayerService()
        {
            _sounds.Add(ButtonSound, Properties.Resources.button);
            _sounds.Add(CameraSound, Properties.Resources.camera);
            player = new SoundPlayer();
        }

        /// <summary>
        ///     PlaySound Method
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            try
            {
                player.Stream = _sounds[sound];
                player.Play();
            }
            catch (Exception e)
            {
                Logger.Error("Sound player error {e}", e);
            }
        }

        public void Dispose()
        {
            player?.Dispose();
        }
    }
}