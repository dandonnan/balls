namespace Multiball.Menu
{
    using System;
    using System.Collections.Generic;
    using Multiball.Audio;
    using Multiball.Extensions;
    using Multiball.Input;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A collection of menu options.
    /// </summary>
    internal class MenuOptionCollection
    {
        /// <summary>
        /// The list of options.
        /// </summary>
        private readonly List<MenuOption> options;

        /// <summary>
        /// The currently highlighted option.
        /// </summary>
        private int currentOption;

        /// <summary>
        /// The name of the sound effect to play when moving up.
        /// </summary>
        private string soundMoveUp;

        /// <summary>
        /// The name of the sound effect to play when moving down.
        /// </summary>
        private string soundMoveDown;

        /// <summary>
        /// The name of the sound effect to play when selecting an option.
        /// </summary>
        private string soundConfirm;

        /// <summary>
        /// Create an empty collection.
        /// </summary>
        public MenuOptionCollection()
        {
            options = new List<MenuOption>();
            currentOption = 0;
            soundMoveUp = string.Empty;
            soundMoveDown = string.Empty;
            soundConfirm = string.Empty;
        }

        /// <summary>
        /// Set the sound effects.
        /// </summary>
        /// <param name="moveUp">The effect when moving up.</param>
        /// <param name="moveDown">The effect when moving down.</param>
        /// <param name="confirm">The effect when selecting an option.</param>
        public void SetSounds(string moveUp, string moveDown, string confirm)
        {
            soundMoveUp = moveUp;
            soundMoveDown = moveDown;
            soundConfirm = confirm;
        }

        /// <summary>
        /// Add an option to the collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="onSelected">The action to perform when the option is selected.</param>
        /// <param name="image">The image.</param>
        public void Add(string name, Action onSelected, Image image)
        {
            options.Add(new MenuOption
            {
                Name = name,
                OnSelected = onSelected,
                Image = image
            });

            if (options.Count == 1)
            {
                options[0].Highlight();
            }
        }

        /// <summary>
        /// Add an option to the collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="onValueChanged">The action to perform when the option's value changes.</param>
        /// <param name="image">The image.</param>
        public void Add(string name, Action<int> onValueChanged, Image image)
        {
            options.Add(new MenuOption
            {
                Name = name,
                OnValueChanged = onValueChanged,
                Image = image
            });

            if (options.Count == 1)
            {
                options[0].Highlight();
            }
        }

        /// <summary>
        /// Handle input.
        /// </summary>
        public void HandleInput()
        {
            // If the move input has been pressed
            if (InputManager.Menu.Move.WasPressedThisFrame(out Vector2 move))
            {
                // Movement in the y-axis should move the selection up and down
                if (move.y > 0)
                {
                    ShiftSelection(false);
                }
                else if (move.y < 0)
                {
                    ShiftSelection(true);
                }

                // Movements in the x-axis should change the value
                if (move.x > 0)
                {
                    options[currentOption].ChangeValue(1);
                }
                else if (move.x < 0)
                {
                    options[currentOption].ChangeValue(-1);
                }
            }

            // If the accept input is pressed, select the option
            if (InputManager.Menu.Accept.WasPressedThisFrame())
            {
                AudioManager.PlaySound(soundConfirm);
                options[currentOption].Select();
            }
        }

        /// <summary>
        /// Shift the highlighted option up or down.
        /// </summary>
        /// <param name="forward">Whether or not to move forward (down) or back (up) through the list.</param>
        private void ShiftSelection(bool forward = true)
        {
            // Unhighlight the current option
            options[currentOption].Highlight(false);

            // Move up or down, and loop to the start or end when reached
            currentOption = forward ?
                                currentOption == options.Count - 1 ? 0 : currentOption + 1
                                : currentOption == 0 ? options.Count - 1 : currentOption - 1;

            // Play sound effects
            if (forward)
            {
                AudioManager.PlaySound(soundMoveDown);
            }
            else
            {
                AudioManager.PlaySound(soundMoveUp);
            }

            // Highlight the new option
            options[currentOption].Highlight();
        }
    }
}
