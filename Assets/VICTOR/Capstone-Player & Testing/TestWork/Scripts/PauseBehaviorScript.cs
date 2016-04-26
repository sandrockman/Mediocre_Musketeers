using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.SceneManagement;


public class PauseBehaviorScript : MonoBehaviour {

    //public Canvas pCanvas;

    public void PauseGame()
    {
        Time.timeScale = 0;
        GameObject pCanvas = GameObject.Find("PauseCanvas") as GameObject;
        //pCanvas.enabled = true;

        pCanvas.SetActive(true);
    }

    public void _UnpauseGame()
    {
        GameObject pCanvas = GameObject.Find("PauseCanvas") as GameObject;
        //pCanvas.enabled = false;
        if(pCanvas != null)
            pCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void _QuitGame()
    {
        GameObject pCanvas = GameObject.Find("PauseCanvas") as GameObject;
        //pCanvas.enabled = false;
        if (pCanvas != null)
            pCanvas.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Quit Game.");
    }

    public void _RestartGame()
    {
        
        SceneManager.LoadScene("Level 1");
    }
}
