using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Item {
    //Private variables
    [SerializeField]
    string name;
    [SerializeField]
    Sprite icon;
    bool collected;

    //Properties
    /// <summary>
    /// Name of the Item.
    /// </summary>
    public string Name {
        get { return name; }
        set { name = value; }
    }

    /// <summary>
    /// Is the item collected or not?
    /// </summary>
    public bool Collected {
        get { return collected; }
        set { collected = value; }
    }

    /// <summary>
    /// The icon that the item will use when displayed in UI.
    /// </summary>
    public Sprite Icon {
        get { return icon; }
        set { icon = value; }
    }

    /// <summary>
    /// Object that can go inside of the inventory.
    /// </summary>
    /// <param name="name">Name of the item.</param>
    /// <param name="value">Value of the item.</param>
    public Item(string name, bool collected, Sprite icon = null) {
        Name = name;
        Collected = collected;
        Icon = icon;
    }
}
