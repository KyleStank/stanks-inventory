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
                slots.Add(itemHolder.GetChild(i).GetComponent<Slot>());

                Slot slot = slots[i];
                Item item = inventory.GetAt(i);
                
                if(slot == null || item == null)
                    continue;

                slot.UpdateSlot(item);
            }
        }

        void Update() {
            if(Input.GetKeyDown(KeyCode.I)) {
                if(inventoryGm == null)
                    return;

                inventoryGm.SetActive(!inventoryGm.activeInHierarchy);
            }
        }

        void PickupInventoryItem(Item item) {
            inventory.Pickup(item);
            
            //Go through every item in inventory
            for(int i = 0; i < inventory.TakenSpace; i++) {
                Item _item = inventory.GetAt(i);

                //Make sure that item exists by checking the object's hash code
                if(_item.GetHashCode() == item.GetHashCode()) {
                    Slot slot = slots[i]; //THIS IS CAUSING THE ERROR

                    //Set the slot
                    slot.UpdateSlot(item);
                }
            }

            inventory.Save();
        }
    }
}
