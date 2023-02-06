namespace Multiball.Menu
{
    using Multiball.Shared;
    using Multiball.Utils;
    using System;
    using UnityEngine.UI;

    /// <summary>
    /// An option in a menu.
    /// </summary>
    internal class MenuOption
    {
        /// <summary>
        /// The name of the option.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The image.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Whether the option is highlighted.
        /// </summary>
        public bool Highlighted { get; private set; }

        /// <summary>
        /// The action to perform when the option is selected.
        /// </summary>
        public Action OnSelected { get; set; }

        /// <summary>
        /// The action to perform if the option's value changes.
        /// </summary>
        public Action<int> OnValueChanged { get; set; }

        /// <summary>
        /// Highlight the option.
        /// </summary>
        /// <param name="highlight">Whether or not to highlight the option. Defaults to true.</param>
        public void Highlight(bool highlight = true)
        {
            Highlighted = highlight;

            // Change the colour of the background image
            if (highlight)
            {
                ColoursUtils.SetImageColour(Image, Colours.LightBlue);
            }
            else
            {
                ColoursUtils.SetImageColour(Image, Colours.White);
            }
        }

        /// <summary>
        /// Select the option.
        /// </summary>
        public void Select()
        {
            OnSelected?.Invoke();
        }

        /// <summary>
        /// Change the value of the option.
        /// </summary>
        /// <param name="value">The value.</param>
        public void ChangeValue(int value)
        {
            OnValueChanged?.Invoke(value);
        }
    }
}
