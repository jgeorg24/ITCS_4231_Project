using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour, IDamagable
{
    public EnemyBar health;
    public Camera mainCamera;

    private void Start()
    {
        health.currentValue = health.startValue;
    }

    void Update()
    {
        health.uiBar.value = health.GetPercentage();
        health.counter.text = ((int)health.currentValue).ToString() + "/" + health.maxValue.ToString();

        // Billboard the health bar towards the camera
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        }
    }

    // Implement the TakeDamage method from the IDamagable interface
    public void TakeDamage(int amount)
    {
        health.Subtract(amount);
    }
}

[System.Serializable]
public class EnemyBar
{
    [HideInInspector]
    public float currentValue;
    public float maxValue;
    public float startValue;
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
