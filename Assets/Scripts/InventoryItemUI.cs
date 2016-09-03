using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItemUI : MonoBehaviour {
    //Static variables
    public static List<Transform> items = new List<Transform>();

    public Transform itemsHolder;

    //UI variables
    Image backgroundImage;
    Image foregroundImage;

    [HideInInspector]
    public Item item;
    [HideInInspector]
    public bool hasItem;

    void Awake() {
        backgroundImage = GetComponent<Image>();
        foregroundImage = GetComponentInChildren<Image>();

        //Get correct image. Foreground image MUST be first child for this to work
        if(foregroundImage.GetHashCode() == backgroundImage.GetHashCode()) {
            Transform child = transform.GetChild(0);
            foregroundImage = child.GetComponent<Image>();
        }
    }

    void Start() {
        //Add this transform if it doesn't already exist
        if(!items.Contains(transform))
            items.Add(transform);

        if(foregroundImage == null)
            return;

        //Disable because there is no item set yet
        DisableItemIcon();
    }

    void UpdateSprite(Sprite src) {
        if(foregroundImage == null)
            return;

        foregroundImage.sprite = src;
    }

    /// <summary>
    /// Enables the item icon.
    /// </summary>
    public void EnableItemIcon() {
        if(foregroundImage == null)
            return;

        foregroundImage.enabled = true;
    }

    /// <summary>
    /// Disables the item icon
    /// </summary>
    public void DisableItemIcon() {
        if(foregroundImage == null)
            return;

        foregroundImage.enabled = false;
    }

    /// <summary>
    /// Sets a new item, and updates the icon that is displayed.
    /// </summary>
    /// <param name="item">New item to be set.</param>
    public void UpdateItem(Item item) {
        if(foregroundImage == null)
            return;

        if(item == null) {
            this.item = null;
            UpdateSprite(null);
            return;
        }

        this.item = item;
        UpdateSprite(item.Icon);
    }
}
