using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace KStank.stanks_inventory {
    /// <summary>
    /// Inventory class. Simply use "new Inventory()" to create an Inventory for anything.
    /// </summary>
    public class Inventory : MonoBehaviour {
        //Private variables
        List<Item> inventoryList = new List<Item>();
        int maxItems = 0;

        [SerializeField]
        string fileName = "";

        /*
        Properties
        */
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

        /*
        Public Methods
        */
        /// <summary>
        /// Searches all items for a specific item.
        /// </summary>
        /// <param name="id">ID of item to search for.</param>
        /// <returns>Item that was found. If none was found, returns null.</returns>
        public Item Find(int id) {
            //Search each item
            Item item = InventoryList.Find(i => i.ID == id);

            return item;
        }

        /// <summary>
        /// Gets an item at a provided index. Returns null if that index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Item GetAt(int index) {
            if(index < InventoryList.Count)
                return InventoryList[index];

            return null;
        }

        /// <summary>
        /// Add an item to the inventory. Checks if there is room first.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(Item item) {
            if(item == null)
                return;

            if(IsRoom()) { //If there is room to add the item
                InventoryList.Add(item);

                item.Position = TakenSpace - 1;
            }
        }

        public void Add(int id) {
            Add(Find(id));
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(Item item) {
            if(item == null)
                return;

            InventoryList.Remove(item);
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="index">ID of Item to remove.</param>
        public void Remove(int id) {
            Remove(Find(id));
        }

        /// <summary>
        /// Picks up an object. Not the same as Inventory.Add(). This handles everything needed during an item pickup.
        /// </summary>
        /// <param name="item">Item that is being picked up.</param>
        public void Pickup(Item item) {
            if(!item.Collected && IsRoom()) { //If item hasn't already been picked up
                if(Find(item.ID) != null)
                    return;

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
            InventoryJson data = new InventoryJson(InventoryList, MaxItems);
            string path = Application.dataPath + "/Data/" + fileName + ".json";
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            //Write to file
            using(StreamWriter writer = new StreamWriter(path))
                writer.WriteLine(json);
        }
        
        /// <summary>
        /// Load all of the inventory data.
        /// </summary>
        public void Load() {
            string path = Application.dataPath + "/Data/" + fileName + ".json";
            string json = "";

            //If file doesn't exist
            if(!File.Exists(path)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //Read json contents from file
            using(StreamReader streamReader = new StreamReader(path))
                json = streamReader.ReadToEnd();

            //Load information
            InventoryJson data = JsonConvert.DeserializeObject<InventoryJson>(json);
            MaxItems = data.MaxItems;
            InventoryList = data.InventoryList;
        }

        class InventoryJson {
            public List<Item> InventoryList = new List<Item>();
            public int MaxItems = 0;

            public InventoryJson(List<Item> InventoryList, int MaxItems) {
                this.InventoryList = InventoryList;
                this.MaxItems = MaxItems;
            }
        }
    }
}
