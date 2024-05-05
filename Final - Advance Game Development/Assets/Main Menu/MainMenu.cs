using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); //Change "SampleScence" to the game scence
    }

    public void InstructionsLoad()
    {
        SceneManager.LoadScene("Instructions"); //Make Instructions Scene
    }

    public void MainMenuLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
}
