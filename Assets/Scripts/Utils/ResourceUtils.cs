namespace Multiball.Resources
{
    using UnityEngine.Localization.Settings;

    /// <summary>
    /// Utilities for Resources.
    /// </summary>
    internal class ResourceUtil
    {
        /// <summary>
        /// Get a localised string from the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value of the key in the string table.</returns>
        public static string Translate(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(key);
        }
    }
}