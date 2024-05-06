using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactable
{
    // Allows script to be called from different places
    public void Interact();
}

public class Interact : MonoBehaviour
{
    // Public changeable variables
    public Transform interactSource;
    public float interactRange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checking if the key 'E' is hit
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Cast a new ray out to deem if in front of the chracter is the object of interaction if so do this
            Ray r = new Ray(interactSource.position, interactSource.forward);

            if (Physics.Raycast(r, out RaycastHit hit, interactRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out Interactable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
