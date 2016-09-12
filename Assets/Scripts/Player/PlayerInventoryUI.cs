using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace KStank.stanks_inventory {
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

        void PickupInventoryItem(Item item) {
            inventory.Pickup(item);
            
            //Go through every item in inventory
            for(int i = 0; i < inventory.TakenSpace; i++) {
                Item _item = inventory.Find(inventory.GetAt(i).Name);

                //Make sure item exists in the inventory
                if(_item == null)
                    continue;

                if(_item == item) {
                    Slot slot = slots[i];

                    slot.UpdateSlot(item);
                }
            }

            inventory.Save();
        }
    }
}
