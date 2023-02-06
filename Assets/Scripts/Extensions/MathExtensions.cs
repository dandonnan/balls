namespace Multiball.Extensions
{
    /// <summary>
    /// Extension methods for maths and numbers.
    /// </summary>
    internal static class MathExtensions
    {
        /// <summary>
        /// Invert a value to make it positive / negative.
        /// </summary>
        /// <param name="source">The value.</param>
        /// <returns>The inverted value.</returns>
        public static float Invert(this float source)
        {
            return source * -1;
        }
    }
}