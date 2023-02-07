namespace Multiball.Menu
{
    using Multiball.Audio;
    using Multiball.Input;
    using Multiball.Resources;
    using Multiball.Save;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    /// <summary>
    /// The level select screen.
    /// </summary>
    internal class LevelSelect : MonoBehaviour
    {
        [Header("UI")]
        /// <summary>
        /// The level select page container.
        /// </summary>
        public LevelSelectPage PageContainer;

        /// <summary>
        /// The image for the left tab.
        /// </summary>
        public Image TabLeft;

        /// <summary>
        /// The image for the right tab.
        /// </summary>
        public Image TabRight;

        [Header("Strings")]
        /// <summary>
        /// The text for the left tab.
        /// </summary>
        public TMP_Text TabLeftText;

        /// <summary>
        /// The text for the right tab.
        /// </summary>
        public TMP_Text TabRightText;

        /// <summary>
        /// The text for the back prompt.
        /// </summary>
        public TMP_Text BackText;

        /// <summary>
        /// The id of the prefix to use for the button prompt on the left tab.
        /// </summary>
        public string TabLeftPrefixId;

        /// <summary>
        /// The id of the prefix to use for the button prompt on the right tab.
        /// </summary>
        public string TabRightPrefixId;

        /// <summary>
        /// The id of the prefix to use for the button prompt to go back.
        /// </summary>
        public string BackPrefixId;

        [Header("Setup")]
        /// <summary>
        /// The number of levels shown on each page.
        /// </summary>
        public int LevelsPerPage;

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages;

        /// <summary>
        /// The menu that was shown before this one.
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
        /// The current page.
        /// </summary>
        private int currentPage;

        /// <summary>
        /// The final page that can be shown to the player.
        /// </summary>
        private int maxPage;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            SetTabDisplay();
            PageContainer.SetSound(SoundMoveDown);
            PageContainer.Setup(currentPage, LevelsPerPage);

            // Set the max page the player can change to based on their furthest level
            maxPage = Mathf.FloorToInt(SaveManager.Data.FurthestLevel / (float)LevelsPerPage);

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
            HandleInput();
        }

        /// <summary>
        /// Handle input.
        /// </summary>
        private void HandleInput()
        {
            // Change to the previous tab
            if (InputManager.Menu.TabLeft.WasPressedThisFrame() && currentPage > 0)
            {
                AudioManager.PlaySound(SoundMoveUp);
                ChangeTab(currentPage-1);
            }

            // Change to the next tab
            if (InputManager.Menu.TabRight.WasPressedThisFrame() && currentPage < maxPage)
            {
                AudioManager.PlaySound(SoundMoveDown);
                ChangeTab(currentPage + 1);
            }

            // Go to the previous menu
            if (InputManager.Menu.Decline.WasPressedThisFrame())
            {
                AudioManager.PlaySound(SoundBack);
                PreviousMenu.SetActive(true);
                gameObject.SetActive(false);
            }

            PageContainer.HandleInput();
        }

        /// <summary>
        /// Change the tab.
        /// </summary>
        /// <param name="newTab">The index of the new tab.</param>
        private void ChangeTab(int newTab)
        {
            currentPage = newTab;
            SetTabDisplay();

            PageContainer.Setup(currentPage, LevelsPerPage);
        }

        /// <summary>
        /// Set whether or not the tabs display.
        /// </summary>
        private void SetTabDisplay()
        {
            // Only show the left tab if not on the first page
            TabLeft.gameObject.SetActive(currentPage > 0);

            // Don't display the right tab if the player hasn't played everything on this page
            bool showRightTab = SaveManager.Data.FurthestLevel > (currentPage + 1) * LevelsPerPage;

            TabRight.gameObject.SetActive(showRightTab);
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

            TabLeftText.text = StringUtils.Translate($"{TabLeftPrefixId}_{suffix}");
            TabRightText.text = StringUtils.Translate($"{TabRightPrefixId}_{suffix}");
            BackText.text = StringUtils.Translate($"{BackPrefixId}_{suffix}");
        }
    }
}