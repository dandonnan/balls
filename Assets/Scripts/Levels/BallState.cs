namespace Multiball.Levels
{
    /// <summary>
    /// States that the Ball object can be in.
    /// </summary>
    internal enum BallState
    {
        /// <summary>
        /// The ball can be controlled.
        /// </summary>
        Controllable = 0,

        /// <summary>
        /// The ball is fading out.
        /// </summary>
        Fading = 1,

        /// <summary>
        /// The ball is paused.
        /// </summary>
        Paused = 2,
    }
}