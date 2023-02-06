namespace Multiball.Levels
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Handle screen fade.
    /// </summary>
    internal class ScreenFade : MonoBehaviour
    {
        /// <summary>
        /// An image used as an overlay.
        /// </summary>
        public Image Overlay;

        /// <summary>
        /// An event fired when the screen has faded.
        /// </summary>
        public event EventHandler ScreenFaded;

        /// <summary>
        /// Whether or not to fade out.
        /// </summary>
        private bool fadeOut;

        /// <summary>
        /// Whether to fade in after fading out.
        /// </summary>
        private bool fadeInAfterOut;

        /// <summary>
        /// Whether or not to stay faded.
        /// </summary>
        private bool stayFaded;

        /// <summary>
        /// Whether the overlay is active.
        /// </summary>
        private bool active;

        /// <summary>
        /// The transparency of the overlay.
        /// </summary>
        private float alpha;

        /// <summary>
        /// Called when the object spawns.
        /// </summary>
        private void Start()
        {
            FadeIn();
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        private void LateUpdate()
        {
            if (active)
            {
                Fade();
            }
        }

        /// <summary>
        /// Start a fade in.
        /// </summary>
        public void FadeIn()
        {
            SetOverlayActive(true);
            fadeInAfterOut = false;
            fadeOut = false;
            alpha = 1;
        }

        /// <summary>
        /// Start a fade out.
        /// </summary>
        /// <param name="stayFaded">Whether or not to stay faded. Defaults to false.</param>
        public void FadeOut(bool stayFaded = false)
        {
            SetOverlayActive(true);
            fadeOut = true;
            fadeInAfterOut = false;
            this.stayFaded = stayFaded;
            alpha = 0;
        }

        /// <summary>
        /// Start a fade out, and fade in afterwards.
        /// </summary>
        public void FadeOutThenIn()
        {
            FadeOut();
            fadeInAfterOut = true;
        }

        /// <summary>
        /// Fade the screen.
        /// </summary>
        private void Fade()
        {
            // Change the transparency, and deactivate the overlay once finished
            if (fadeOut)
            {
                alpha += Time.deltaTime;

                if (alpha >= 1)
                {
                    if (fadeInAfterOut)
                    {
                        OnScreenFaded();
                        FadeIn();
                    }
                    else
                    {
                        OnScreenFaded();
                        SetOverlayActive(stayFaded);
                    }
                }
            }
            else
            {
                alpha -= Time.deltaTime;

                if (alpha <= 0)
                {
                    OnScreenFaded();
                    SetOverlayActive(false);
                }
            }

            // Set the transparency on the overlay's colour
            Color colourWithAlpha = Color.white;
            
            colourWithAlpha.a = alpha;

            Overlay.color = colourWithAlpha;
        }

        /// <summary>
        /// Activate, or deactivate, the overlay.
        /// </summary>
        /// <param name="active">Whether to activate (true) or deactivate (false) the overlay.</param>
        private void SetOverlayActive(bool active)
        {
            this.active = active;
            Overlay.gameObject.SetActive(active);
        }

        /// <summary>
        /// Fire the screen faded event.
        /// </summary>
        private void OnScreenFaded()
        {
            ScreenFaded?.Invoke(this, EventArgs.Empty);
        }
    }
}