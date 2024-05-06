using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolPickaxe : EquipItem
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
        //set the ray to shoot from center of screen
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //store all the actual hit data in
        RaycastHit hit;
        //shoot raycast
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // if we hit damagable or enemy 
            if (hit.collider.GetComponent<IDamagable>() != null)
            {
                hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
            }
            
        }
    } 
}