using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory.Example {
    /// <summary>
    /// Example script that adds an item to the inventory when player touches it.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class ItemObject : MonoBehaviour {
        //Private variables
        [SerializeField]
        int itemId = 0;

        void OnTriggerEnter(Collider col) {
            if(col.tag == "Player") //Invoke the pick up method in the PlayerInventoryUI class
                col.SendMessage("PickupInventoryItem", itemId, SendMessageOptions.DontRequireReceiver);
        }
    }
}
