using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    // When we drop something onto this slot, this is what happens.
    public void OnDrop(PointerEventData eventData){
        // If this slot is empty...
        if (transform.childCount == 0){
            // Get the item we dropped onto this slot.
            GameObject dropped = eventData.pointerDrag;
            
            // Check if it's an inventory item.
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>(); 
            
            // If it's an inventory item, we remember this slot as its new home.
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
