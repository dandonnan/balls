namespace Multiball.Audio
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A script that is assigned all sound effects.
    /// </summary>
    internal class SoundEffects : MonoBehaviour
    {
        /// <summary>
        /// The list of sound effects.
        /// </summary>
        public List<AudioClip> Sounds;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            // Register with the audio manager
            AudioManager.RegisterSoundEffects(this);
        }
    }
}