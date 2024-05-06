using UnityEngine;
using UnityEngine.InputSystem;

public class EquipSystem : MonoBehaviour
{
    public EquipItem currentEquip; // Reference to the currently equipped weapon
    public Transform toolHolder; // Reference to the tool holder object containing weapon prefabs
    private GameObject[] weapons; // Array to hold references to weapon prefabs

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        // Deactivate all weapons initially
        DeactivateAllWeapons();

        // Initialize weapons array with references to child objects of the tool holder
        int numWeapons = toolHolder.childCount;
        weapons = new GameObject[numWeapons];
        for (int i = 0; i < numWeapons; i++)
        {
            weapons[i] = toolHolder.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        // Check for input to activate weapons in slots 1, 2, and 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateWeapon(0); // Activate weapon in slot 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateWeapon(1); // Activate weapon in slot 2
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateWeapon(2); // Activate weapon in slot 3
        }
    }

    // Handle attack input using InputAction.CallbackContext
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentEquip != null && controller.canView)
        {
            currentEquip.OnAttackInput();
        }
    }

    // Handle alternate attack input using InputAction.CallbackContext
    public void OnAltAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentEquip != null && controller.canView)
        {
            currentEquip.OnAltAttackInput();
        }
    }

    private void ActivateWeapon(int index)
    {
        // Deactivate all weapons first
        DeactivateAllWeapons();

        // Activate the weapon at the specified index
        if (index >= 0 && index < weapons.Length)
        {
            GameObject weapon = weapons[index];
            if (weapon != null)
            {
                weapon.SetActive(true);
                currentEquip = weapon.GetComponent<EquipItem>();
            }
        }
    }

    private void DeactivateAllWeapons()
    {
        // Deactivate all weapons
        if (weapons != null)
        {
            foreach (GameObject weapon in weapons)
            {
                if (weapon != null)
                {
                    weapon.SetActive(false);
                }
            }
        }
    }
}
