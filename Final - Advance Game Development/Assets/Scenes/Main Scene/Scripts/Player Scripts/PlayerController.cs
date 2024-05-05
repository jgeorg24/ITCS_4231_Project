using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed;
    private Vector2 currentMovementInput;
    public float jumpForce;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public bool isGrounded;

    [Header("Player View")] 
    public Transform cameraContainer;
    public float minXView;
    public float maxXView;
    private float camXRot;
    public float cameraSensitivity;

    [Header("Fall Damage")] 
    public float fallDamageThreshold = 1;
    public float damageMultiplier = 5;
    
    public Camera cam;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canView = true;
    public float speed;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rig = GetComponent<Rigidbody>();

    }

    private Rigidbody rig;
    private PlayerUI playerUI;

    public static PlayerController Instance;
    private void Awake()
    {
        playerUI = PlayerUI.Instance;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded == true)
        {
            //anim.SetBool("isGrounded", true);
        }

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
        speed = Vector3.Magnitude(rig.velocity);

    }

    private void CameraView()
    {
        camXRot += mouseDelta.y * cameraSensitivity;
        camXRot = Mathf.Clamp(camXRot, minXView, maxXView);
        cameraContainer.localEulerAngles = new Vector3(-camXRot, 0, 0);
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
        if (context.phase == InputActionPhase.Performed)
        {

            if (isGrounded == true)
            {
                //anim.SetTrigger("Jump");
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            float fallVelocity = Mathf.Abs(transform.position.y - collision.contacts[0].point.y);
            float calculatedDamage = (fallVelocity - fallDamageThreshold) * damageMultiplier;

            if (calculatedDamage > 0)
            {
                ApplyFallDamage(calculatedDamage);
            }
        }
    }

    void ApplyFallDamage(float damage)
    {
        playerUI.TakeDamage((int)damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f),Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f),Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f),Vector3.down);
        
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canView = !toggle;
    }
}