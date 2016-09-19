using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KStank.stanks_inventory {
    /// <summary>
    /// Contains a display item that is display to the screen.
    /// </summary>
    public class Slot : MonoBehaviour {
        [SerializeField]
        Item displayItem = null;
        [SerializeField]
        Image iconImage = null;
        [SerializeField]
        Text stackSizeText = null;

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

        public Text StackSizeText {
            get { return stackSizeText; }
            set { stackSizeText = value; }
        }

        void Awake() {
            if(IconImage == null) {
                IconImage = transform.GetChild(0).GetComponent<Image>();

                if(StackSizeText == null)
                    StackSizeText = IconImage.transform.GetChild(0).GetComponent<Text>();
            }

            ResetSlot();
        }

        /// <summary>
        /// Updates the item that will be displayed.
        /// </summary>
        /// <param name="icon">Item to update to.</param>
        public void UpdateItem(Item item) {
            DisplayItem = item;

            if(DisplayItem == null) {
                IconImage.sprite = null;
                StackSizeText.text = "";

                return;
            }

            IconImage.sprite = Resources.Load("Item Icons/" + DisplayItem.IconName, typeof(Sprite)) as Sprite;
            StackSizeText.text = DisplayItem.StackSize.ToString();
        }

        /// <summary>
        /// Updates the entire object, meaning it's Display Item, it's Icon to display, and it will enable it.
        /// </summary>
        /// <param name="item">Item to display.</param>
        public void UpdateSlot(Item item) {
            UpdateItem(item);
            EnableIcon();
        }

        /// <summary>
        /// This will set the Display Item and Icon to null. It will also disable the Icon.
        /// So basically, the default settings of a Slot object.
        /// </summary>
        public void ResetSlot() {
            if(IconImage == null)
                return;

            UpdateItem(null);
            DisableIcon();
        }

        /// <summary>
        /// Enable the slot's icon.
        /// </summary>
        public void EnableIcon() {
            if(IconImage == null)
                return;

            IconImage.enabled = true;
        }

        /// <summary>
        /// Disable the slot's icon.
        /// </summary>
        public void DisableIcon() {
            if(IconImage == null)
                return;

            IconImage.enabled = false;
        }
    }
}
