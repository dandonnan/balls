namespace Multiball.Menu
{
    using Multiball.Levels;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// A level option on the level select page.
    /// </summary>
    internal class LevelOption : MonoBehaviour
    {
        /// <summary>
        /// The image for the selected level highlight.
        /// </summary>
        public Image HighlightImage;

        /// <summary>
        /// The level icon.
        /// </summary>
        public Image Icon;

        /// <summary>
        /// The level id.
        /// </summary>
        private int levelId;

        /// <summary>
        /// Set the level id.
        /// </summary>
        /// <param name="levelId">The level id.</param>
        /// <param name="icon">The level icon.</param>
        public void SetLevelIdAndIcon(int levelId, Sprite icon)
        {
            this.levelId = levelId;
            Icon.sprite = icon;
        }

        /// <summary>
        /// Highlight the level.
        /// </summary>
        /// <param name="highlight">Whether or not to highlight the level. Defaults to true.</param>
        public void Highlight(bool highlight = true)
        {
            HighlightImage.gameObject.SetActive(highlight);
        }

        /// <summary>
        /// Load the level.
        /// </summary>
        public void LoadLevel()
        {
            LevelManager.Pause(false);
            LevelManager.SetLevelId(levelId);
            SceneManager.LoadScene("LevelScene");
        }
    }
}