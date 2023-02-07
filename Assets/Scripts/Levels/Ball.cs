namespace Multiball.Levels
{
    using Multiball.Audio;
    using Multiball.Extensions;
    using Multiball.Input;
    using Multiball.Shared;
    using Multiball.Utils;
    using UnityEngine;

    /// <summary>
    /// The Ball object.
    /// </summary>
    internal class Ball : MonoBehaviour
    {
        [Header("Controls")]
        /// <summary>
        /// Whether controls are inverted horizontally.
        /// </summary>
        public bool InvertHorizontal;

        /// <summary>
        /// Whether controls are inverted vertically.
        /// </summary>
        public bool InvertVertical;

        /// <summary>
        /// The speed the ball moves.
        /// </summary>
        public float MovementSpeed;

        [Header("Physics")]
        /// <summary>
        /// The layer the ball collides with.
        /// </summary>
        public LayerMask CollisionLayer;

        /// <summary>
        /// The height to use in collision checks.
        /// </summary>
        public float CollisionHeight;

        /// <summary>
        /// The height the ball jumps.
        /// </summary>
        public float JumpHeight;

        /// <summary>
        /// The cooldown between jumps.
        /// </summary>
        public float JumpCooldown;

        [Header("Visuals")]
        /// <summary>
        /// The colour of the ball.
        /// </summary>
        public Colours Colour;

        [Header("Audio")]
        /// <summary>
        /// The sound to play when jumping.
        /// </summary>
        public string JumpSound;

        /// <summary>
        /// The sound to play when reaching the exit.
        /// </summary>
        public string ExitSound;

        /// <summary>
        /// Whether the ball can jump.
        /// </summary>
        private bool canJump;

        /// <summary>
        /// A cooldown between jumps.
        /// </summary>
        private float jumpCooldown;

        /// <summary>
        /// The rigidbody component.
        /// </summary>
        private Rigidbody2D rigidbody2d;

        /// <summary>
        /// The sprite renderer component.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The ball's state.
        /// </summary>
        private BallState state;

        /// <summary>
        /// The ball's transparency.
        /// </summary>
        private float alpha;

        /// <summary>
        /// The gravity scale before being paused.
        /// </summary>
        private float gravityBeforePause;

        /// <summary>
        /// The velocity before being paused.
        /// </summary>
        private Vector2 velocityBeforePause;

        /// <summary>
        /// Called when the ball spawns.
        /// </summary>
        private void Start()
        {
            // Set initial values
            state = BallState.Controllable;
            alpha = 1;

            // Change the ball's colour
            ColoursUtils.SetSpriteRendererColour(gameObject, Colour);

            // Get other components
            rigidbody2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Invert gravity on the rigidbody, if needed
            InvertGravityWhenUpsideDown();

            // Register the ball with the manager to track when the level ends
            LevelManager.RegisterBall();
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        private void LateUpdate()
        {
            // Call the relevant update method based on the state.
            switch (state)
            {
                case BallState.Controllable:
                    UpdateControllable();
                    break;

                case BallState.Paused:
                    UpdatePaused();
                    break;

                case BallState.Fading:
                    UpdateFading();
                    break;
            }
        }

        /// <summary>
        /// Called when the ball enters an object's trigger area.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If the ball is controllable and the trigger was an Exit point
            if (state == BallState.Controllable && collision.tag == "Exit")
            {
                ReachedExit();
            }
        }

        /// <summary>
        /// Update the ball when it is controllable.
        /// </summary>
        private void UpdateControllable()
        {
            if (LevelManager.Paused == false)
            {
                HandleMovement();
                HandleJump();
                HandleGravity();
            }
            else
            {
                // If the game has been paused, get the physics state
                velocityBeforePause = rigidbody2d.velocity;
                gravityBeforePause = rigidbody2d.gravityScale;

                // Turn the physics off
                rigidbody2d.velocity = Vector2.zero;
                rigidbody2d.gravityScale = 0;

                state = BallState.Paused;
            }
        }

        /// <summary>
        /// Update the ball when it is paused.
        /// </summary>
        private void UpdatePaused()
        {
            if (LevelManager.Paused == false)
            {
                // Reset the physics to the state before pausing
                rigidbody2d.velocity = velocityBeforePause;
                rigidbody2d.gravityScale = gravityBeforePause;

                state = BallState.Controllable;
            }
        }

        /// <summary>
        /// Update the ball when it is fading out.
        /// </summary>
        private void UpdateFading()
        {
            // Reduce the transparency
            alpha -= Time.deltaTime;

            // Set the alpha on the sprite renderer
            spriteRenderer.color = ColoursUtils.Convert(Colour, alpha);

            // If the ball is no longer visible, destroy it
            if (alpha <= 0)
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// Handle movement.
        /// </summary>
        private void HandleMovement()
        {
            // If the movement input being held
            if (InputManager.Game.Move.IsHeld(out Vector2 movement))
            {
                // Get the force in the x-axis (direction of the input * speed * time)
                float xForce = movement.x * MovementSpeed * Time.deltaTime;

                // If the controls are inverted, then invert the force
                if (InvertHorizontal)
                {
                    xForce = xForce.Invert();
                }

                // Add the force to the rigidbody
                rigidbody2d.AddRelativeForce(new Vector2(xForce, 0), ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// Handle jumping.
        /// </summary>
        private void HandleJump()
        {
            // If the cooldown is greater than 0, then reduce it
            if (jumpCooldown > 0)
            {
                jumpCooldown -= Time.deltaTime;
            }

            // If jump was pressed, or the up button was pressed, and the ball can jump
            if ((InputManager.Game.Jump.WasPressedThisFrame() || WasUpPressed()) && canJump)
            {
                // Set the jump height, and invert it if controls are inverted
                float jumpHeight = InvertVertical ? JumpHeight.Invert() : JumpHeight;

                // Stop the ball from jumping again
                canJump = false;

                // Set the cooldown
                jumpCooldown = JumpCooldown;

                // Add a jump force to the rigidbody
                rigidbody2d.AddForce(new Vector2(0, jumpHeight));

                // Play a jump sound
                AudioManager.PlaySound(JumpSound);
            }
        }

        /// <summary>
        /// Handle gravity.
        /// </summary>
        private void HandleGravity()
        {
            // Create an array of results - these are never actually checked, so set the size to 1.
            RaycastHit2D[] raycastResults = new RaycastHit2D[1];

            // Create a filter to limit raycasting to the collision layer
            ContactFilter2D filter = new ContactFilter2D
            {
                layerMask = CollisionLayer,
                useLayerMask = true,
            };

            // Set the direction of a raycast to be down, or up if controls are inverted
            Vector2 direction = InvertVertical ? Vector2.up : Vector2.down;

            // Raycast to see if the ball has collided with anything
            int hits = Physics2D.Raycast(transform.position, direction, filter, raycastResults, CollisionHeight);

            // If there were any collisions, and the jump cooldown is finished
            if (hits > 0 && jumpCooldown <= 0)
            {
                // Allow jumping
                canJump = true;
            }
        }

        /// <summary>
        /// Get whether the Up movement input was pressed.
        /// </summary>
        /// <returns>true if pressed, false if not.</returns>
        private bool WasUpPressed()
        {
            bool pressed = false;

            if (InputManager.Game.Move.WasPressedThisFrame(out Vector2 movement))
            {
                pressed = movement.y > 0;
            }

            return pressed;
        }

        /// <summary>
        /// Handle when the ball has reached an exit.
        /// </summary>
        private void ReachedExit()
        {
            // Play a sound
            AudioManager.PlaySound(ExitSound);

            // Invert the gravity if required - to stop balls floating away
            InvertGravityWhenUpsideDown();

            // Set the state to fading out
            state = BallState.Fading;

            // Tell the level manager a ball has reached the exit
            LevelManager.ClearBall();
        }

        /// <summary>
        /// Invert the gravity of the ball if it is upside down.
        /// </summary>
        private void InvertGravityWhenUpsideDown()
        {
            // Only invert if there is a rigidbody component and vertical controls should be inverted
            if (rigidbody2d != null && InvertVertical)
            {
                rigidbody2d.gravityScale = rigidbody2d.gravityScale.Invert();
            }
        }
    }
}