using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //public Image Death;
    public Bar health;
    public Bar hunger;
    public Bar thirst;
    public float hungerHealthDecay;
    public float thirstHealthDecay;
    public static PlayerUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;

    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.decayRate*Time.deltaTime);
        thirst.Subtract(thirst.decayRate*Time.deltaTime);
        
        if (hunger.currentValue == 0.0f)
        {
            health.Subtract(hungerHealthDecay * Time.deltaTime);
        }
        
        if (thirst.currentValue == 0.0f)
        {
            health.Subtract(hungerHealthDecay * Time.deltaTime);
        }
        
        if (health.currentValue == 0.0f)
        {
            Die();
        }
        
        //health.uiBar.fillAmount = health.GetPercentage();
        //hunger.uiBar.fillAmount = hunger.GetPercentage();
        //thirst.uiBar.fillAmount = thirst.GetPercentage();
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //SceneManager.LoadScene("Menu");
        
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
    //public Image uiBar;

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