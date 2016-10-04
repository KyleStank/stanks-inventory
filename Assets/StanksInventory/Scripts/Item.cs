using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace KStank.stanks_inventory {
    /// <summary>
    /// Item class. Contains item pool, and other useful information for Items.
    /// </summary>
    [System.Serializable]
    public class Item {
        //Private variables
        //static Item[] itemPool = new Item[0];
        static List<Item> itemPool = new List<Item>();
        int id = 0;
        [SerializeField]
        string name = "";
        [SerializeField]
        Sprite icon = null;
        string iconName = "";
        int pos = 0;
        int stackSize = 0;
        int maxStackSize = 1;

        /*
        Properties
        */
        /// <summary>
        /// Array of every item in the game.
        /// </summary>
        public static List<Item> ItemPool {
            get { return itemPool; }
            private set { itemPool = value; }
        }
        
        /// <summary>
        /// ID of the item.
        /// </summary>
        public int ID {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Name of the Item.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The name of the icon that will be displayed.
        /// </summary>
        public string IconName {
            get {
                if(icon == null)
                    return iconName;

                return icon.name;
            }
            set { iconName = value; }
        }

        /// <summary>
        /// Position of the item in the slots.
        /// </summary>
        public int Position {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// Current stack size of item.
        /// </summary>
        public int StackSize {
            get { return stackSize; }
            set { stackSize = value; }
        }

        /// <summary>
        /// Max stack size of item.
        /// </summary>
        public int MaxStackSize {
            get { return maxStackSize; }
            set { maxStackSize = value; }
        }

        /*
        Public Static Methods
        */
        /// <summary>
        /// Searches the entire item pool for a specifc item.
        /// </summary>
        /// <param name="id">ID of the item to search for.</param>
        /// <returns>Item if it was found. Returns null if no item was found.</returns>
        public static Item LookUpItem(int id) {
            //Search entire item pool for ID
            foreach(Item i in ItemPool) {
                if(i.ID == id)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Removes an item from the item pool.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public static void RemoveItem(Item item) {
            if(item == null)
                return;

            Item _item = LookUpItem(item.ID);

            if(_item == null)
                return;

            ItemPool.Remove(item);
            //AssignCorrectIDS();
        }

        /// <summary>
        /// Loads the entire item pool from a file.
        /// </summary>
        public static void LoadItemPool() {
            string directory = Util.streamingAssetsDir;
            string path = Util.itemPoolPath;

            string json = "";

            //If directory doesn't exist
            if(!Directory.Exists(directory)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //If file doesn't exist
            if(!File.Exists(path))
                SaveItemPool();

            //Read json contents from file
            using(StreamReader streamReader = new StreamReader(path))
                json = streamReader.ReadToEnd();

            //Load information
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            ItemPool = JsonConvert.DeserializeObject<List<Item>>(json, settings);
        }

        /// <summary>
        /// Saves the entire item pool to a file.
        /// </summary>
        public static void SaveItemPool() {
            string directory = Util.streamingAssetsDir;
            string path = Util.itemPoolPath;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            string json = JsonConvert.SerializeObject(ItemPool, Formatting.Indented, settings);

            //If directory doesn't exist
            if(!Directory.Exists(directory)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //If file doesn't exist
            if(!File.Exists(path))
                SaveItemPool();

            //Write to file
            using(StreamWriter writer = new StreamWriter(path))
                writer.WriteLine(json);
        }

        /*
        Private Methods
        */
        //Assigns the correct IDs to each item. Useful after something is removed
        static void AssignCorrectIDS() {
            int lastId = 1;

            foreach(Item item in ItemPool) {
                Debug.Log("Name: " + item.Name + " - ID: " + item.ID);


            }
        }
        
        /// <summary>
        /// Empty constructor for Item.
        /// </summary>
        public Item() { }

        /// <summary>
        /// Object that can go inside of the inventory.
        /// </summary>
        /// <param name="Name">Name of the item.</param>
        /// <param name="Position">Position of the item.</param>
        public Item(string Name, int Position) {
            this.Name = Name;
            this.Position = Position;
        }
    }
}
