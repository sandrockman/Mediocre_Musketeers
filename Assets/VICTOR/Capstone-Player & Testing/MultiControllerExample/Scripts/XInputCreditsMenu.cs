using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.UI;

public class XInputCreditsMenu : MonoBehaviour {
    [Tooltip("Player index to show what controller it is tied to.")]
    public PlayerIndex playerIndex;
    #region OldVariables
    //[Tooltip("current prefab of the player")]
    //public GameObject playerPrefab;
    //public GameObject playerInstance;
    //public GUIText text;
    /*
    [Tooltip("Button to start the level.")]
    public Button StartGameButton;
    [Tooltip("Button to move to the Options screen.")]
    public Button OptionsGameButton;
    //*/
    /*
    [Tooltip("Button to Quit the Game.")]
    public Button QuitGameButton;

    [Tooltip("Highlight image for start button")]
    public Image StartHighlight;
    [Tooltip("Highlight image for options button")]
    public Image OptionsHighlight;
    //*/
    /*
    [Tooltip("Highlight image for quit button")]
    public Image QuitHighlight;
    //*/
    #endregion
    [Tooltip("Button to move back to the Main Menu")]
    public Button BackGameButton;
    [Tooltip("Highlight image for back button")]
    public Image BackHighlight;
    [Tooltip("Static button object that is set to an appropriate object by the script.")]
    public static Button toSelect = null;
    //previous artifacts that can be used to gauge time since last controller button press
    uint lastPacketNumber;
    public float lastPacketTime;
    //object to determine current state of buttons
    GamePadState currentState;
    //object to determine the last update's state of buttons
    GamePadState previousState;

    /// <summary>
    /// starts with setting all highlights to false and sets the player to inactive.
    /// </summary>
    void Start()
    {
        toSelect = BackGameButton;
        BackHighlight.enabled = true;
        #region OldStart
        /*
        StartHighlight.enabled = true;
        OptionsHighlight.enabled = false;
        //*/
        /*
        QuitHighlight.enabled = false;
        //*/
        //StaticSpawnController.ActivatePlayer(playerIndex, false);
        #endregion
    }

    /// <summary>
    /// reads controller inputs
    /// </summary>
    void Update()
    {
        //grab the current state
        currentState = GamePad.GetState(playerIndex);
        //if start button was just pressed.
        if (currentState.Buttons.Start == ButtonState.Pressed && previousState.Buttons.Start == ButtonState.Released ||
            currentState.Buttons.A == ButtonState.Pressed && previousState.Buttons.A == ButtonState.Released)
        {
            //call activate button.
            Debug.Log("Start Button Pressed.");
            ActivateButton();
        }
        #region OldUpdate1
        /*
        //check to see if the player pushed the A button
        if (currentState.Buttons.A == ButtonState.Pressed)
        {
            //set current player as active.
            int activateCheck = StaticSpawnController.GetSetPlayers();
            StaticSpawnController.ActivatePlayer(playerIndex, true);

            //if is player 1 and not active or is player 2 and not active
            if ((playerIndex == PlayerIndex.One && activateCheck != 1 && activateCheck != 3) ||
                (playerIndex == PlayerIndex.Two && activateCheck != 2 && activateCheck != 3))
            {
                //set the toSelect button so start button presses can now be used
                toSelect = StartGameButton;
                //set start hi
                StartHighlight.enabled = true;
            }
        }
        //*/
        #endregion
        //NOTE:  Doesn't work with some XInput emulated device drivers like the popular PS3 Controller one
        //destroy the player instance if the controller disconnects
        if (currentState.IsConnected == false)
        {
            #region OldUpdate2
            /*
            
            //if player controller disconnects at the lobby, remove them from the active script.
            StaticSpawnController.ActivatePlayer(playerIndex, false);
            if (StaticSpawnController.GetSetPlayers() == 0)
            {
                toSelect = null;
                StartHighlight.enabled = false;
            }
            //text.enabled = true;
            //*/
            #endregion
            return;
        }
        else
        {
            #region OldUpdate3
            /*
            //call action selection
            ActionSelection();
            //*/

            /*
            //remove the player call if the player pushed the Back button
            if (currentState.Buttons.Back == ButtonState.Pressed)
            {
                //check for players
                int removeCheck = StaticSpawnController.GetSetPlayers();
                //if appropriate player is activated when they hit the back button,
                if ((playerIndex == PlayerIndex.One && removeCheck != 0 && removeCheck != 2) ||
                    (playerIndex == PlayerIndex.Two && removeCheck != 0 && removeCheck != 1))
                {
                    //set that player to false
                    StaticSpawnController.ActivatePlayer(playerIndex, false);
                    //text.enabled = true;
                    //set screen to null the toSelect button and highlighters if there
                    //are no active players
                    if (StaticSpawnController.GetSetPlayers() == 0)
                    {
                        toSelect = null;
                        StartHighlight.enabled = false;
                    }
                    return;
                }

            }
            //*/
            #endregion
            //update packet numbers
            if (currentState.PacketNumber > lastPacketNumber)
            {
                lastPacketNumber = currentState.PacketNumber;
                lastPacketTime = Time.time;
            }
            else
            {
                //NOTE:  Doesn't work with some XInput emulated device drivers like the popular PS3 Controller one
                if (Time.time - lastPacketTime > 10)
                {
                    //controller has been idle for 10 seconds
                }
            }

        }
        //Set the previous state to the current state and run the set Highlighter.
        previousState = currentState;
        SetHighlighter();
    }

    /// <summary>
    /// Given the toSelect button object above, the appropriate highlight will run.
    /// </summary>
    void SetHighlighter()
    {
        BackHighlight.enabled = true;
        #region Oldhighlight
        /*
        //if null, deactivate all highlighters
        if (toSelect == null)
        {
            StartHighlight.enabled = false;
            OptionsHighlight.enabled = false;
            BackHighlight.enabled = false;
            QuitHighlight.enabled = false;
        }//activate only the start highlighter
        else if (toSelect == StartGameButton)
        {
            StartHighlight.enabled = true;
            OptionsHighlight.enabled = false;
            BackHighlight.enabled = false;
            QuitHighlight.enabled = false;
        }//activate only the options highlighter
        else if (toSelect == OptionsGameButton)
        {
            StartHighlight.enabled = false;
            OptionsHighlight.enabled = true;
            BackHighlight.enabled = false;
            QuitHighlight.enabled = false;
        }//activate only the back highlighter
        else if (toSelect == BackGameButton)
        {
            StartHighlight.enabled = false;
            OptionsHighlight.enabled = false;
            BackHighlight.enabled = true;
            QuitHighlight.enabled = false;
        }//activate only the quit highlighter
        else if (toSelect == QuitGameButton)
        {
            StartHighlight.enabled = false;
            OptionsHighlight.enabled = false;
            BackHighlight.enabled = false;
            QuitHighlight.enabled = true;
        }
        //*/
        #endregion
    }

    /// <summary>
    /// State machine to change the button selection based on the old
    /// </summary>
    void ActionSelection()
    {
        /*
        //move from top button to lower set of buttons
        if (currentState.DPad.Down == ButtonState.Pressed && previousState.DPad.Down == ButtonState.Released)
        {
            if (toSelect == StartGameButton)
            {
                toSelect = OptionsGameButton;
            }
            else if (toSelect == OptionsGameButton)
            {
                toSelect = BackGameButton;
            }
            else if (toSelect == BackGameButton)
            {
                toSelect = QuitGameButton;
            }
            else if (toSelect == QuitGameButton)
            {
                toSelect = StartGameButton;
            }
        }
        //*/
        /*
        //move left one button if able
        else if (currentState.DPad.Left == ButtonState.Pressed && previousState.DPad.Left == ButtonState.Released)
        {
            //from quit button to options button
            if (toSelect == QuitGameButton)
            {
                toSelect = OptionsGameButton;
            }//from back button to quit button
            else if (toSelect == CreditsGameButton)
            {
                toSelect = QuitGameButton;
            }
        }
        //*/
        /*
        //move right one button if able
        else if (currentState.DPad.Right == ButtonState.Pressed && previousState.DPad.Right == ButtonState.Released)
        {
            if (toSelect == QuitGameButton)
            {
                toSelect = CreditsGameButton;
            }
            else if (toSelect == OptionsGameButton)
            {
                toSelect = QuitGameButton;
            }
        }
        //*/
        /*
        //move up from the bottom row of buttons to the start button
        else if (currentState.DPad.Up == ButtonState.Pressed && previousState.DPad.Up == ButtonState.Released)
        {
            if (toSelect == StartGameButton)
            {
                toSelect = QuitGameButton;
            }
            else if (toSelect == OptionsGameButton)
            {
                toSelect = StartGameButton;
            }
            else if (toSelect == BackGameButton)
            {
                toSelect = OptionsGameButton;
            }
            else if (toSelect == QuitGameButton)
            {
                toSelect = BackGameButton;
            }
        }
        //*/
    }
    //*/

    /// <summary>
    /// When the player hits the start button, the game will react according 
    /// to the toSelect button
    /// </summary>
    void ActivateButton()
    {
        MenuButtonsScript._MainMenu();
        #region OldActivate
        /*
        if (toSelect == StartGameButton)
        {
            Debug.Log("Starting Game...");
            //StartGameButton.Select();
            MenuButtonsScript._LobbyMenu();
        }
        else if (toSelect == OptionsGameButton)
        {
            //OptionsGameButton.Select();
            MenuButtonsScript._OptionsMenu();
        }
        else if (toSelect == QuitGameButton)
        {
            //QuitGameButton.Select();
            MenuButtonsScript._QuitGame();
        }
        else if (toSelect == CreditsGameButton)
        {
            //BackGameButton.Select();
            MenuButtonsScript._CreditsMenu();
        }
        //*/
        #endregion
    }
}