using UnityEngine;
using System.Collections;

public class ScriptDetectionRadius : MonoBehaviour
{
    [Tooltip("The gameObject parent of this script.")]
    public GameObject thisParent;
    [Tooltip("This enemy's class script.")]
    public ScriptEnemyClass scriptThing;

    [Tooltip("The targeted player; set whenever the player \"Enters\" (read: Collides with) this enemy's detection sphere.")]
    public Transform thePlayer;
    [Tooltip("This enemy's movement script.")]
    public ScriptEnemyMovement eMove;
    [Tooltip("Is this enemy detecting a player?")]
    public bool isDetecting = false;
    [Tooltip("SphereCollider; this enemy's detection sphere.")]
    public SphereCollider detectSphere;
    [Tooltip("Has this script run it's Awake function at least once?")]
    public bool firstAwake = true;
    [Tooltip("Holds the collider of the detected player for use by other scripts.")]
    public Collider colliderHolder;
    [Tooltip("This enemy's rotation script.")]
    public ScriptEnemyRotation rotate;
    [Tooltip("Used by other scripts to determine whether or not the player is within the detection radius.")]
    public bool isColliding = false; //this is used to avoid on trigger stay
    //public AttackScript attackThing;

    //public NavMeshAgent navi;

    void Awake()
    {
        firstAwake = true;

    }

    void OnEnable()
    {
        Debug.Log("detectEnabled");
        if(isColliding)
        {
            eMove.enabled = true;
            //Debug.Log("postattack scriptenemymovement isMoving" + gameObject.GetComponentInParent<ScriptEnemyMovement>().isMovementRunning);
        }
    }

    // Use this for initialization
    void Start()
    {
        //set thisparent to be this collider's parent in the hierarchy; i.e. the object that contains the object with this script
        //thisParent = ;
        scriptThing = thisParent.GetComponentInParent<ScriptEnemyClass>();
        eMove = GetComponentInParent<ScriptEnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("enemymove" + eMove.enabled);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //if (isDetecting && Vector3.Distance(other.transform.position, eMove.playerLoc) < Vector3.Distance(eMove.moveStart.position, eMove.playerLoc))
            //{

            //    Debug.Log("I see a closer player.");
            //    thePlayer = other.transform;
            //    eMove.playerCollider = other;
            //}
            //call enemymovement coroutine
            //if(thisParent.GetComponentInChildren<FillerAttackScript>().canAttack == true)
            //Debug.Log(thisParent.GetComponentInChildren<FillerAttackScript>().canAttack + "canattack");
            {
                if (scriptThing.isMovementRunning)
                { Debug.Log("I still see a player."); 
                }
                else
                {
                    //StartCoroutine(scriptThing.EnemyBasicMovement());
                    isDetecting = true;
                    Debug.Log("I see a player.");
                    thePlayer = other.transform;
                    eMove.playerCollider = other;
                    Debug.Log(eMove.firstAwake);
                    firstAwake = false;
                    colliderHolder = other;
                    eMove.isMovementRunning = true;
                    eMove.enabled = true;
                    isColliding = true;
                    Debug.Log(eMove.firstAwake);
                    //eMove.StartCoroutine("EnemyRotation");
                    //eMove.hasRun = true;

                }

            }
            //else
            //{
            //    Debug.Log("can't attack");
            //    eMove.hasRun = false;
            //}

        }

        
    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("I don't see a player anymore.");
            eMove.fracJourney = 0;
            eMove.currentLerpTime = 0;
            //eMove.StopCoroutine("EnemyRotation");
            eMove.enabled = false;
            rotate.enabled = false;
            isColliding = false;
            //if(navi != null && navi.enabled)
            //{

            //    navi.velocity = Vector3.zero;
            //    navi.Stop();
            //}
            //            if(eMove.isLerping)
            //            {
            //				isDetecting = false;
            //                scriptThing.isMovementRunning = false;
            //                scriptThing.eMove.isMovementRunning = false;
            //				eMove.isLerping = false;
            //				scriptThing.eMove.enabled = false;
            //
            //            }

        }
    }


    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.collider.tag == "Separation")
    //    {
    //        print("Points colliding: " + other.contacts.Length);
    //        print("First normal of the point that collide: " + other.contacts[0].normal);
    //    }
    //}
    //void OnTriggerStay(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        if (thisParent.GetComponentInChildren<FillerAttackScript>().canAttack != true)
    //        {
    //            //Debug.Log("can't attack");
    //            eMove.hasRun = false;
    //        }
    //    }
    //}

    void OnDisable()
    {
        Debug.Log("detect disabled");
    }
}
