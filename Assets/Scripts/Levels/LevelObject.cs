namespace Multiball.Levels
{
    using Multiball.Audio;
    using UnityEngine;

    /// <summary>
    /// A script for a level's prefab object.
    /// </summary>
    internal class LevelObject : MonoBehaviour
    {
        /// <summary>
        /// The name of the background music track;
        /// </summary>
        public string MusicTrack;

        /// <summary>
        /// Called when the level spawns.
        /// </summary>
        private void Start()
        {
            // Play the track
            AudioManager.PlayMusic(MusicTrack);
        }
    }
}