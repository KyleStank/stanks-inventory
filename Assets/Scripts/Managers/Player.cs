using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
    public static Player instance;

    //Private variables
    static GameObject realPlayer;
    static Inventory inventory;
    static PlayerUI ui = new PlayerUI();

    //Properties
    public static GameObject RealPlayer {
        get {
            CreateInstance();
            return realPlayer;
        }

        private set { realPlayer = value; }
    }

    public static Inventory Inventory {
        get {
            CreateInstance();
            return inventory;
        }

        private set { inventory = value; }
    }

    [SerializeField]
    public static PlayerUI UI {
        get {
            CreateInstance();
            return ui;
        }
    }

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject ui_inventory;
    [SerializeField]
    GameObject ui_itemsHolder;

    static void CreateInstance() {
        if(instance == null) { //If this game object doesn't exist, create it
            GameObject gm = new GameObject("Player Manager");
            gm.AddComponent<Player>();
        }
    }

    void Awake() {
        instance = this;

        //Set up the player stuff
        if(player == null)
            realPlayer = GameObject.Find("Player");
        else
            realPlayer = player;

        if(realPlayer == null) {
            Debug.LogError("Player not found!");
            return;
        }

        inventory = RealPlayer.GetComponent<Inventory>();

        //Setup player UI
        UI.Setup(ui_inventory, ui_itemsHolder);
    }

    void Update() {
        UI.DrawInventory();
    }

    //Classes
    public class PlayerUI {
        public GameObject inventory;
        public GameObject itemsHolder;
        public ItemUI[] items;

        //Setup the UI
        public void Setup(GameObject inventory, GameObject itemsHolder) {
            //Main inventory object
            if(inventory == null)
                this.inventory = GameObject.Find("Inventory");
            else
                this.inventory = inventory;

            //Items holder
            if(itemsHolder == null)
                this.itemsHolder = GameObject.Find("Items");
            else
                this.itemsHolder = itemsHolder;

            //Find each item
            items = new ItemUI[itemsHolder.transform.childCount];
            for(int i = 0; i < itemsHolder.transform.childCount; i++) {
                items[i] = new ItemUI();

                //Get the item game object
                GameObject itemGo = itemsHolder.transform.GetChild(i).gameObject;

                if(itemGo == null)
                    break;
                
                items[i].ItemGO = itemGo;

                //Get the item's icon
                Transform icon = items[i].ItemGO.transform.GetChild(0);

                if(icon == null)
                    break;
                
                //Disable icon
                items[i].ItemIMG = icon.GetComponent<Image>();
                items[i].ItemIMG.enabled = false;
            }

            //Set set the maximum possible inventory size
            Inventory.MaxItems = items.Length;
        }

        //Update/draw the inventory
        public void DrawInventory() {
            for(int i = 0; i < items.Length; i++) {
                if(Inventory.TakenSpace > i) {
                    Item item = Inventory.InventoryList[i];

                    items[i].UpdateItem(item, item.Icon);
                    items[i].ItemIMG.enabled = true;
                }
            }
        }

        public class ItemUI {
            Item item;
            GameObject itemGameObject;
            Image iconImage;

            /// <summary>
            /// The item that the inventory item/game object will display to users.
            /// </summary>
            public Item DisplayItem {
                get { return item; }
                set { item = value; }
            }

            /// <summary>
            /// The actual inventory game object of the item.
            /// </summary>
            public GameObject ItemGO {
                get { return itemGameObject; }
                set { itemGameObject = value; }
            }

            /// <summary>
            /// The item's image that will use a sprite to display the item to the user.
            /// </summary>
            public Image ItemIMG {
                get { return iconImage; }
                set { iconImage = value; }
            }

            public void UpdateItem(Item item) {
                if(item == null)
                    return;

                this.item = item;

                //Update the UI
                if(this.item.Icon == null)
                    return;

                iconImage.sprite = this.item.Icon;
            }

            public void UpdateItem(Item item, Sprite icon) {
                if(item == null)
                    return;
                else
                    if(icon == null)
                        UpdateItem(item);

                //Set the icon
                item.Icon = icon;

                UpdateItem(item);
            }
        }
    }
}
