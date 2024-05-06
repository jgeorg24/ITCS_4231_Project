using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePickup : MonoBehaviour, Interactable
{
    // Public changeable variables
    public TextMeshProUGUI text;

    // Private unchangeable variables
    private int currentAmount = 0;

    public void Interact()
    {
        // Every time the 'E' button is hit and the player is in range it'll increment the resource counter
        currentAmount++;

        text.text = currentAmount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
