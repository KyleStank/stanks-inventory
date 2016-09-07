using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory {
    public sealed class ItemObject : MonoBehaviour {
        //Private variables
        [SerializeField]
        Item item = null;

        void OnTriggerEnter(Collider col) {
            if(col.tag == "Player") {
                //Invoke Inventory.Pickup() method
                col.SendMessage("PickupInventoryItem", item, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
