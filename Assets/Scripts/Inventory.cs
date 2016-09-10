using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KStank.stanks_inventory {
    public sealed class InventoryData {
        public List<Item> InventoryList = new List<Item>();
        public int MaxItems = 0;

        public InventoryData(List<Item> InventoryList, int MaxItems) {
            this.InventoryList = InventoryList;
            this.MaxItems = MaxItems;
        }
    }

    public class Inventory : MonoBehaviour {
        //Private variables
        List<Item> inventoryList = new List<Item>();
        int maxItems = 0;

        [SerializeField]
        string fileName = "";

        //Properties
        /// <summary>
        /// The entire inventory.
        /// </summary>
        public List<Item> InventoryList {
            get { return inventoryList; }
            private set { inventoryList = value; }
        }

        /// <summary>
        /// The max amount of items allowed in the inventory.
        /// </summary>
        public int MaxItems {
            get { return maxItems; }
            set { maxItems = value; }
        }

        /// <summary>
        /// Amount of remaining space in inventory.
        /// </summary>
        public int OpenSpace {
            get { return MaxItems - TakenSpace; }
        }

        /// <summary>
        /// Amount of taken space in inventory. Same as InventoryList.Count.
        /// </summary>
        public int TakenSpace {
            get { return InventoryList.Count; }
        }

        //Public methods
        /// <summary>
        /// Searches for all items that have a provided name.
        /// </summary>
        /// <param name="name">Name of item(s) to search for.</param>
        /// <returns>Array of items that were found.</returns>
        public Item[] Find(string name, bool debug = false) {
            List<Item> items = new List<Item>();

            if(items != null) {
                //Search each item
                foreach(Item item in InventoryList)
                    if(item.Name == name)
                        items.Add(item);

                //Return as array
                return items.ToArray();
            } else
                if(debug)
                Debug.LogError("Could not find any items with the name of '" + name + "'!");

            return null;
        }

        /// <summary>
        /// Gets an item at a provided index. Returns null if that index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Item GetAt(int index, bool debug = false) {
            if(index < InventoryList.Count)
                return InventoryList[index];
            else
                if(debug)
                Debug.LogError("Inventory does not contain an item at [" + index.ToString() + "]!");

            return null;
        }

        /// <summary>
        /// Add an item to the inventory. Checks if there is room first.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(Item item, bool debug = false) {
            if(item != null) {
                if(IsRoom()) {
                    InventoryList.Add(item);
                } else {
                    if(debug) {
                        Debug.LogError("Cannot add the item '" + item.Name + "' " +
                            "to the inventory because there is no room!");
                    }
                }
            } else {
                if(debug) {
                    Debug.LogError("Cannot add the item '" + item.Name + "' " +
                        "to the inventory because it is null!");
                }
            }
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(Item item, bool debug = false) {
            if(item != null)
                InventoryList.Remove(item);
            else
                if(debug)
                Debug.LogError("Cannot remove item '" + item.Name + "' because it is null!");
        }

        /// <summary>
        /// Removes an item from the inventory at a provided index.
        /// </summary>
        /// <param name="index">Index to remove from.</param>
        public void Remove(int index, bool debug = false) {
            if(index < InventoryList.Count)
                InventoryList.RemoveAt(index);
            else
                if(debug)
                Debug.LogError("Inventory does not contain an item at [" + index.ToString() + "]!");
        }

        /// <summary>
        /// Removes an item or items from the inventory that have a matching name.
        /// </summary>
        /// <param name="name">Name to search for</param>
        public void Remove(string name) {
            Item[] items = Find(name);

            //Search each item
            foreach(Item item in items) {
                if(item.Name == name) //If we find a match
                    Remove(item);
            }
        }

        /// <summary>
        /// Picks up an object. Not the same as Inventory.Add(). This handles everything needed during an item pickup.
        /// </summary>
        /// <param name="item">Item that is being picked up.</param>
        public void Pickup(Item item) {
            if(!item.Collected && IsRoom()) { //If item hasn't already been picked up
                item.Collected = true;

                Add(item);
            }
        }

        /// <summary>
        /// Tells if there is or is not any room left in the inventory.
        /// </summary>
        /// <returns>True if there is room, false if not.</returns>
        public bool IsRoom() {
            if(OpenSpace > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Save the information of inventory.
        /// </summary>
        public void Save() {
            InventoryData data = new InventoryData(InventoryList, MaxItems);
            string path = Application.dataPath + "/" + fileName + ".json";
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            //Write to file
            using(StreamWriter writer = new StreamWriter(path)) {
                writer.WriteLine(json);
            }
        }
        
        /// <summary>
        /// Load all of the inventory data.
        /// </summary>
        public void Load() {
            string path = Application.dataPath + "/" + fileName + ".json";
            string json = "";

            //If file doesn't exist
            if(!File.Exists(path)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //Read json contents from file
            using(StreamReader streamReader = new StreamReader(path)) {
                json = streamReader.ReadToEnd();
            }
            
            //Load information
            InventoryData data = JsonConvert.DeserializeObject<InventoryData>(json);
            MaxItems = data.MaxItems;
            InventoryList = data.InventoryList;
        }
    }
}
