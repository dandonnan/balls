namespace Multiball.Extensions
{
    using Multiball.Save;

    /// <summary>
    /// Extension methods for SaveData.
    /// </summary>
    internal static class SaveDataExtensions
    {
        /// <summary>
        /// Get the width of the screen resolution.
        /// </summary>
        /// <param name="source">The save data.</param>
        /// <returns>The width of the screen resolution.</returns>
        public static int GetResolutionWidth(this SaveData source)
        {
            // Assuming the format is 0x1, get the value before x
            return int.Parse(source.Resolution.Substring(0, source.Resolution.IndexOf("x")));
        }

        /// <summary>
        /// Get the height of the screen resolution.
        /// </summary>
        /// <param name="source">The save data.</param>
        /// <returns>The height of the screen resolution.</returns>
        public static int GetResolutionHeight(this SaveData source)
        {
            // Assuming the format is 0x1, get the value after the x
            return int.Parse(source.Resolution.Substring(source.Resolution.IndexOf("x") + 1));
        }
    }
}