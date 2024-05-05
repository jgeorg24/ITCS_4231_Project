using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour, IDamagable
{
    public EnemyBar health;
    public Camera mainCamera;
    public Transform target; // Reference to the enemy's transform

    private void Start()
    {
        health.currentValue = health.startValue;
    }

    void Update()
    {
        health.uiBar.value = health.GetPercentage();
        health.counter.text = ((int)health.currentValue).ToString() + "/" + health.maxValue.ToString();

        // Ensure the target (enemy's transform) is not null and the main camera is not null
        if (target != null && mainCamera != null)
        {
            // Calculate the direction from the health bar to the camera
            Vector3 toCameraDirection = mainCamera.transform.position - transform.position;

            // Face the health bar towards the camera
            transform.rotation = Quaternion.LookRotation(toCameraDirection, Vector3.up);

            // Optionally, align the health bar with the enemy's orientation
            transform.rotation *= target.rotation;
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
