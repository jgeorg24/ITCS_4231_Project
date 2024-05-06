using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to your inventory panel
    public Camera mainCamera; // Reference to your main camera

    void Start()
    {
        // If mainCamera is not assigned, try to find the main camera in the scene
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Check if the "I" key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // If inventory panel is active, then close it; otherwise, open it
            bool isInventoryOpen = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isInventoryOpen);
            
            // Freeze/unfreeze camera movement
            FreezeCameraMovement(isInventoryOpen);

            // Show/hide cursor and lock it
            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    void FreezeCameraMovement(bool freeze)
    {
        if (mainCamera != null)
        {
            // Disable or enable the script that controls camera movement
            mainCamera.GetComponent<Camera>().enabled = !freeze;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned or found in the scene.");
        }
    }
}
