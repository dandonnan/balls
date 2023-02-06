namespace Multiball.Menu
{
    using Multiball.Levels;
    using Multiball.Resources;
    using Multiball.Save;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// The main menu.
    /// </summary>
    internal class MainMenu : MonoBehaviour
    {
        [Header("UI")]
        /// <summary>
        /// The background image for the top option.
        /// </summary>
        public Image Option1Background;

        /// <summary>
        /// The text value for the top option.
        /// </summary>
        public TMP_Text Option1Text;

        /// <summary>
        /// The background image for the second option.
        /// </summary>
        public Image Option2Background;

        /// <summary>
        /// The text value for the second option.
        /// </summary>
        public TMP_Text Option2Text;

        /// <summary>
        /// The background image for the options option.
        /// </summary>
        public Image OptionsBackground;

        /// <summary>
        /// The background image for the quit option.
        /// </summary>
        public Image QuitBackground;

        [Header("Strings")]
        /// <summary>
        /// The string id for the option to Start a new game.
        /// </summary>
        public string StartStringId;

        /// <summary>
        /// The string id for the option to Continue a game.
        /// </summary>
        public string ContinueStringId;

        /// <summary>
        /// The string id for the level select option.
        /// </summary>
        public string LevelStringId;

        [Header("Menus")]
        /// <summary>
        /// The level select menu.
        /// </summary>
        public LevelSelect LevelSelect;
        
        /// <summary>
        /// The options menu.
        /// </summary>
        public OptionsMenu OptionsMenu;

        /// <summary>
        /// The options on the main menu.
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
        }

        /// <summary>
        /// Setup the options on the main menu.
        /// </summary>
        private void SetupOptions()
        {
            menuOptions = new MenuOptionCollection();

            // If the player has played before
            if (SaveManager.Data.LatestLevel > -1)
            {
                // Add the Continue option
                menuOptions.Add("Continue", LoadLastPlayedLevel, Option1Background);
                Option1Text.text = ResourceUtil.Translate(ContinueStringId);
            }
            else
            {
                // If the player has never played before
                if (SaveManager.Data.FurthestLevel == 0)
                {
                    // Display Start as the second option, and disable the first option
                    menuOptions.Add("Start", StartNewGame, Option2Background);
                    Option2Text.text = ResourceUtil.Translate(StartStringId);
                    Option1Background.gameObject.SetActive(false);
                }
                else
                {
                    // If the player has played before, but has completed all levels, add Start to the top
                    menuOptions.Add("Start", StartNewGame, Option1Background);
                    Option1Text.text = ResourceUtil.Translate(StartStringId);
                }
            }

            // If the player has gone past the first level
            if (SaveManager.Data.LatestLevel > -1 || SaveManager.Data.FurthestLevel > 0)
            {
                // Add the Level Select option
                menuOptions.Add("Levels", OpenLevelSelect, Option2Background);
                Option2Text.text = ResourceUtil.Translate(LevelStringId);
            }

            // Always show Options and Quit
            menuOptions.Add("Options", DisplayOptionsMenu, OptionsBackground);
            menuOptions.Add("Quit", QuitGame, QuitBackground);
        }

        /// <summary>
        /// Load the level that was played last.
        /// </summary>
        private void LoadLastPlayedLevel()
        {
            LoadIntoGame(SaveManager.Data.LatestLevel);
        }

        /// <summary>
        /// Start a new game.
        /// </summary>
        private void StartNewGame()
        {
            LoadIntoGame(0);
        }

        /// <summary>
        /// Load into the game at the given level.
        /// </summary>
        /// <param name="levelId">The id of the level.</param>
        private void LoadIntoGame(int levelId)
        {
            LevelManager.Pause(false);
            LevelManager.SetLevelId(levelId);
            SceneManager.LoadScene("LevelScene");
        }

        /// <summary>
        /// Open the level select menu.
        /// </summary>
        private void OpenLevelSelect()
        {
            LevelSelect.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Open the options menu.
        /// </summary>
        private void DisplayOptionsMenu()
        {
            OptionsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Close the game.
        /// </summary>
        private void QuitGame()
        {
            Application.Quit();
        }
    }
}