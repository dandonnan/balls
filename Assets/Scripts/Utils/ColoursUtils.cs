namespace Multiball.Utils
{
    using Multiball.Shared;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Utilities for colours.
    /// </summary>
    internal class ColoursUtils
    {
        /// <summary>
        /// Set the colour of an image on a sprite renderer.
        /// </summary>
        /// <param name="gameObject">The game object with the sprite renderer.</param>
        /// <param name="colour">The colour to use.</param>
        public static void SetSpriteRendererColour(GameObject gameObject, Colours colour)
        {
            // Get the object's SpriteRenderer component
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();

            if (image != null)
            {
                // Set the image's colour by converting the enum
                image.color = Convert(colour);
            }
        }

        /// <summary>
        /// Set the colour of an image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="colour">The colour to use.</param>
        public static void SetImageColour(Image image, Colours colour)
        {
            image.color = Convert(colour);
        }

        /// <summary>
        /// Convert the Colour enum into a Unity Color.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="alpha">An optional alpha parameter for transparency. Defaults to 1.</param>
        /// <returns>A Unity Color.</returns>
        public static Color Convert(Colours colour, float alpha = 1)
        {
            Color newColour;

            // Set an appropriate colour for each case
            switch (colour)
            {
                case Colours.Red:
                    newColour = Color.red;
                    break;

                case Colours.LightBlue:
                    newColour = new Color(0, 0.7f, 1);
                    break;

                case Colours.DarkBlue:
                    newColour = Color.blue;
                    break;

                case Colours.Green:
                    newColour = Color.green;
                    break;

                case Colours.Yellow:
                    newColour = Color.yellow;
                    break;

                case Colours.Grey:
                    newColour = new Color(0.8f, 0.8f, 0.8f);
                    break;

                case Colours.White:
                default:
                    newColour = Color.white;
                    break;
            }

            // Set the alpha for transparency
            newColour.a = alpha;

            return newColour;
        }
    }
}