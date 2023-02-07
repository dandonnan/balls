namespace Multiball.Save
{
    /// <summary>
    /// The game's save data.
    /// </summary>
    internal class SaveData
    {
        /// <summary>
        /// The minimum volume.
        /// </summary>
        public const int MinVolume = 0;

        /// <summary>
        /// The maximum volume.
        /// </summary>
        public const int MaxVolume = 10;

        /// <summary>
        /// The default volume.
        /// </summary>
        public const int DefaultVolume = 7;

        /// <summary>
        /// The latest level that was played.
        /// </summary>
        public int LatestLevel { get; set; }

        /// <summary>
        /// The furthest level the player has reached.
        /// </summary>
        public int FurthestLevel { get; set; }

        /// <summary>
        /// The volume for sound effects.
        /// </summary>
        public int SoundVolume { get; set; }

        /// <summary>
        /// The volume for music.
        /// </summary>
        public int MusicVolume { get; set; }

        /// <summary>
        /// The window resolution.
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Whether the window is fullscreen.
        /// </summary>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// The language code.
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Set the data to the defaults.
        /// </summary>
        public SaveData()
        {
            LatestLevel = -1;
            FurthestLevel = 0;
            SoundVolume = DefaultVolume;
            MusicVolume = DefaultVolume;
            Resolution = "1280x720";
            Fullscreen = false;
            LanguageCode = "en";
        }
    }
}