using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace KStank.stanks_inventory {
    public class DraggableSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler {
        Transform parent = null;
        GameObject draggingGo = null;
        int previousIndex = 0;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;
            
            parent = transform.parent;

            //Create a temporay game object for the user to drag around
            CreateDragObject("Dragged Object");

            //Get original sibling index before it changes
            previousIndex = transform.GetSiblingIndex();

            //Set slot above every other slot so it is rendered on top of them
            transform.SetAsLastSibling();
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;

            //Make the icon follow the cursor
            draggingGo.transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;

            DestroyDraggedObject(); //Destroy the dragged object

            //Set slot back to original position
            transform.SetSiblingIndex(previousIndex);
            
            //Get the items holder
            Slot[] slots = new Slot[parent.childCount];
            
            for(int i = 0; i < slots.Length; i++) {
                //Get the slot
                Transform slotTransform = parent.GetChild(i);
                Slot slot = slotTransform.GetComponent<Slot>();
                
                //Get slot info
                float width = slot.IconImage.rectTransform.rect.width;
                Vector2 slotPos = slot.transform.position;

                //Get the distance from the slot icon to the items, then set the new position
                float distance = Vector2.Distance(eventData.position, slotPos);
                if(distance <= width) {
                    slot.UpdateSlot(DisplayItem);
                    RuinSlot();
                }
            }
        }

        void OnDisable() {
            DestroyDraggedObject();
        }

        void CreateDragObject(string name) {
            //Create the temporary game object that will be dragged by the user
            draggingGo = new GameObject(name);
            draggingGo.transform.SetParent(transform);
            draggingGo.AddComponent<Image>();

            //Set the game object's sprite to this sprite
            draggingGo.GetComponent<Image>().sprite = IconImage.sprite;
        }

        //Destroys the temporary object that gets dragged
        void DestroyDraggedObject() {
            if(draggingGo == null)
                return;

            Destroy(draggingGo);
        }
    }
}
