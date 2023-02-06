namespace Multiball.Save
{
    using UnityEngine;
    using Newtonsoft.Json;

    /// <summary>
    /// The manager for save data.
    /// </summary>
    internal class SaveManager
    {
        /// <summary>
        /// The key for where save data is stored in PlayerPrefs.
        /// </summary>
        private const string playerPrefsKey = "MultiballData";

        /// <summary>
        /// The save data.
        /// </summary>
        public static SaveData Data => saveManager.saveData;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static SaveManager saveManager;

        /// <summary>
        /// The save data.
        /// </summary>
        private readonly SaveData saveData;

        /// <summary>
        /// A private constructor.
        /// </summary>
        private SaveManager()
        {
            saveManager = this;

            saveData = Load();
        }

        /// <summary>
        /// Initialises the save manager instance.
        /// </summary>
        public static void Initialise()
        {
            if (saveManager == null)
            {
                new SaveManager();
            }
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        public static void Save()
        {
            // Serialise the save data to a string
            string data = JsonConvert.SerializeObject(saveManager.saveData);

            // Store that string in PlayerPrefs
            PlayerPrefs.SetString(playerPrefsKey, data);

            // Save changes made to PlayerPrefs
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load the game data.
        /// </summary>
        /// <returns>The loaded data.</returns>
        private static SaveData Load()
        {
            // Create a new save data using default values
            SaveData data = new SaveData();

            // Set a key to use if there is no data in PlayerPrefs
            string defaultKey = "nodata";

            // Get data stored in PlayerPrefs
            string storedData = PlayerPrefs.GetString(playerPrefsKey, defaultKey);

            // If the stored data does not match the key indicated there is no data
            if (storedData != defaultKey)
            {
                // Deserialise the data
                data = JsonConvert.DeserializeObject<SaveData>(storedData);
            }

            return data;
        }
    }
}
