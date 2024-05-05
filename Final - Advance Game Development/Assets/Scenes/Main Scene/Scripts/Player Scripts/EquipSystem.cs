using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EquipSystem : MonoBehaviour
{
    // 
    public static EquipSystem Instance { get; set; }
    public GameObject toolHolder;
    private PlayerController controller;
    public Axe axeTool;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<PlayerController>();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && axeTool != null && controller.canView)
        {
            axeTool.OnAttackInput();
        }
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
