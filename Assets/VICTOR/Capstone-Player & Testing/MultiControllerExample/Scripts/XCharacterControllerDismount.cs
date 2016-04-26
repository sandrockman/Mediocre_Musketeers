using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/**
 * @author Victor Haskins
 * class XCharacterControllerDismount
 * movement script that translates Microsoft compatible controller movements
 * to the character dismounted unit controls.
 */
public class XCharacterControllerDismount : MonoBehaviour {
    [Tooltip("Player index enum to be set for players. Shouldn't have to alter.")]
    public PlayerIndex playerIndex;
    //move speed variable for script use
    public float moveSpeed = 2f;
    //rotation rate variable to be altered by script.
    public float gearShift = 150f;
    [Tooltip("height of jump mechanic. Deprecated. Not in final build. Testing use.")]
    public float jumpSpeed = 6f;
    [Tooltip("toggle jump use.")]
    public bool enableMoveJoyJump = true;
    
    [Tooltip("Canvas gameobject that dictates the actions of Pausing the game.")]
    public GameObject pauseCanvas;
    [Tooltip("Script to turn on attack colliders.")]
    public PlayerDismountAttackScript attackScript;
    //previous and current states of the controller for the specific index
    GamePadState previousState;
    GamePadState currentState;
    //vectors for moving and aiming the character
    Vector3 moveJoy;
    Vector3 aimJoy;
    //variable that allows for jumping when turned on.
    bool jump = false;
    //artifact from previous versions
    //float timeElapsedRunning = 0f;

    public float smooth = 5.0f;

    public float groundDist;
    public float grav;
    public float gravStep = 0.1f;
    public float maxGrav = -2f;

    void Awake()
    {
        pauseCanvas = GameObject.Find("PauseCanvas");
        attackScript.playerIndex = playerIndex;
        //camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //pauseScript.pCanvas = GameObject.Find("Main Camera").GetComponent<PauseBehaviorScript>().pCanvas;
    }

    void Update()
    {
        HandleXInput();
    }

    /// <summary>
    /// Reads inputs and performs actions such as movement and rotation.
    /// </summary>
    void HandleXInput()
    {
        //get current state of controller for player index
        currentState = GamePad.GetState(playerIndex);
        //disregard if player controller is not connected
        if (!currentState.IsConnected)
        {
            return;
        }

        //Pause by pushing Start Button OR Enter key
        if (previousState.Buttons.Start == ButtonState.Released &&
            currentState.Buttons.Start == ButtonState.Pressed ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //camera.GetComponent<PauseBehaviorScript>().PauseGame();
            //GameObject tempCanvas = GameObject.Find("PauseCanvas") as GameObject;
            Time.timeScale = 0f;
            if (GameObject.Find("Mission_Failed") || GameObject.Find("Mission_Complete"))
            {

            }
            else
            {
                pauseCanvas.SetActive(true);
            }
        }

        //used to set the forward/back movement of the character locally
        moveJoy.x = currentState.ThumbSticks.Left.X;
        moveJoy.y = currentState.ThumbSticks.Left.Y;
        //used to set the xz rotation of the character globally.
        aimJoy.x = currentState.ThumbSticks.Right.X;
        aimJoy.y = currentState.ThumbSticks.Right.Y;

        //jump by pushing A
        if (previousState.Buttons.A == ButtonState.Released &&
            currentState.Buttons.A == ButtonState.Pressed && enableMoveJoyJump)
        {
            if (transform.GetComponent<Rigidbody>().velocity.y == 0)
                jump = true;
        }

        
        //aiming
        //if(aimJoy.sqrMagnitude > 0)
        //	aimTransform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0,0,90) * new Vector3( aimJoy.x, aimJoy.y, 0));

        //LANCE ATTACK BY PRESSING Y BUTTON
        if (previousState.Buttons.RightShoulder == ButtonState.Released &&
            currentState.Buttons.RightShoulder == ButtonState.Pressed)
        {
            if (attackScript.canAttack)
            {
                attackScript.StartAttack();
            }
        }

        previousState = currentState;
    }

    void FixedUpdate()
    {
        //Vector3 velocity = GetComponent<Rigidbody>().velocity;
        //velocity.x = moveJoy.x * moveSpeed;
        //velocity.z = moveJoy.y * moveSpeed;
        if (IsGrounded())
        {
            grav = 0;
        }
        else
        {
            grav = (grav >= maxGrav ? maxGrav : grav - gravStep);

        }
        //Vector3 velocity = Vector3.forward * moveSpeed;
        //Vector3 velocity = transform.TransformDirection(Vector3.forward * moveSpeed);
        Vector3 velocity = transform.position;
        Vector3 footMove = new Vector3(moveJoy.x, 0, moveJoy.y);
        footMove = footMove.normalized;
        footMove = transform.position + (footMove * moveSpeed);
        velocity.y = grav;
        //footMove.y = GetComponent<Rigidbody>().velocity.y;
        //velocity += transform.TransformDirection(Vector3.forward * moveSpeed);
        //velocity.y = GetComponent<Rigidbody>().velocity.y;
        //  fail. Boomerang action. Thought I could get away with it because
        //  the start and end points move with the character
        transform.position = Vector3.Lerp(transform.position, footMove, Time.fixedDeltaTime);
        //  fail. moves much faster and will eventually clip through the terrain
        //  might work with some tweaking, but still just as choppy as the tried and true.
        //  ALSO, direction and rotation are out of sync with this
        //transform.Translate(transform.forward * moveSpeed * Time.fixedDeltaTime, Space.Self);

        if (jump)
        {
            velocity.y = jumpSpeed;
            jump = false;
        }

        //*
        // Make the current object turn
        //transform.localRotation *= Quaternion.Euler(0.0f, aimJoy.x * gearShift * Time.deltaTime, 0.0f);
        if (aimJoy.x == 0 && aimJoy.y == 0)
        {
            Vector3 newLook = new Vector3(moveJoy.x, 0, moveJoy.y);
            Vector3 idleRotate =
                Vector3.RotateTowards(transform.forward, newLook,
                                        (gearShift * Mathf.PI / 180) * Time.fixedDeltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(idleRotate);
        }
        else
        {
            Vector3 newLook = new Vector3(aimJoy.x, 0, aimJoy.y).normalized;
            newLook = transform.position + newLook;
            transform.LookAt(newLook, Vector3.up);
        }
        //*/
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDist + 0.1f);
    }
}