using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        // Changing the scene to the game
        SceneManager.LoadScene("MainScene");
    }

    public void InstructionsLoad()
    {
        // Changing the scene to the instructions
        SceneManager.LoadScene("Instructions");
    }

    public void MainMenuLoad()
    {
        // Changing the scene to the main menu
        SceneManager.LoadScene("MenuScene");
    }
    public void QuitGame()
    {
        // Quits the game
        Application.Quit();
    }
}
