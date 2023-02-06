namespace Multiball.Input
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// An input manager.
    /// </summary>
    internal class InputManager
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static InputManager inputManager;

        /// <summary>
        /// The input actions.
        /// </summary>
        private readonly InputActions inputActions;

        /// <summary>
        /// The name of the last input device.
        /// </summary>
        private string lastInputDeviceName;

        /// <summary>
        /// The type of the current controller.
        /// </summary>
        private SupportedControllers controllerType;

        /// <summary>
        /// A private constructor.
        /// </summary>
        private InputManager()
        {
            inputManager = this;

            // Initialise and enable the input actions
            inputActions = new InputActions();
            inputActions.Enable();

            // Set the controller type to the current device
            SetControllerType(SystemInfo.deviceName);

            // Add an event to trigger whenever input on the menu occurs
            inputActions.Menu.Get().actionTriggered += OnInputActionTriggered;
        }

        /// <summary>
        /// Initialise the input manager instance.
        /// </summary>
        public static void Initialise()
        {
            if (inputManager == null)
            {
                new InputManager();
            }
        }

        /// <summary>
        /// The input actions for Menu controls.
        /// </summary>
        public static InputActions.MenuActions Menu => inputManager.inputActions.Menu;

        /// <summary>
        /// The input actions for Game controls.
        /// </summary>
        public static InputActions.GameActions Game => inputManager.inputActions.Game;

        /// <summary>
        /// Get the name of the current controller type.
        /// </summary>
        /// <returns>The name of the current controller type.</returns>
        public static string GetControllerName()
        {
            return inputManager.controllerType.ToString();
        }

        /// <summary>
        /// An event fired when input occurs.
        /// </summary>
        /// <param name="context">The input context.</param>
        private void OnInputActionTriggered(InputAction.CallbackContext context)
        {
            // Get the name of the device from the context
            string deviceName = context.control.device.name;

            // If the device name is not the same as the last time an input occurred
            if (deviceName != lastInputDeviceName)
            {
                // Change the device name and controller type
                lastInputDeviceName = deviceName;

                SetControllerType(lastInputDeviceName);
            }
        }

        /// <summary>
        /// Set the type of controller based on the device name.
        /// </summary>
        /// <param name="deviceName">The device name.</param>
        private void SetControllerType(string deviceName)
        {
            switch (deviceName)
            {
                case "PS5":
                case "DualSenseGamepadHID":
                case "DualShock4GamepadHID":
                    controllerType = SupportedControllers.PlayStation;
                    break;

                case "XInputControllerWindows":
                    controllerType = SupportedControllers.Xbox;
                    break;

                case "Keyboard":
                default:
                    controllerType = SupportedControllers.PC;
                    break;
            }
        }
    }
}