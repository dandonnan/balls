namespace Multiball.Menu
{
    using Multiball.Audio;
    using Multiball.Extensions;
    using Multiball.Input;
    using Multiball.Resources;
    using Multiball.Save;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
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

        /// <summary>
        /// The background image for the language option.
        /// </summary>
        public Image LanguageBackground;

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
        /// The options to display.
        /// </summary>
        private MenuOptionCollection menuOptions;

        /// <summary>
        /// The index of the screen resolution.
        /// </summary>
        private int resolutionIndex;

        /// <summary>
        /// The index of the current locale.
        /// </summary>
        private int localeIndex;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            SetupOptions();
            SetValues();

            // Add events to be called when input occurs or the locale is changed
            InputManager.Menu.Get().actionTriggered += OnInputEvent;

            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

            // Get the current resolution
            Resolution resolution = Screen.resolutions.Where(r => r.width == SaveManager.Data.GetResolutionWidth()
                                                                && r.height == SaveManager.Data.GetResolutionHeight())
                                                      .FirstOrDefault();

            // Get the index of the current resolution
            resolutionIndex = Screen.resolutions.ToList().IndexOf(resolution);

            // Get the index of the current locale
            localeIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Remove events when actions are performed
            InputManager.Menu.Get().actionTriggered -= OnInputEvent;

            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
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
                AudioManager.PlaySound(SoundBack);

                PreviousMenu.SetActive(true);
                gameObject.SetActive(false);

                SaveManager.Save();
            }
        }

        /// <summary>
        /// Setup the options.
        /// </summary>
        private void SetupOptions()
        {
            menuOptions = new MenuOptionCollection();
            menuOptions.SetSounds(SoundMoveUp, SoundMoveDown, SoundConfirm);

            // Add options with the UI background image, and the method to call when the value is changed
            menuOptions.Add("Sound", ChangeSoundVolume, SoundBackground);
            menuOptions.Add("Music", ChangeMusicVolume, MusicBackground);
            menuOptions.Add("Fullscreen", ChangeFullscreen, FullscreenBackground);
            menuOptions.Add("Resolution", ChangeResolution, ResolutionBackground);
            menuOptions.Add("Language", ChangeLanguage, LanguageBackground);
        }

        /// <summary>
        /// Set the initial values of each option.
        /// </summary>
        private void SetValues()
        {
            SoundValue.text = SaveManager.Data.SoundVolume.ToString();
            MusicValue.text = SaveManager.Data.MusicVolume.ToString();
            ResolutionValue.text = SaveManager.Data.Resolution;

            SetFullscreenValue();
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

            // Play a sound
            PlayValueChangedSound(value);
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

            AudioManager.SetMusicVolume(SaveManager.Data.MusicVolume);

            // Play a sound
            PlayValueChangedSound(value);
        }

        /// <summary>
        /// Change the window size.
        /// </summary>
        /// <param name="_">Not required.</param>
        private void ChangeFullscreen(int _)
        {
            SaveManager.Data.Fullscreen = !SaveManager.Data.Fullscreen;

            SetFullscreenValue();

            Screen.fullScreenMode = SaveManager.Data.Fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

            AudioManager.PlaySound(SoundMoveUp);
        }

        /// <summary>
        /// Change the window resolution.
        /// </summary>
        /// <param name="value">The value to change the resolution by.</param>
        private void ChangeResolution(int value)
        {
            resolutionIndex += value;

            // Make sure the index is within the bounds of the array
            if (resolutionIndex < 0)
            {
                resolutionIndex = Screen.resolutions.Length - 1;
            }

            if (resolutionIndex >= Screen.resolutions.Length)
            {
                resolutionIndex = 0;
            }

            // Get the new resolution
            Resolution resolution = Screen.resolutions[resolutionIndex];

            // Set it on the window
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);

            // Set in in the save data
            SaveManager.Data.Resolution = $"{resolution.width}x{resolution.height}";

            // Display it on screen
            ResolutionValue.text = SaveManager.Data.Resolution;

            // Play a sound
            PlayValueChangedSound(value);
        }

        /// <summary>
        /// Change the language.
        /// </summary>
        /// <param name="value">The value to change the language by.</param>
        private void ChangeLanguage(int value)
        {
            localeIndex += value;

            int totalLocales = LocalizationSettings.AvailableLocales.Locales.Count;

            // Make sure the locale index is inside the list
            if (localeIndex < 0)
            {
                localeIndex = totalLocales - 1;
            }

            if (localeIndex >= totalLocales)
            {
                localeIndex = 0;
            }

            // Set the locale from the new index
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];

            // Save the code
            SaveManager.Data.LanguageCode = LocalizationSettings.SelectedLocale.Identifier.Code;

            // Play a sound
            PlayValueChangedSound(value);
        }

        /// <summary>
        /// Play a sound when a value has changed.
        /// </summary>
        /// <param name="value">The value</param>
        private void PlayValueChangedSound(int value)
        {
            if (value > 0)
            {
                AudioManager.PlaySound(SoundMoveDown);
            }
            else
            {
                AudioManager.PlaySound(SoundMoveUp);
            }
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
        /// An event called when the local is changed.
        /// </summary>
        /// <param name="locale">The locale.</param>
        private void OnLocaleChanged(Locale locale)
        {
            SetFullscreenValue();
        }

        /// <summary>
        /// Set the value of the fullscreen option.
        /// </summary>
        private void SetFullscreenValue()
        {
            // Set the text to Yes or No
            FullscreenValue.text = SaveManager.Data.Fullscreen ? StringUtils.Translate(YesStringId) : StringUtils.Translate(NoStringId);
        }

        /// <summary>
        /// Change the prompts for the controller.
        /// </summary>
        private void ChangeControllerPrompts()
        {
            // Get the name of the controller to use as a suffix on the string
            string suffix = InputManager.GetControllerName();

            // Display a back prompt with the controller's button
            BackText.text = StringUtils.Translate($"{BackStringId}_{suffix}");
        }
    }
}