using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolAxe : MonoBehaviour
{
    [Header("Combat")] 
    public float attackRate;
    public float attackRange = 3f; // Range of the axe attack
    public int damage = 10;
    public Camera cam;
    public Animator anim;

    private bool attacking = false; // Flag to track whether an attack is in progress

    public void OnAttackInput()
    {
        OnAttackStart();
        OnHit();
    }

    public void OnAttackStart()
    {
        // Check if an attack is already in progress
        if (!attacking)
        {
            // Set the flag to indicate that an attack is in progress
            attacking = true;

            // Play attack animation or any other setup
            Debug.Log("Attack started");

            anim.SetTrigger("Attack");
            Invoke("OnAttackEnd",attackRate);

        }
    }

    public void OnAttackEnd()
    {
        // Reset the flag when the attack animation ends
        attacking = false;
        Debug.Log("Attack ended");
    }

    public void OnHit()
    {
        //set the ray to shoot from center of screen
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //store all the actual hit data in
        RaycastHit hit;
        //shoot raycast
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            if (hit.collider.GetComponent<IDamagable>() != null)
            {
                hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
                Debug.Log("Enemy hit");
            }
        }
    }
}
