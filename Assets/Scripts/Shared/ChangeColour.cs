namespace Multiball.Shared
{
    using Multiball.Levels;
    using Multiball.Utils;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A script for changing an image's colour.
    /// </summary>
    internal class ChangeColour : MonoBehaviour
    {
        /// <summary>
        /// The colour of the image.
        /// </summary>
        public Colours Colour;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out Image image))
            {
                ColoursUtils.SetImageColour(image, Colour);
            }
            else
            {
                // Set the colour of the background image
                ColoursUtils.SetSpriteRendererColour(gameObject, Colour);
            }
        }
    }
}