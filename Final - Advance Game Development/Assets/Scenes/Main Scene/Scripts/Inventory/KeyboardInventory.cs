using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;

    void Update()
    {
        // Check if "i" key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the inventory panel
            bool isInventoryOpen = !inventoryPanel.activeSelf;
            // If inventory panel is active, then close it; otherwise, open it
            inventoryPanel.SetActive(isInventoryOpen);

            // Show/hide cursor and lock it for inventory
            if (isInventoryOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
