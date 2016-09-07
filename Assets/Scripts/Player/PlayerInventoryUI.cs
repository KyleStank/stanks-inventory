using UnityEngine;
using System.Collections.Generic;

namespace KStank.stanks_inventory {
    public sealed class PlayerInventoryUI : MonoBehaviour {
        Inventory inventory;
        List<Slot> slots = new List<Slot>();

        //Temp vars
        [SerializeField]
        int maxItems = 4;

        //Others
        public Transform itemHolder;

        void Awake() {
            inventory = GetComponent<Inventory>();

            if(inventory == null)
                gameObject.AddComponent<Inventory>();

            //Load inventory
            if(inventory.MaxItems < itemHolder.childCount || maxItems < itemHolder.childCount)
                inventory.MaxItems = itemHolder.childCount;
            else
                inventory.MaxItems = maxItems;

            //Setup the slots
            for(int i = 0; i < itemHolder.childCount; i++) {
                slots.Add(itemHolder.GetChild(i).GetComponent<Slot>());
                Slot slot = slots[i];

                if(slot == null)
                    continue;
                
                slot.DisplayItem = inventory.GetAt(i);
            }
        }

        public void PickupInventoryItem(Item item) {
            inventory.Pickup(item);

            for(int i = 0; i < inventory.TakenSpace; i++) {
                Item _item = inventory.GetAt(i);

                if(_item.GetHashCode() == item.GetHashCode()) {
                    Slot slot = slots[i];

                    slot.UpdateSlot(item);
                    slot.EnableIcon();
                }
            }
        }
    }
}
