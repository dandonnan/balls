namespace Multiball.Levels
{
    using Multiball.Input;
    using Multiball.Menu;
    using Multiball.Save;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// A controller for the current level.
    /// </summary>
    internal class LevelController : MonoBehaviour
    {
        /// <summary>
        /// The screen fader.
        /// </summary>
        public ScreenFade ScreenFader;

        /// <summary>
        /// The position to place levels.
        /// </summary>
        public Transform LevelSpawnPosition;

        /// <summary>
        /// The list of level prefabs.
        /// </summary>
        public List<GameObject> Levels;

        /// <summary>
        /// The pause menu.
        /// </summary>
        public PauseMenu PauseMenu;

        public int DebugStartLevelId;

        /// <summary>
        /// The id of the current level.
        /// </summary>
        private int levelId;

        /// <summary>
        /// The current level object.
        /// </summary>
        private GameObject currentLevel;

        /// <summary>
        /// The next level object.
        /// </summary>
        private GameObject nextLevel;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            if (DebugStartLevelId > -1)
            {
                LevelManager.SetLevelId(DebugStartLevelId);
            }

            // Register the controller with the level manager so it can call LoadNextLevel
            LevelManager.RegisterLevelController(this);

            // Immediately load the level from the level manager, and ignore fading
            LoadLevel(LevelManager.LevelId, true);
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        private void LateUpdate()
        {
            // If pause is press, pause the game and show the menu
            if (InputManager.Game.Pause.WasPressedThisFrame() && LevelManager.Paused == false)
            {
                LevelManager.Pause();
                PauseMenu.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Load the next level.
        /// </summary>
        public void LoadNextLevel()
        {
            // If the level id is at the end of the levels list (the last level)
            if (levelId >= Levels.Count - 1)
            {
                // Set the latest level to -1 to prevent Continue appearing on the menu
                SaveManager.Data.LatestLevel = -1;

                // Fade out
                ScreenFader.FadeOut(stayFaded: true);

                // Prepare an event once the screen has faded
                ScreenFader.ScreenFaded += OnScreenFaded_ReturnToMenu;
            }
            else
            {
                // Load the level from the next id
                LoadLevel(levelId + 1);
            }
        }

        /// <summary>
        /// Load a level.
        /// </summary>
        /// <param name="levelId">The level id.</param>
        /// <param name="ignoreFade">Whether to ignore the fade. Defaults to false.</param>
        private void LoadLevel(int levelId, bool ignoreFade = false)
        {
            // Set the level id, and update the ids in the save data
            this.levelId = levelId;

            SaveManager.Data.LatestLevel = levelId;

            if (SaveManager.Data.FurthestLevel < levelId)
            {
                SaveManager.Data.FurthestLevel = levelId;
            }

            // todo: save

            // Spawn the level from the prefab list
            nextLevel = Instantiate(Levels[levelId], LevelSpawnPosition);

            // If the screen will fade
            if (ignoreFade == false)
            {
                // Temporarily deactivate the next level
                nextLevel.SetActive(false);

                // Start screen fades
                ScreenFader.FadeOutThenIn();

                // Prepare an event once the screen has faded
                ScreenFader.ScreenFaded += OnScreenFaded_LoadLevel;
            }
            else
            {
                // Set the current level to be the next level
                currentLevel = nextLevel;
                nextLevel = null;
            }
        }

        /// <summary>
        /// Called when the screen has faded after a level has loaded.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="args">Event arguments.</param>
        private void OnScreenFaded_LoadLevel(object sender, EventArgs args)
        {
            // If there is a current level, then destroy it
            if (currentLevel != null)
            {
                Destroy(currentLevel);
            }

            // Set the current level to be the next level, and activate it
            currentLevel = nextLevel;
            currentLevel.SetActive(true);

            nextLevel = null;

            // Remove the screen fade event
            ScreenFader.ScreenFaded -= OnScreenFaded_LoadLevel;
        }

        /// <summary>
        /// Called when the screen has faded to return to the menu.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="args">Event arguments.</param>
        private void OnScreenFaded_ReturnToMenu(object sender, EventArgs args)
        {
            // Remove the screen fade event
            ScreenFader.ScreenFaded -= OnScreenFaded_ReturnToMenu;

            // Return to the menu
            SceneManager.LoadScene("MenuScene");
        }
    }
}