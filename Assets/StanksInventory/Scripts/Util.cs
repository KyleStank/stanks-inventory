using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory {
    public static class Util {
        public static string streamingAssetsDir = Application.dataPath + "\\StanksInventory" +
            "\\StreamingAssets\\";

        public static string itemPoolPath = streamingAssetsDir + "item-pool.json";
        public static string playerInventoryPath = streamingAssetsDir + "player-inventory.json";
        public static string itemIconsPath = "Item Icons/";

        /// <summary>
        /// Loads an icon from the Resources folder.
        /// </summary>
        /// <param name="iconName">Name of icon to load.</param>
        /// <returns>The loaded icon.</returns>
        public static Sprite LoadIcon(string iconName) {
            return Resources.Load(itemIconsPath + iconName, typeof(Sprite)) as Sprite;
        }

        /// <summary>
        /// Removes all characters from a string that come after a certain word or set of characters.
        /// </summary>
        /// <param name="value">String that contains word or characters.</param>
        /// <param name="word">Word to characters to look for.</param>
        /// <returns>Modified string that has been shortened.</returns>
        public static string RemoveAfter(string value, string word) {
            int index = value.IndexOf(word);

            if(index < 0)
                return "";

            return value.Substring(0, index);
        }
    }
}
