using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory {
    public static class Util {
        public static string streamingAssetsDir = Application.dataPath + "\\StanksInventory" +
            "\\StreamingAssets\\";

        public static string itemPoolPath = streamingAssetsDir + "item-pool.json";
        public static string playerInventoryPath = streamingAssetsDir + "player-inventory.json";
        public static string itemIconsPath = "Item Icons/";
    }
}
