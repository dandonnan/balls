namespace Multiball.Audio
{
    using Multiball.Save;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A script that is assigned background music tracks.
    /// </summary>
    internal class BackgroundMusic : MonoBehaviour
    {
        /// <summary>
        /// The audio source.
        /// </summary>
        public AudioSource AudioSource;

        /// <summary>
        /// The list of tracks.
        /// </summary>
        public List<AudioClip> Tracks;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            // Register with the audio manager
            AudioManager.RegisterBackgroundMusic(this);

            // Change the volume on the audio source
            AudioSource.volume = SaveManager.Data.MusicVolume / (float)SaveData.MaxVolume;
        }

        /// <summary>
        /// Play the new track.
        /// </summary>
        /// <param name="track">The track to play.</param>
        public void Play(AudioClip track)
        {
            AudioSource.Stop();
            AudioSource.clip = track;
            AudioSource.Play();
        }
    }
}