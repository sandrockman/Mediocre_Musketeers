using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 *@author Victor Haskins
 *class MenuButtonsScript setup for both static and non-static functions
 *to load new scenes or quit the game.
 */
public class MenuButtonsScript : MonoBehaviour {
    /// <summary>
    ///loads the main menu scene
    /// </summary>
    public static void _MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    /// <summary>
    /// loads the lobby menu
    /// </summary>
    public static void _LobbyMenu()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    /// <summary>
    /// loads the Options Menu
    /// </summary>
    public static void _OptionsMenu()
    {
        SceneManager.LoadScene("OptionsScene");
    }
    /// <summary>
    /// loads the Credits Menu
    /// </summary>
    public static void _CreditsMenu()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    /// <summary>
    /// Loads the First Level
    /// </summary>
    public static void _Level1Scene()
    {
        if (StaticSpawnController.GetSetPlayers() != 0)
        {
            Debug.Log("Starting Level...");
            SceneManager.LoadScene("Level 1");
        }
        else
            Debug.Log("Needs at least one player entered.");
    }
    /// <summary>
    /// Quits the game
    /// </summary>
    public static void _QuitGame()
    {
        Debug.Log("Game has been quit.");
        Application.Quit();
    }
    /// <summary>
    /// non-static method to call the main menu
    /// </summary>
    public void _NonStaticMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    /// <summary>
    /// non-static method to call the Lobby Scene
    /// </summary>
    public void _NonStaticLobbyMenu()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    /// <summary>
    /// non-static method to call the Options Menu
    /// </summary>
    public void _NonStaticOptionsMenu()
    {
        SceneManager.LoadScene("OptionsScene");
    }
    /// <summary>
    /// non-static method to call the Credits menu
    /// </summary>
    public void _NonStaticCreditsMenu()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    /// <summary>
    /// non-static method to Quit the game
    /// </summary>
    public void _NonStaticQuitGame()
    {
        Debug.Log("Game has been quit.");
        Application.Quit();
    }
    /// <summary>
    /// non-static method to call the first level of the game
    /// </summary>
    public void _NonStaticLevel1Scene()
    {
        if (StaticSpawnController.GetSetPlayers() != 0)
        {
            Debug.Log("Starting Level...");
            SceneManager.LoadScene("test_level_FINAL");
        }
        else
            Debug.Log("Needs at least one player entered.");
    }
}
