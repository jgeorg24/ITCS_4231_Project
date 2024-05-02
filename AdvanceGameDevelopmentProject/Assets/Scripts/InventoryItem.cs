using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image; // This keeps track of the item's picture.

    [HideInInspector] public Transform parentAfterDrag; // This remembers where the item should go back after being dragged.

    // When we start dragging the item, this happens.
    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log("Begin Drag");
        
        // Remember where the item was before we moved it.
        parentAfterDrag = transform.parent;
        
        // We move the item to the top layer so it doesn't hide behind other stuff.
        transform.SetParent(transform.root);
        
        // Stop the item from blocking clicks on things behind it.
        image.raycastTarget = false;
    }

    // As we're dragging the item around, this keeps happening.
    public void OnDrag(PointerEventData eventData){
        Debug.Log("Dragging");
        
        // Move the item to wherever the mouse pointer is.
        transform.position = Input.mousePosition;
    }

    // When we stop dragging the item, this gets called.
    public void OnEndDrag(PointerEventData eventData){
        Debug.Log("End Drag");
        
        // Move the item back to where it was before we moved it.
        transform.SetParent(parentAfterDrag);
        
        // Let the item block clicks again.
        image.raycastTarget = true;
    }
}
