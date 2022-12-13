using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public EscController escController;
    public void PlayGame()
    {
        SceneManager.LoadScene("master");
    }
    public void Resume()
    {
        escController.OnEscPressed();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
