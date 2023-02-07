namespace Multiball.Audio
{
    using Multiball.Save;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// An audio manager.
    /// </summary>
    internal class AudioManager
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static AudioManager audioManager;

        /// <summary>
        /// The list of sound effects.
        /// </summary>
        private SoundEffects soundEffects;

        /// <summary>
        /// The background music controller.
        /// </summary>
        private BackgroundMusic backgroundMusic;

        /// <summary>
        /// The name of the current music track.
        /// </summary>
        private string currentMusicTrackName;

        /// <summary>
        /// A private constructor.
        /// </summary>
        private AudioManager()
        {
            audioManager = this;
        }

        /// <summary>
        /// Initialise the audio manager instance.
        /// </summary>
        public static void Initialise()
        {
            if (audioManager == null)
            {
                new AudioManager();
            }
        }

        /// <summary>
        /// Register the background music controller.
        /// </summary>
        /// <param name="music">The background music controller.</param>
        public static void RegisterBackgroundMusic(BackgroundMusic music)
        {
            audioManager.backgroundMusic = music;
        }

        /// <summary>
        /// Register the sound effects.
        /// </summary>
        /// <param name="effects">The sound effects.</param>
        public static void RegisterSoundEffects(SoundEffects effects)
        {
            audioManager.soundEffects = effects;
        }

        /// <summary>
        /// Play a music track.
        /// </summary>
        /// <param name="trackName">The track to play.</param>
        public static void PlayMusic(string trackName)
        {
            // If the requested track is not current playing
            if (trackName != audioManager.currentMusicTrackName
                || audioManager.backgroundMusic.AudioSource.isPlaying == false)
            {
                // Find the track in the list
                AudioClip track = audioManager.backgroundMusic.Tracks.FirstOrDefault(t => t.name == trackName);

                // If the track was found then play it
                if (track != null)
                {
                    audioManager.currentMusicTrackName = trackName;
                    audioManager.backgroundMusic.Play(track);
                }
            }
        }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        /// <param name="soundName">The name of the sound.</param>
        public static void PlaySound(string soundName)
        {
            // Find the sound in the list
            AudioClip clip = audioManager.soundEffects.Sounds.FirstOrDefault(s => s.name == soundName);

            // If the sound was found then play it
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Vector3.zero, SaveManager.Data.SoundVolume / (float)SaveData.MaxVolume);
            }
        }

        /// <summary>
        /// Set the volume of the music.
        /// </summary>
        /// <param name="volume">The volume.</param>
        public static void SetMusicVolume(int volume)
        {
            audioManager.backgroundMusic.AudioSource.volume = volume / (float)SaveData.MaxVolume;
        }
    }
}