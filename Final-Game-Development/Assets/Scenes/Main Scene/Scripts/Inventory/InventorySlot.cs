using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            EquipItem equipItem = dropped.GetComponent<EquipItem>(); 
            equipItem.parentAfterDrag = transform;
            equipItem.equippedSlot = this; // Remember the slot where the item is equipped
        }
    }
}
