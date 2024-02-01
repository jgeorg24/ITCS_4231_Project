using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float minXView;
    public float maxXView;
    private float camCurXRot;
    public float cameraSensitivity;

    private Vector2 currentMovementInput;
    private Vector2 mouseDelta;
    public LayerMask groundLayer;

    [Header("Look")] 
    public Transform cameraContainer;

    [HideInInspector]
    public bool canView=true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private Rigidbody rig;
    public static PlayerController instance;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        instance = this;
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        if (canView == true)
        {
            CameraView();
        }
        
    }

    private void Movement()
    {
        Vector3 move = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        move *= moveSpeed;
        move.y = rig.velocity.y;

        rig.velocity = move;

    }

    private void CameraView()
    {
        camCurXRot += mouseDelta.y * cameraSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXView, maxXView);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * cameraSensitivity, 0);

    }

    public void ViewInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {   
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {   
            currentMovementInput = Vector2.zero;
        }
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isGrounded())
            {
                rig.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
                
            }
        }
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f)+(Vector3.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)+(Vector3.up*0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f)+(Vector3.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f)+(Vector3.up*0.01f),Vector3.down)

        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canView = !toggle;
    }
}