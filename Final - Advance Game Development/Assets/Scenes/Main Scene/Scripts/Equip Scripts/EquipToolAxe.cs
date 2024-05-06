using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolAxe : EquipItem
{
    public float attackRate;
    public float attackRange;
    private bool attacking;

    [Header("Combat")] 
    public int damage;

    //components
    public Animator anim;
    public Camera cam;

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            anim.SetTrigger("Attack");
            Invoke("OnCanAttack",attackRate);
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        // Get the position of the weapon or player's weapon point
        Vector3 weaponPoint = transform.position; // You may need to adjust this based on your setup

        // Set the ray to shoot from the weapon point towards the center of the screen
        Ray ray = new Ray(weaponPoint, cam.transform.forward);

        // Store the hit data
        RaycastHit hit;

        // Shoot the raycast
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // Check if we hit a damagable object or enemy
            if (hit.collider.GetComponent<IDamagable>() != null)
            {
                hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
            }
        }
    }
}
