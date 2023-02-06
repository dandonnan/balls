namespace Multiball.Menu
{
    using Multiball.Input;
    using Multiball.Resources;
    using Multiball.Save;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    /// <summary>
    /// The options menu.
    /// </summary>
    internal class OptionsMenu : MonoBehaviour
    {
        [Header("UI")]
        /// <summary>
        /// The background image for the sound option.
        /// </summary>
        public Image SoundBackground;

        /// <summary>
        /// The background image for the music option.
        /// </summary>
        public Image MusicBackground;

        /// <summary>
        /// The background image for the fullscreen option.
        /// </summary>
        public Image FullscreenBackground;

        /// <summary>
        /// The background image for the resolution option.
        /// </summary>
        public Image ResolutionBackground;

        [Header("Values")]
        /// <summary>
        /// The value for the sound option.
        /// </summary>
        public TMP_Text SoundValue;

        /// <summary>
        /// The value for the music option.
        /// </summary>
        public TMP_Text MusicValue;

        /// <summary>
        /// The value for the fullscreen option.
        /// </summary>
        public TMP_Text FullscreenValue;

        /// <summary>
        /// The value for the resolution option.
        /// </summary>
        public TMP_Text ResolutionValue;

        [Header("Text")]
        /// <summary>
        /// The string id for the Yes text.
        /// </summary>
        public string YesStringId;

        /// <summary>
        /// The string id for the No text.
        /// </summary>
        public string NoStringId;

        /// <summary>
        /// The string id for the Back text.
        /// </summary>
        public string BackStringId;

        /// <summary>
        /// The text for the back prompt.
        /// </summary>
        public TMP_Text BackText;

        [Header("Menu")]
        /// <summary>
        /// The menu that opened the pause menu.
        /// </summary>
        public GameObject PreviousMenu;

        /// <summary>
        /// The options to display.
        /// </summary>
        private MenuOptionCollection menuOptions;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            SetupOptions();
            SetValues();

            // Add an event to be called when input occurs
            InputManager.Menu.Get().actionTriggered += OnInputEvent;
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            InputManager.Menu.Get().actionTriggered -= OnInputEvent;
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        private void LateUpdate()
        {
            menuOptions.HandleInput();

            // If the back button was pressed, return to the previous menu
            if (InputManager.Menu.Decline.WasPressedThisFrame())
            {
                PreviousMenu.SetActive(true);
                gameObject.SetActive(false);

                // todo: save
            }
        }

        /// <summary>
        /// Setup the options.
        /// </summary>
        private void SetupOptions()
        {
            menuOptions = new MenuOptionCollection();

            // Add options with the UI background image, and the method to call when the value is changed
            menuOptions.Add("Sound", ChangeSoundVolume, SoundBackground);
            menuOptions.Add("Music", ChangeMusicVolume, MusicBackground);
            menuOptions.Add("Fullscreen", ChangeFullscreen, FullscreenBackground);
            menuOptions.Add("Resolution", ChangeResolution, ResolutionBackground);
        }

        /// <summary>
        /// Set the initial values of each option.
        /// </summary>
        private void SetValues()
        {
            SoundValue.text = SaveManager.Data.SoundVolume.ToString();
            MusicValue.text = SaveManager.Data.MusicVolume.ToString();
            FullscreenValue.text = SaveManager.Data.Fullscreen ? ResourceUtil.Translate(YesStringId) : ResourceUtil.Translate(NoStringId);
            ResolutionValue.text = SaveManager.Data.Resolution;
        }

        /// <summary>
        /// Change the sound volume.
        /// </summary>
        /// <param name="value">The value to change the sound by.</param>
        private void ChangeSoundVolume(int value)
        {
            SaveManager.Data.SoundVolume += value;

            SaveManager.Data.SoundVolume = WrapVolume(SaveManager.Data.SoundVolume);

            SoundValue.text = SaveManager.Data.SoundVolume.ToString();

            // todo: change audio levels
        }

        /// <summary>
        /// Change the music volume.
        /// </summary>
        /// <param name="value">The value to change the music by.</param>
        private void ChangeMusicVolume(int value)
        {
            SaveManager.Data.MusicVolume += value;

            SaveManager.Data.MusicVolume = WrapVolume(SaveManager.Data.MusicVolume);

            MusicValue.text = SaveManager.Data.MusicVolume.ToString();

            // todo: change audio levels
        }

        /// <summary>
        /// Change the window size.
        /// </summary>
        /// <param name="_">Not required.</param>
        private void ChangeFullscreen(int _)
        {
            SaveManager.Data.Fullscreen = !SaveManager.Data.Fullscreen;

            // Set the text to Yes or No
            FullscreenValue.text = SaveManager.Data.Fullscreen ? ResourceUtil.Translate(YesStringId) : ResourceUtil.Translate(NoStringId);

            Screen.fullScreenMode = SaveManager.Data.Fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        }

        /// <summary>
        /// Change the window resolution.
        /// </summary>
        /// <param name="value">The value to change the resolution by.</param>
        private void ChangeResolution(int value)
        {
            // todo: resolution
        }

        /// <summary>
        /// Wrap the volume within the allowed bounds.
        /// </summary>
        /// <param name="volume">The current volume.</param>
        /// <returns>The adjusted volume.</returns>
        private int WrapVolume(int volume)
        {
            // If volume is too low, wrap to the highest
            if (volume < SaveData.MinVolume)
            {
                volume = SaveData.MaxVolume;
            }

            // If volume is too high, wrap to the lowest
            if (volume > SaveData.MaxVolume)
            {
                volume = SaveData.MinVolume;
            }

            return volume;
        }

        /// <summary>
        /// An event called when an input occurs.
        /// </summary>
        /// <param name="context">The context.</param>
        private void OnInputEvent(InputAction.CallbackContext context)
        {
            ChangeControllerPrompts();
        }

        /// <summary>
        /// Change the prompts for the controller.
        /// </summary>
        private void ChangeControllerPrompts()
        {
            string suffix = InputManager.GetControllerName();

            BackText.text = ResourceUtil.Translate($"{BackStringId}_{suffix}");
        }
    }
}