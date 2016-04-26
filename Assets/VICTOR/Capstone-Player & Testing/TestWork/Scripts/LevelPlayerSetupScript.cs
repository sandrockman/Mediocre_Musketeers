using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.UI;

/**
 *@author Victor Haskins
 *class LevelPlayerSetupScript 
 *
 */
public class LevelPlayerSetupScript : MonoBehaviour {
    [Tooltip("Prefab object for players")]
    public GameObject playerPrefab;
    [Tooltip("Spawn point location for Player 1. should be empty game object.")]
    public GameObject player1StartLocation;
    [Tooltip("Spawn point location for Player 2. should be empty game object.")]
    public GameObject player2StartLocation;
    [Tooltip("Empty game object used to hold and keep track of player objects.")]
    public GameObject playerContainer;

    //added sprint 3
    [Tooltip("Canvas filler for Pause Menu. Meant to be modified later.")]
    public GameObject pauseCanvas;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    void Start()
    {
        pauseCanvas.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    /// <summary>
    /// Awake function to Create the appropriate number of players depending on
    /// whether there are 1 or two players setup for play from the lobby.
    /// </summary>
	void Awake () {
        //pauseCanvas.enabled = false;
        //pauseCanvas.SetActive(false);

        switch (StaticSpawnController.GetSetPlayers())
        {
            case 0://no players added.
                Debug.Log("Entry Error at Lobby. Generic First player added.");
                //Adding the new character for testing of levels.
                CreatePlayer1();
                //CreatePlayer2();
                break;
            case 1://just the first player
                CreatePlayer1();
                break;
            case 2://just the second player added
                CreatePlayer2();
                break;
            case 3://both players added
                CreatePlayer1();
                CreatePlayer2();
                break;
            default://error with read
                Debug.Log("Error reading set players. First Player added.");
                CreatePlayer1();
                break;

        }
    }

    /// <summary>
    /// Instantiates the Player 1 prefab at the designated spawn point and sets up the
    /// movement control to the appropriate controller
    /// </summary>
    void CreatePlayer1()
    {
        GameObject player1 = (GameObject)Instantiate(playerPrefab, 
                                    player1StartLocation.transform.position, 
                                    player1StartLocation.transform.rotation);
        player1.GetComponent<XCharacterControllerLancer>().playerIndex = PlayerIndex.One;
        player1.transform.parent = playerContainer.transform;
    }

    /// <summary>
    /// Instantiates the Player 2 prefab at the designated spawn point and sets up the
    /// movement control to the appropriate controller
    /// </summary>
    void CreatePlayer2()
    {
        GameObject player2 = (GameObject)Instantiate(playerPrefab,
                                    player2StartLocation.transform.position,
                                    player2StartLocation.transform.rotation);
        player2.GetComponent<XCharacterControllerLancer>().playerIndex = PlayerIndex.Two;
        player2.transform.parent = playerContainer.transform;
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        LoseScreen.SetActive(true);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        WinScreen.SetActive(true);
    }
}
