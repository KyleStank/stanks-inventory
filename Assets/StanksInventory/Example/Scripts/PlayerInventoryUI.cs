using UnityEngine;
using System.Collections.Generic;

namespace KStank.stanks_inventory.Example {
    /// <summary>
    /// Example script of properly displaying items to the screen.
    /// </summary>
    public sealed class PlayerInventoryUI : MonoBehaviour {
        Inventory inventory = null;
        List<Slot> slots = new List<Slot>();

        //Others
        [SerializeField]
        GameObject inventoryGm = null;
        [SerializeField]
        Transform itemHolder = null;

        void Start() {
            inventory = GetComponent<Inventory>();

            if(inventory == null)
                gameObject.AddComponent<Inventory>();

            //Load item pool
            Item.LoadItemPool();

            //Find inventory
            inventoryGm = GameObject.Find("Inventory");

            //Load inventory
            inventory.MaxItems = itemHolder.childCount;
            inventory.Load();

            //Setup the slots
            for(int i = 0; i < itemHolder.childCount; i++) {
                //Add slot to list
                slots.Add(itemHolder.GetChild(i).GetComponent<Slot>());
                Slot slot = slots[i];

                if(slot == null)
                    continue;

                //Loop through every inventory item, for every slot iteration
                for(int u = 0; u < inventory.TakenSpace; u++) {
                    Item item = inventory.GetAt(u);

                    if(item == null)
                        continue;

                    //If the item's position is equal to the current slot iteration, update it
                    if(item.Position == i)
                        slot.UpdateSlot(item);
                }
            }
        }

        void Update() {
            if(Input.GetKeyDown(KeyCode.I)) {
                if(inventoryGm == null)
                    return;

                inventoryGm.SetActive(!inventoryGm.activeInHierarchy);
                inventory.Save();
            }
        }

        void PickupInventoryItem(int id) {
            Item item = Item.LookUpItem(id);

            if(item == null)
                return;

            item.StackSize++;
            inventory.Add(item);
            
            //Go through every item in inventory
            for(int i = 0; i < inventory.TakenSpace; i++) {
                Item _item = inventory.Find(inventory.GetAt(i).ID);

                //Make sure item exists in the inventory
                if(_item == null)
                    continue;

                //If item exists, update the slot it goes to
                if(_item == item) {
                    Slot slot = slots[i];

                    if(slot.DisplayItem != null)
                        continue;

                    slot.UpdateSlot(item);
                }
            }
        }
    }
}
