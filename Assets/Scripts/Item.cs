using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace KStank.stanks_inventory {
    /// <summary>
    /// Item class. Contains item pool, and other useful information for Items.
    /// </summary>
    [System.Serializable]
    public class Item {
        //Private variables
        static Item[] itemPool = new Item[0];
        int id = 0;
        [SerializeField]
        string name = "";
        [SerializeField]
        Sprite icon = null;
        string iconName = "";
        int pos = 0;
        int stackSize = 0;
        int maxStackSize = 1;

        //Saving variables
        static string fileName = "item-pool.json";

        /*
        Properties
        */
        /// <summary>
        /// Array of every item in the game.
        /// </summary>
        public static Item[] ItemPool {
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
            foreach(Item i in itemPool) {
                if(i.ID == id)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Loads the entire item pool from a file.
        /// </summary>
        public static void LoadItemPool() {
            string directory = Application.streamingAssetsPath;
            string path = directory + "/" + fileName;
            string json = "";

            //If directory doesn't exist
            if(!Directory.Exists(directory)) {
                Debug.Log(path + " does not exist!");
                return;
            }

            //If file doesn't exist
            if(!File.Exists(path))
                InitItemPool();

            //Read json contents from file
            using(StreamReader streamReader = new StreamReader(path))
                json = streamReader.ReadToEnd();

            //Load information
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            ItemPool = JsonConvert.DeserializeObject<Item[]>(json, settings);
        }

        /*
        Private Methods
        */
        static void InitItemPool() {
            string directory = Application.streamingAssetsPath;
            string path = directory + "/" + fileName;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            string json = JsonConvert.SerializeObject(ItemPool, Formatting.Indented, settings);

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
