using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KStank.stanks_inventory {
    public class Slot : MonoBehaviour {
        Item displayItem = null;
        Image iconImage = null;

        /// <summary>
        /// The Item that will be displayed to the screen.
        /// </summary>
        public Item DisplayItem {
            get { return displayItem; }
            set { displayItem = value; }
        }

        /// <summary>
        /// Image that is used to display item's icon to the screen.
        /// </summary>
        public Image IconImage {
            get { return iconImage; }
            set { iconImage = value; }
        }

        void Awake() {
            iconImage = transform.GetChild(0).GetComponent<Image>();
            DisableIcon();
        }

        /// <summary>
        /// Updates the slot to the item.
        /// </summary>
        /// <param name="icon">Item to update slot to.</param>
        public void UpdateSlot(Item item) {
            if(iconImage == null || item == null)
                return;

            DisplayItem = item;
            iconImage.overrideSprite = DisplayItem.Icon;
        }

        /// <summary>
        /// Enable the slot's icon.
        /// </summary>
        public void EnableIcon() {
            if(iconImage == null)
                return;

            iconImage.enabled = true;
        }

        /// <summary>
        /// Disable the slot's icon.
        /// </summary>
        public void DisableIcon() {
            if(iconImage == null)
                return;

            iconImage.enabled = false;
        }
    }
}
