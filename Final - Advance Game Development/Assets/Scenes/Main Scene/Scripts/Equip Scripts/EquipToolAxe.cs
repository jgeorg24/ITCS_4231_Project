using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolAxe : MonoBehaviour
{
    [Header("Combat")] 
    public float attackRange = 3f; // Range of the axe attack
    public int damage = 10;
    public Camera cam;
    public Animator anim;
    public AudioSource axeAudioSource;

    private bool isAttacking = false; // Flag to track whether an attack is in progress

    private void Awake()
    {
        // Assign the AudioSource component
        axeAudioSource = GetComponent<AudioSource>();
    }

    public void OnAttackInput()
    {
        OnAttackStart();
        OnHit();
        OnAttackEnd();
    }

    public void OnAttackStart()
    {
        // Check if an attack is already in progress
        if (!isAttacking)
        {
            // Set the flag to indicate that an attack is in progress
            isAttacking = true;

            // Play attack animation or any other setup
            Debug.Log("Attack started");

            anim.SetTrigger("Attack");

            // Check if the axe has an AudioSource component attached
            if (axeAudioSource != null)
            {
                // Play the sound effect
                axeAudioSource.Play();
            }
        }
    }

    public void OnAttackEnd()
    {
        // Reset the flag when the attack animation ends
        isAttacking = false;
        Debug.Log("Attack ended");
    }

    public void OnHit()
    {
        // Only perform hit detection if an attack is currently in progress
        if (isAttacking)
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
}
