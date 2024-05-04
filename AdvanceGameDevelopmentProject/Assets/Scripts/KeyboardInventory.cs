using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to your inventory panel

    void Update()
    {
        // Check if the "I" key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // If inventory panel is active, then close it; otherwise, open it
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}

