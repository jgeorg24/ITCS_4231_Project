using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PlayerUI : MonoBehaviour, IDamagable
{
    //public Image Death;
    public Bar health;
    public Bar hunger;
    public Bar thirst;
    public float hungerHealthDecay;
    public float thirstHealthDecay;
    public static PlayerUI Instance { get; set; }

    public UnityEvent onDamage = new UnityEvent();
    // Player Health //
    private float currentHealth, maxHealth;

    // Player Hunger //
    private float currentHunger, maxHunger;

    // Player Thirst //
    private float currentThirst, maxThirst;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start()
    {   
        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;
    }
        

    void Update()
    {
        float fillValue = currentHealth / maxHealth;

        health.Subtract(health.decayRate * Time.deltaTime);
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);
        
        if (hunger.currentValue == 0.0f)
        {
            health.Subtract(hungerHealthDecay * Time.deltaTime);
        }
        
        if (thirst.currentValue == 0.0f)
        {
            health.Subtract(thirstHealthDecay * Time.deltaTime);
        }
        
        if (health.currentValue == 0.0f)
        {
            Die();
        }

        health.uiBar.value = health.GetPercentage();
        hunger.uiBar.value = hunger.GetPercentage();
        thirst.uiBar.value = thirst.GetPercentage();

        health.counter.text = (int)health.currentValue + "/" + health.maxValue;
        hunger.counter.text = (int)hunger.currentValue + "/" + hunger.maxValue;
        thirst.counter.text = (int)thirst.currentValue + "/" + thirst.maxValue;

    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }
    
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        thirst.Add(amount);
    }

    public void Die()
    {
        //Death.GetComponent<Animation>().Play("You Are Dead");
        //Debug.Log("Player has died");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //SceneManager.LoadScene("Menu");
        
    }
	
    // NEEDS WORK
	public void TakeDamage(int amount)
	{
		health.Subtract(amount);
        Debug.Log("Passed subtract " + amount);
        //onDamage?.Invoke();
        Debug.Log("Passed OnDamage");
    }
}

[System.Serializable]
public class Bar
{   
    [HideInInspector]
    public float currentValue;
    public float maxValue;
    public float startValue;
    public float decayRate;
    public Slider uiBar;
    public TextMeshProUGUI counter;


    public void Add(float amount)
    {
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        currentValue = Mathf.Max(currentValue - amount, 0);
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }
}

public interface IDamagable
{
    void TakeDamage(int damageAmount);
}