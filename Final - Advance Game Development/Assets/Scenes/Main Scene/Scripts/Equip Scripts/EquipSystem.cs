using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance {get; set;}
    public GameObject toolHolder;
    private PlayerController controller;
    public EquipToolAxe currentEquip;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<PlayerController>();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentEquip != null && controller.canView)
        {
            currentEquip.OnAttackInput();
        }
    }
    /*
    public void OnAltAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentEquip != null && controller.canView)
        {
            currentEquip.OnAltAttackInput();
        }
    }*/
}