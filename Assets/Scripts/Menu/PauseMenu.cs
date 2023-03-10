namespace Multiball.Menu
{
    using Multiball.Audio;
    using Multiball.Input;
    using Multiball.Levels;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// The pause menu.
    /// </summary>
    internal class PauseMenu : MonoBehaviour
    {
        [Header("UI")]
        /// <summary>
        /// The background image for the resume option.
        /// </summary>
        public Image ResumeBackground;

        /// <summary>
        /// The background image for the options option.
        /// </summary>
        public Image OptionsBackground;

        /// <summary>
        /// The background image for the quit option.
        /// </summary>
        public Image QuitBackground;

        [Header("Menu")]
        /// <summary>
        /// The options menu.
        /// </summary>
        public OptionsMenu OptionsMenu;

        [Header("Audio")]
        /// <summary>
        /// The name of the sound effect when moving up.
        /// </summary>
        public string SoundMoveUp;

        /// <summary>
        /// The name of the sound effect when moving down.
        /// </summary>
        public string SoundMoveDown;

        /// <summary>
        /// The name of the sound effect when selecting an option.
        /// </summary>
        public string SoundConfirm;

        /// <summary>
        /// The name of the sound effect when returning to the previous menu.
        /// </summary>
        public string SoundBack;

        /// <summary>
        /// The options to display in the pause menu.
        /// </summary>
        private MenuOptionCollection menuOptions;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            SetupOptions();
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        private void LateUpdate()
        {
            menuOptions.HandleInput();

            // If pause is pressed, then resume the game
            if (InputManager.Game.Pause.WasPressedThisFrame())
            {
                AudioManager.PlaySound(SoundBack);
                Resume();
            }
        }

        /// <summary>
        /// Setup the options.
        /// </summary>
        private void SetupOptions()
        {
            menuOptions = new MenuOptionCollection();
            menuOptions.SetSounds(SoundMoveUp, SoundMoveDown, SoundConfirm);

            // Add each option, with the UI image and the method to call when selected
            menuOptions.Add("Resume", Resume, ResumeBackground);
            menuOptions.Add("Options", DisplayOptionsMenu, OptionsBackground);
            menuOptions.Add("Quit", QuitToMenu, QuitBackground);
        }

        /// <summary>
        /// Resume the game.
        /// </summary>
        private void Resume()
        {
            LevelManager.Pause(false);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Display the options menu.
        /// </summary>
        private void DisplayOptionsMenu()
        {
            OptionsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit to the main menu.
        /// </summary>
        private void QuitToMenu()
        {
            LevelManager.Pause(false);
            SceneManager.LoadScene("MenuScene");
        }
    }
}