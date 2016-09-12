using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KStank.stanks_inventory {
    [System.Serializable]
    public class Item {
        //Private variables
        [SerializeField]
        string name = "";
        [SerializeField]
        Sprite icon = null;
        string iconName = "";
        int pos = 0;
        bool collected = false;

        //Properties
        /// <summary>
        /// Name of the Item.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The name of the icon that will be displayed.
        /// </summary>
        public string IconName {
            get {
                if(icon != null)
                    return icon.name;

                return iconName;
            }
            set { iconName = value; }
        }

        public int Position {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// Is the item collected or not?
        /// </summary>
        public bool Collected {
            get { return collected; }
            set { collected = value; }
        }

        /// <summary>
        /// Object that can go inside of the inventory.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="position">Position of the item.</param>
        public Item(string name, int position) {
            Name = name;
            Position = position;
        }
    }
}
