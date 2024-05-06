using UnityEngine;

public class WeaponFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's transform
    public Vector3 offset; // Offset between the camera and the weapon

    void Update()
    {
        // Match the weapon's position to the camera's position with an offset
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(offset);

        // Match the weapon's rotation to the camera's rotation
        transform.rotation = cameraTransform.rotation;
    }
}
