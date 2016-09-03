using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public sealed class PlayerUi : MonoBehaviour {
    //UI variables
    [SerializeField]
    GameObject playerInventoryUI;
    [SerializeField]
    GameObject itemsHolder;
    [SerializeField]
    GameObject itemPrefab;

    void Update() {
        if(playerInventoryUI == null)
            return;

        if(Input.GetKeyDown(KeyCode.I)) { //If user presses inventory button
            bool enabled = !playerInventoryUI.activeInHierarchy;

            //Show or hide inventory
            playerInventoryUI.SetActive(enabled);

            if(enabled) {
                List<Transform> children = new List<Transform>();

                //Add children to list
                for(int i = 0; i < itemsHolder.transform.childCount; i++) {
                    Transform child = itemsHolder.transform.GetChild(i);

                    if(child != null)
                        children.Add(itemsHolder.transform.GetChild(i));
                }

                for(int i = 0; i < children.Count; i++) {
                    Item item = null;
                    InventoryItemUI itemUI = children[i].GetComponent<InventoryItemUI>();

                    if(itemUI != null && item != null) {
                        itemUI.UpdateItem(item);
                        itemUI.EnableItemIcon();
                    }
                }
            }
        }
    }
}
