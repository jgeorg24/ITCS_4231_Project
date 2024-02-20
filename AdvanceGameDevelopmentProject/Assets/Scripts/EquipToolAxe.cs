using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolAxe : Equip
{
    public float attackRate;
    public float attackdistance;
    private bool attack;

    [Header("Combat")] 
    public bool doesDealDamage;
    public int damage;

    [Header("Resource Gathering")]
    public bool doesGatherresources;

    private Animator anim;
    private Camera cam;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attack)
        {
            attack = true;
            anim.SetTrigger("Attack");
            Invoke("OnCanAttack",attackRate);
        }
    }
    void OnCanAttack()
    {
        attack = false;
    }

    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackdistance))
        {
            if (doesDealDamage && hit.collider.GetComponent<IDamagable>() != null)
            {
                hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
            }
        }
    }

}