using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>(); 
            inventoryItem.parentAfterDrag = transform; // Access parentAfterDrag through an instance of InventoryItem
            //equipItem.equippedSlot = this; // Remember the slot where the item is equipped
        }
    }
}
