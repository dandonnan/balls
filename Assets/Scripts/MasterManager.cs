namespace Multiball
{
    using Multiball.Input;
    using Multiball.Levels;
    using Multiball.Save;
    using UnityEngine;

    /// <summary>
    /// A master manager for handling other managers.
    /// </summary>
    internal class MasterManager : MonoBehaviour
    {
        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            // Initialise other managers
            InputManager.Initialise();
            SaveManager.Initialise();
            LevelManager.Initialise();
        }
    }
}