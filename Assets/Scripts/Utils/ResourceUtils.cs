namespace Multiball.Resources
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Localization.Settings;

    /// <summary>
    /// Utilities for Resources.
    /// </summary>
    internal class ResourceUtil
    {
        /// <summary>
        /// Load a list of data from a file in Resources.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="file">The file in the Resources folder to load from.</param>
        /// <returns>A list of data.</returns>
        public static List<T> LoadDataFromResource<T>(string file)
        {
            TextAsset asset = Resources.Load<TextAsset>(file);

            return JsonConvert.DeserializeObject<List<T>>(asset.text);
        }

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