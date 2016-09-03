using UnityEngine;
using System.Collections;

public sealed class ItemObject : MonoBehaviour {
    //Private variables
    [SerializeField]
    Item item;

    [SerializeField]
    float destroyTime;

    void OnTriggerEnter(Collider col) {
        //Check for null not needed because it is already built into the Add() method
        Player.Inventory.Pickup(item);

        //Not needed, but heck, why not. Only destroy if the item is actually there
        if(item != null)
            Destroy(gameObject, destroyTime);
    }
}
