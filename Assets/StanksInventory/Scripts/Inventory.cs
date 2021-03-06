﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace KStank.stanks_inventory {
    /// <summary>
    /// Inventory class. Simply use "new Inventory()" to create an Inventory for anything.
    /// </summary>
    public class Inventory : MonoBehaviour {
        public string fileName = "";

        //Private variables
        List<Item> inventoryList = new List<Item>();
        int maxItems = 0;

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

            Item i = Find(item.ID);

            if(IsRoom()) { //If there is room to add the item
                if(i == null)
                    inventoryList.Add(item);

                AssignCorrectItems();

                item.Position = TakenSpace - 1;
            } else {
                if(i != null)
                    if(i.StackSize < i.MaxStackSize)
                        i.StackSize++;
            }
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
            string directory = Util.streamingAssetsDir;
            string path = directory + fileName + ".json";

            AssignCorrectItems();

            InventoryJson data = new InventoryJson(InventoryList, MaxItems);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);

            //If directory doesn't exist
            if(!Directory.Exists(directory)) {
                Debug.Log(path + " does not exist!");
                return;
            }
            
            //Write to file
            using(StreamWriter writer = new StreamWriter(path))
                writer.WriteLine(json);
        }
        
        /// <summary>
        /// Load all of the inventory data.
        /// </summary>
        public void Load() {
            string directory = Util.streamingAssetsDir;
            string path = directory + fileName + ".json";

            string json = "";

            //If directory doesn't exist
            if(!Directory.Exists(directory)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //If file doesn't exist
            if(!File.Exists(path)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //Read json contents from file
            using(StreamReader streamReader = new StreamReader(path))
                json = streamReader.ReadToEnd();
            
            //Load information
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            InventoryJson data = JsonConvert.DeserializeObject<InventoryJson>(json, settings);
            MaxItems = data.MaxItems;
            InventoryList = data.InventoryList;

            AssignCorrectItems();
        }

        //Assign the correct items to the inventory from the Item Pool.
        void AssignCorrectItems() {
            //Assign items correctly
            for(int i = 0; i < TakenSpace; i++) {
                Item item = InventoryList[i];
                Item _item = Item.LookUpItem(item.ID);

                if(_item == null) {
                    Remove(item);
                    continue;
                }

                Item pastItem = item;
                item = _item;

                //Assign old values back.
                item.Name = pastItem.Name;
                item.Position = pastItem.Position;
                //item.StackSize = Mathf.Clamp(pastItem.StackSize, 0, pastItem.MaxStackSize);
                item.StackSize = pastItem.StackSize;
                item.MaxStackSize = pastItem.MaxStackSize;

                //Set "new" item
                InventoryList[i] = item;
            }
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
