using UnityEngine;

public class EquipItem : MonoBehaviour
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public InventorySlot equippedSlot;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnAttackInput()
    {
        Debug.Log("Regular Attack");
    }

    public virtual void OnAltAttackInput()
    {
        Debug.Log("Alternate Attack");
    }
    
    public void Equip(Transform toolHolder)
    {
        transform.SetParent(toolHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }
}
