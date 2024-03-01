using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolAxe : MonoBehaviour
{
    public float attackRate;
    public float attackdistance;
    private bool attack;

    [Header("Combat")] 
    public bool doesDealDamage;
    public int damage;
    public Animator anim;
    public Camera cam;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAttackInput()
    {
        anim.SetTrigger("Attack");
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