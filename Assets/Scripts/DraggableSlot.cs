using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace KStank.stanks_inventory {
    public class DraggableSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler {
        Slot newSlot = null;
        Vector3 originalPos = new Vector3(0.0f, 0.0f, 0.0f);
        int previousIndex = 0;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;

            //Get original position and sibling index
            originalPos = IconImage.rectTransform.position;
            previousIndex = transform.GetSiblingIndex();

            //Set Slot above every other slot so it is rendered on top of them
            transform.SetAsLastSibling();
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;

            //Make Slot follow cursor
            IconImage.rectTransform.position = Input.mousePosition;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
            if(DisplayItem == null)
                return;

            //Set Slot back to original position
            IconImage.rectTransform.position = originalPos;
            transform.SetSiblingIndex(previousIndex);
            
            Transform parent = transform.parent;
            Slot[] slots = new Slot[parent.childCount];

            for(int i = 0; i < slots.Length; i++) {
                Transform slotTransform = parent.GetChild(i);
                Slot slot = slotTransform.GetComponent<Slot>();
                
                float width = slot.IconImage.rectTransform.rect.width;
                float height = slot.IconImage.rectTransform.rect.height;

                Vector2 slotPos = slot.transform.position;

                float distance = Vector2.Distance(eventData.position, slotPos);
                
                if(distance < width) {
                    transform.SetSiblingIndex(i);
                    IconImage.rectTransform.position = slot.IconImage.rectTransform.position;
                }
            }
        }
    }
}
