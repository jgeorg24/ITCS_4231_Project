using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Countdown : MonoBehaviour
{
    // Public changeable variables
    public TextMeshProUGUI text;
    public float totalTime = 60f;
    public float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = totalTime;
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("YouWinPage");
        }
    }

    // Countdown
    IEnumerator StartCountdown()
    {
        while (timeRemaining > 0)
        {
            // Decreasing time
            timeRemaining -= Time.deltaTime;

            // Updating the display of the countdown
            text.text = ((int)timeRemaining).ToString();

            // Wait for the next frame
            yield return null;
        }
    }

}
