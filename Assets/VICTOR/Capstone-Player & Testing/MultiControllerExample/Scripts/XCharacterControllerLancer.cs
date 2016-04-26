using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/**
 * @author Victor Haskins
 * class XCharacterControllerLancer
 * movement script that translates Microsoft compatible controller movements
 * to the character Lancer unit controls.
 */

public class XCharacterControllerLancer : MonoBehaviour {
    [Tooltip("Player index enum to be set for players. Shouldn't have to alter.")]
    public PlayerIndex playerIndex;
    //move speed variable for script use
    float moveSpeed = 4f;
    //rotation rate variable to be altered by script.
    float gearShift = 90f;
    [Tooltip("height of jump mechanic. Deprecated. Not in final build. Testing use.")]
    public float jumpSpeed = 6f;
    [Tooltip("toggle jump use.")]
    public bool enableMoveJoyJump = true;
    //keeps track of time in forward movement for changing speed and rotation rate
    float timeGearShift;
    [Tooltip("First Gear movement rate. Must be set positive. Set lower than moveSpeed2 or 3.")]
    public float moveSpeed1 = 4f;
    [Tooltip("Time that starts the first gear shift. Should always be zero.")]
    public float gearShiftTime1 = 0;
    [Tooltip("2nd Gear movement rate. Must be set positive. Set higher than moveSpeed1 lower than moveSpeed3.")]
    public float moveSpeed2 = 6f;
    [Tooltip("Time that starts the 2nd gear shift. higher than first gear, lower than 3rd.")]
    public float gearShiftTime2 = 2f;
    [Tooltip("3rd Gear movement rate. Must be set positive. Set higher than moveSpeed1 or 2.")]
    public float moveSpeed3 = 8f;
    [Tooltip("Time that starts the 3rd gear shift. higher than first & second gear")]
    public float gearShiftTime3 = 4f;
    [Tooltip("reverse speed. Set as a negative number.")]
    public float reverseSpeed = -2f;
    [Tooltip("tilt percentage between 0 and 1 that will register increase in gearShiftTime")]
    public float shiftPull = 0.55f;
    [Tooltip("Rate at which the velocity decreases.")]
    public float deceleration = 0.2f;
    [Tooltip("Rotation rate for the first gear shift. Largest amount.")]
    public float gear1RotationRate = 90f;
    [Tooltip("Rotation rate for the second gear shift. Between gear1 and gear3 rates.")]
    public float gear2RotationRate = 45f;
    [Tooltip("Rotation rate for the third gear shift. Smallest amount.")]
    public float gear3RotationRate = 10f;
    //public Transform aimTransform;
    [Tooltip("turns on/off OnGUI button inputs.")]
    public bool testButtonInputs = true;
    [Tooltip("Canvas gameobject that dictates the actions of Pausing the game.")]
    public GameObject pauseCanvas;
    [Tooltip("Particle Effect gameobject to show max speed.")]
    public GameObject maxSpeedEffect;
    [Tooltip("Animator used to show off attacks and other cool effects.")]
    public Animator anim;
    [Tooltip("Script to turn on attack colliders.")]
    public PlayerAttackScript attackScript;
    //previous and current states of the controller for the specific index
    GamePadState previousState;
    GamePadState currentState;
    //vectors for moving and aiming the character
    Vector3 moveJoy;
    Vector3 aimJoy;
    //variable that allows for jumping when turned on.
    bool jump = false;
    //variable that tells the script when it is trying to brake.
    bool braking = false;
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
        maxSpeedEffect.SetActive(false);
        anim = GetComponent<Animator>();
        groundDist = GetComponent<Collider>().bounds.size.y;
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
            pauseCanvas.SetActive(true);
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

        //brake by pressing the right shoulder button
        if (previousState.Buttons.RightShoulder == ButtonState.Released &&
            currentState.Buttons.RightShoulder == ButtonState.Pressed)
        {
            GearShift();
        }

        //set forward velocity
        if (moveJoy.y > 0 && !braking)
        {
            if (timeGearShift >= gearShiftTime3)
                moveSpeed = moveSpeed3;
            else if (timeGearShift >= gearShiftTime2)
                moveSpeed = moveSpeed2;
            else //timeGearShift <gearShiftTime2 , or in first gear
                moveSpeed = moveSpeed1;

            //if forward movement on stick is greater than the 
            //shiftPull increase the time toward moving to the next gear shift
            if (moveJoy.y >= shiftPull)
                timeGearShift += Time.deltaTime;
        }
        else if(moveJoy.y < 0 && !braking) //braking
        {
            GearShift();
        }
        else if(moveJoy.y == 0 && !braking) //deceleration which will act like braking
        {
            if (moveSpeed > 0)
            {
                moveSpeed -= deceleration;
                if (moveSpeed < moveSpeed1)
                {
                    timeGearShift = gearShiftTime1;
                }
                else if (moveSpeed < moveSpeed2)
                {
                    timeGearShift = gearShiftTime2;
                }
                //if at rest but character's move speed is negative, zero out.
                if (moveSpeed < 0)
                    moveSpeed = 0;
                braking = true;
            }
            else if (moveSpeed < 0)
            {
                moveSpeed = 0;
            }
        }

        //set rotation rate from forward moving player.
        if (timeGearShift >= gearShiftTime3)
        {
            //third gear rotation rate sent
            gearShift = gear3RotationRate;
        }
        else if(timeGearShift >= gear2RotationRate)
        {
            //second gear rotation rate sent
            gearShift = gear2RotationRate;
        }
        else
        {
            //first gear rotation rate sent
            gearShift = gear1RotationRate;
        }

        //aiming
        //if(aimJoy.sqrMagnitude > 0)
        //	aimTransform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0,0,90) * new Vector3( aimJoy.x, aimJoy.y, 0));

        //LEFT SIDE ATTACK BY PRESSING X BUTTON
        if (previousState.Buttons.X == ButtonState.Released &&
            currentState.Buttons.X == ButtonState.Pressed)
        {
            if (attackScript.canAttack)
            {
                attackScript.StartLeftSideAttack();
            }
        }

        //LANCE ATTACK BY PRESSING Y BUTTON
        if (previousState.Buttons.Y == ButtonState.Released &&
            currentState.Buttons.Y == ButtonState.Pressed)
        {
            if (attackScript.canAttack)
            {
                //anim.SetBool("DidSwipe", false);
                anim.SetTrigger("Swipey");
                attackScript.StartLanceAttack();
            }
        }

        //RIGHT SIDE ATTACK BY PRESSING B BUTTON
        if (previousState.Buttons.B == ButtonState.Released &&
            currentState.Buttons.B == ButtonState.Pressed)
        {
            if (attackScript.canAttack)
            {
                attackScript.StartRightSideAttack();
            }
        }

        //SHIELD ACTIVATE BY PRESSING LEFT SHOULDER BUTTON
        if (previousState.Buttons.LeftShoulder == ButtonState.Released &&
            currentState.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            if (attackScript.canAttack)
            {
                anim.SetTrigger("Blockey");
                attackScript.StartShield();
                //anim.SetBool("DidBlock", true);
            }
        }

        //Turn on the max speed effect if at highest speed variable
        if(timeGearShift >= gearShiftTime3 && maxSpeedEffect.activeSelf == false)
        {
            maxSpeedEffect.SetActive(true);
        }
        else if(timeGearShift < gearShiftTime3 && maxSpeedEffect.activeSelf == true)
        {
            maxSpeedEffect.SetActive(false);
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
        velocity += transform.TransformDirection(Vector3.forward * moveSpeed);
        velocity.y = grav;
        //velocity.y = GetComponent<Rigidbody>().velocity.y;
        //  fail. Boomerang action. Thought I could get away with it because
        //  the start and end points move with the character
        transform.position = Vector3.Lerp(transform.position, velocity, Time.fixedDeltaTime );
        //  fail. moves much faster and will eventually clip through the terrain
        //  might work with some tweaking, but still just as choppy as the tried and true.
        //  ALSO, direction and rotation are out of sync with this
        //transform.Translate(transform.forward * moveSpeed * Time.fixedDeltaTime, Space.Self);

        if (jump)
        {
            velocity.y = jumpSpeed;
            jump = false;
        }

        //GetComponent<Rigidbody>().velocity = velocity;
        if (braking)
            braking = false;

        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, currentState.Triggers.Left / 10f, currentState.Triggers.Right / 10f);
        //*
        // Make the current object turn
        //transform.localRotation *= Quaternion.Euler(0.0f, aimJoy.x * gearShift * Time.deltaTime, 0.0f);
        transform.Rotate(0, aimJoy.x * gearShift * Time.deltaTime, 0);
        //*/
    }

    /*
    void OnDrawGizmos()
    {
        if (aimTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(aimTransform.position, aimTransform.right);
        }
    }
    //*/

    /// <summary>
    /// Changes the gear shift times; typically a downshift
    /// </summary>
    void GearShift()
    {
        //if in third gear, downgrade to second gear
        if (timeGearShift >= gearShiftTime3)
        {
            timeGearShift = gearShiftTime2;
        }//if in second gear, downgrade to first gear
        else if (timeGearShift >= gearShiftTime2)
        {
            timeGearShift = gearShiftTime1;
        }//if in first gear, and moving forward, stop.
        else //timeGearShift < gearShiftTime2 i.e. in first gear
        {
            if (timeGearShift == 0 && moveSpeed <= 0)
            {
                moveSpeed = reverseSpeed;
            }
            else
            {
                timeGearShift = gearShiftTime1;
                moveSpeed = 0;
            }
        }
        braking = true;

    }
    /// <summary>
    /// used to print button inputs to screen if wanted.
    /// </summary>
    void OnGUI()
    {
        //prints button inputs to screen if true.
        if (testButtonInputs)
        {
            string text = "Use left stick to turn the cube, hold A to change color\n";
            text += string.Format("IsConnected {0} Packet #{1}\n", currentState.IsConnected, currentState.PacketNumber);
            text += string.Format("\tTriggers {0} {1}\n", currentState.Triggers.Left, currentState.Triggers.Right);
            text += string.Format("\tD-Pad {0} {1} {2} {3}\n", currentState.DPad.Up, currentState.DPad.Right, currentState.DPad.Down, currentState.DPad.Left);
            text += string.Format("\tButtons Start {0} Back {1}\n", currentState.Buttons.Start, currentState.Buttons.Back);
            text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", currentState.Buttons.LeftStick, currentState.Buttons.RightStick, currentState.Buttons.LeftShoulder, currentState.Buttons.RightShoulder);
            text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", currentState.Buttons.A, currentState.Buttons.B, currentState.Buttons.X, currentState.Buttons.Y);
            text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", currentState.ThumbSticks.Left.X, currentState.ThumbSticks.Left.Y, currentState.ThumbSticks.Right.X, currentState.ThumbSticks.Right.Y);
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);


            Vector3 screenPos = Camera.main.WorldToScreenPoint(GetComponent<Renderer>().bounds.max);
            GUI.color = Color.black;
            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 100, 100), playerIndex.ToString());
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDist + 0.1f);
    }
}
