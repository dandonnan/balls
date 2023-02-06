namespace Multiball.Extensions
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Extension methods for the Input Actions.
    /// </summary>
    internal static class InputActionsExtensions
    {
        /// <summary>
        /// Get whether an input was pressed on the current frame, and output the
        /// movement delta.
        /// </summary>
        /// <param name="source">The input source.</param>
        /// <param name="movement">The movement delta. Defaults to (0, 0).</param>
        /// <returns>true if pressed, false otherwise.</returns>
        public static bool WasPressedThisFrame(this InputAction source, out Vector2 movement)
        {
            bool wasPressed = false;

            movement = Vector2.zero;

            if (source.WasPressedThisFrame())
            {
                wasPressed = true;
                movement = source.ReadValue<Vector2>();
            }

            return wasPressed;
        }

        /// <summary>
        /// Get whether an input was held, and outputs the movement delta.
        /// </summary>
        /// <param name="source">The input source.</param>
        /// <param name="movement">The movement delta. Defaults to (0, 0).</param>
        /// <returns>true if held, false otherwise.</returns>
        public static bool IsHeld(this InputAction source, out Vector2 movement)
        {
            bool wasPressed = false;

            movement = Vector2.zero;

            if (source.IsPressed())
            {
                wasPressed = true;
                movement = source.ReadValue<Vector2>();
            }

            return wasPressed;
        }
    }
}
