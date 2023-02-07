namespace Multiball.Menu
{
    using Multiball.Audio;
    using Multiball.Extensions;
    using Multiball.Input;
    using Multiball.Save;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The level select page.
    /// </summary>
    internal class LevelSelectPage : MonoBehaviour
    {
        /// <summary>
        /// A list of levels.
        /// </summary>
        public List<LevelOption> Levels;

        /// <summary>
        /// The number of rows displayed on the page.
        /// </summary>
        public int Rows;

        /// <summary>
        /// The level icons.
        /// </summary>
        public LevelIcons LevelIcons;

        /// <summary>
        /// The current selected index.
        /// </summary>
        private int selectedIndex;

        /// <summary>
        /// The number of levels per row.
        /// </summary>
        private int levelsPerRow;

        /// <summary>
        /// The index offset that the page starts at.
        /// </summary>
        private int pageOffset;

        /// <summary>
        /// The name of the sound effect to play when moving.
        /// </summary>
        private string soundMoving;

        /// <summary>
        /// Set the name of the sound effect to play.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        public void SetSound(string name)
        {
            soundMoving = name;
        }

        /// <summary>
        /// Setup the page.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="levelsPerPage">The number of levels per page.</param>
        public void Setup(int pageIndex, int levelsPerPage)
        {
            // Get the furthest level the player has been to
            int furthestLevel = SaveManager.Data.FurthestLevel;

            // Get the index of the first level on the page
            pageOffset = pageIndex * levelsPerPage;

            // Get the number of levels per row
            levelsPerRow = levelsPerPage / Rows;

            // Go through each level slot on the page
            for (int i=0; i<Levels.Count; i++)
            {
                // Remove the highlight
                Levels[i].Highlight(false);

                // Set the level id
                Levels[i].SetLevelIdAndIcon(i + pageOffset, LevelIcons.Icons[i + pageOffset]);

                // Only show levels that are the furthest level, or below
                Levels[i].gameObject.SetActive(i + pageOffset <= furthestLevel);
            }

            // Reset the selected level
            selectedIndex = 0;

            // Highlight the first level
            Levels[selectedIndex].Highlight();
        }

        /// <summary>
        /// Handle input.
        /// </summary>
        public void HandleInput()
        {
            // If the move input was pressed then handle movement
            if (InputManager.Menu.Move.WasPressedThisFrame(out Vector2 movement))
            {
                AudioManager.PlaySound(soundMoving);
                HandleHorizontalInput(movement);
                HandleVerticalInput(movement);
            }

            // If accept was pressed, load the level
            if (InputManager.Menu.Accept.WasPressedThisFrame())
            {
                Levels[selectedIndex].LoadLevel();
            }
        }

        /// <summary>
        /// Handle movement in the horizontal axis.
        /// </summary>
        /// <param name="movement">The movement.</param>
        private void HandleHorizontalInput(Vector2 movement)
        {
            int min = 0;
            int max = 0;

            // Todo: fix when more than 1 page

            // Go through each row
            for (int i=1; i<=Rows; i++)
            {
                // If the current level is on the row
                if (selectedIndex < levelsPerRow * i)
                {
                    // Set the minimum to the first index on the row
                    min = levelsPerRow * (i - 1);

                    // Set the maximum to be the last index on the row
                    max = i * (levelsPerRow - 1);

                    // If the furthest level is below the max, reduce the max
                    if (SaveManager.Data.FurthestLevel < max + pageOffset)
                    {
                        max = pageOffset - SaveManager.Data.FurthestLevel;
                    }
                    break;
                }                
            }

            // Move left and right, and wrap to the min / max when reaching the end / start
            if (movement.x > 0)
            {
                ChangeSelectedLevel(selectedIndex < max ? selectedIndex + 1 : min);
            }
            else if (movement.x < 0)
            {
                ChangeSelectedLevel(selectedIndex > min ? selectedIndex - 1 : max);
            }
        }

        /// <summary>
        /// Handle movement in the vertical axis.
        /// </summary>
        /// <param name="movement">The movement.</param>
        private void HandleVerticalInput(Vector2 movement)
        {
            int min = 0;
            int max = 0;

            // Todo: fix when more than 1 page

            // Go through each row
            for (int i = 1; i <= Rows; i++)
            {
                // If the current level is on the row
                if (selectedIndex < levelsPerRow * i)
                {
                    // Set the minimum to the first index in the column
                    min = selectedIndex - (levelsPerRow * (i - 1));

                    // Set the maximum to the last index in the column
                    max = selectedIndex + ((Rows - i) * levelsPerRow);

                    // If the furthest level is below the max, reduce it
                    if (SaveManager.Data.FurthestLevel < max + pageOffset)
                    {
                        max -= i * levelsPerRow;

                        if (SaveManager.Data.FurthestLevel < max + pageOffset)
                        {
                            max = selectedIndex;
                        }
                    }

                    break;
                }
            }

            // Move up and down, and wrap to the min / max when reaching the end / start
            if (movement.y > 0)
            {
                ChangeSelectedLevel(selectedIndex > min ? selectedIndex - levelsPerRow : max);
            }
            else if (movement.y < 0)
            {
                ChangeSelectedLevel(selectedIndex < max ? selectedIndex + levelsPerRow : min);
            }
        }

        /// <summary>
        /// Change the selected level.
        /// </summary>
        /// <param name="newSelection">The new level to select.</param>
        private void ChangeSelectedLevel(int newSelection)
        {
            // Remove the highlight from the currently selected level
            Levels[selectedIndex].Highlight(false);

            // Change the selected level
            selectedIndex = newSelection;

            // Set the highlight on the selected level
            Levels[selectedIndex].Highlight();
        }
    }
}