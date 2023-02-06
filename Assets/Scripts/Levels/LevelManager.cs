namespace Multiball.Levels
{
    /// <summary>
    /// A level manager.
    /// </summary>
    internal class LevelManager
    {
        /// <summary>
        /// The id of the current level.
        /// </summary>
        public static int LevelId => levelManager.currentLevelId;

        /// <summary>
        /// Whether the level is paused.
        /// </summary>
        public static bool Paused => levelManager.paused;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static LevelManager levelManager;

        /// <summary>
        /// The number of balls needed to complete the level.
        /// </summary>
        private int numberOfBalls;

        /// <summary>
        /// The id of the current level.
        /// </summary>
        private int currentLevelId;

        /// <summary>
        /// Whether the level is paused.
        /// </summary>
        private bool paused;

        /// <summary>
        /// The level controller object.
        /// </summary>
        private LevelController levelController;

        /// <summary>
        /// A private constructor.
        /// </summary>
        private LevelManager()
        {
            levelManager = this;
            numberOfBalls = 0;
            currentLevelId = -1;
        }

        /// <summary>
        /// Initialise the level manager instance.
        /// </summary>
        public static void Initialise()
        {
            if (levelManager == null)
            {
                new LevelManager();
            }
        }

        /// <summary>
        /// Set the id of the level.
        /// </summary>
        /// <param name="id">The id of the level.</param>
        public static void SetLevelId(int id)
        {
            levelManager.currentLevelId = id;

            // Also reset the number of balls
            levelManager.numberOfBalls = 0;
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        /// <param name="pause">Whether or not to pause.</param>
        public static void Pause(bool pause = true)
        {
            levelManager.paused = pause;
        }

        /// <summary>
        /// Register a level controller with the manager.
        /// </summary>
        /// <param name="levelController">The level controller.</param>
        public static void RegisterLevelController(LevelController levelController)
        {
            levelManager.levelController = levelController;
        }

        /// <summary>
        /// Register a ball to keep track.
        /// </summary>
        public static void RegisterBall()
        {
            levelManager.numberOfBalls++;
        }

        /// <summary>
        /// Clear a ball.
        /// </summary>
        public static void ClearBall()
        {
            levelManager.numberOfBalls--;

            // If all balls are cleared, load the next level
            if (levelManager.numberOfBalls <= 0)
            {
                Pause(false);
                levelManager.levelController.LoadNextLevel();
            }
        }
    }
}