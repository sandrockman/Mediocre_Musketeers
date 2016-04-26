using UnityEngine;
using System.Collections;

public class FillerAttackScript : MonoBehaviour {
    [Tooltip("Old: Holds the arrow prefab for this attack.")]
    public GameObject arrow; //original placeholder
    [Tooltip("New: Holds the attack prefab for this attack.")]
    public AttackClass attack; //what i need to make work 
    [Tooltip("DO NOT EDIT! Holds a copy of attack.")]
    public AttackClass newAttack;
    [Tooltip("Old: Time to aim before firing (sec).")]
    public int aimTime = 2;
    [Tooltip("Old: Time to wait after firing before exiting this script (sec).")]
    public int waitTime = 5;
    [Tooltip("Used to determine whether or not an attack can be made.")]
    public bool canAttack = true;
    [Tooltip("Reference to the parent enemy's detection script.")]
    public ScriptDetectionRadius detect;

    //public NavMeshAgent navi;

	// Use this for initialization
	void Start () {
        //instantiate proper attack
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && canAttack)
        {
            if(attack.attackIsRanged)
            {
                StartCoroutine("RangedAttack");
                canAttack = false;
                gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = false;
                //gameObject.GetComponentInParent<ScriptEnemyMovement>().isMovementRunning = false; //prevent enemy from moving during attack
                //gameObject.GetComponentInParent<Transform>().GetComponentInChildren<ScriptDetectionRadius>().enabled = false;
                detect.enabled = false;
                //if (navi != null)
                //{
                //    Debug.Log("rangedattacknavi");
                //    navi.velocity = Vector3.zero;
                //    navi.enabled = false;
                //}
            }
            else
            {
                StartCoroutine("MeleeAttack");
                canAttack = false;
                gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = false;
                //gameObject.GetComponentInParent<ScriptEnemyMovement>().isMovementRunning = false; //prevent enemy from moving during attack
                //gameObject.GetComponentInParent<Transform>().GetComponentInChildren<ScriptDetectionRadius>().enabled = false;
                detect.enabled = false;
                Debug.Log(detect.enabled);
                //if (navi != null)
                //{
                //    Debug.Log("meleeattacknavi");
                //    navi.velocity = Vector3.zero;
                //    navi.enabled = false;
                //}
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("AttackRadius Exited by: " + other.tag + " and canAttack " + canAttack);
        if(other.tag == "Player")
        {
            //detect.enabled = true;
            if(!canAttack)
            {
                if(gameObject.GetComponentInParent<ScriptEnemyClass>().canFrameCancel)
                {
                    StartCoroutine("FrameCancel");
                }
                
            }
            else
            {
                
                //gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = true;
                detect.enabled = true;
                canAttack = true;
                //if (navi != null)
                //{
                //    navi.enabled = true;
                //}
                this.enabled = false;
            }
        }
    }

    public IEnumerator RangedAttack()
    {
        //original version saved
        //yield return new WaitForSeconds(aimTime);
        //Vector3 arrowShot = transform.TransformDirection(Vector3.forward);
        ////arrowShot = transform.InverseTransformPoint(arrowShot);
        //Debug.Log(GetComponentInParent<Rigidbody>().name);
        //Instantiate(attack, gameObject.GetComponentInParent<Rigidbody>().position + transform.forward, this.gameObject.GetComponentInParent<Rigidbody>().rotation);
        //attack.atkScript = gameObject.GetComponent<FillerAttackScript>();
        //yield return new WaitForSeconds(waitTime);
        //canAttack = true;
        ////Debug.Log("gameobject" + gameObject);
        ////Debug.Log("gameobjecttrans" + gameObject.GetComponentInParent<Transform>());
        ////Debug.Log("gameobjecttransscript" + gameObject.GetComponentInParent<Transform>().GetComponentInChildren<ScriptDetectionRadius>());
        ////gameObject.GetComponentInParent<Transform>().GetComponentInChildren<ScriptDetectionRadius>().enabled = false;
        ////gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = false;
        ////gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = true; //these two lines reset the enemymovement script so it doesn't have any odd values
        //Debug.Log("postattack scriptenemymovement isMoving" + gameObject.GetComponentInParent<ScriptEnemyMovement>().isMovementRunning);
        
        //detect.enabled = true;

        yield return new WaitForSeconds(attack.attackPlaceholderAnimFrames / 30);
        Debug.Log("ArrowShot");
        newAttack = Instantiate(attack, gameObject.GetComponentInParent<Rigidbody>().position + transform.forward, this.gameObject.GetComponentInParent<Rigidbody>().rotation) as AttackClass;
        //Debug.Log(this.gameObject.GetComponent<FillerAttackScript>());

        newAttack.atkScript = this.gameObject.GetComponent<FillerAttackScript>();
        //Debug.Log(newAttack.atkScript);
        //Instantiate(attack, gameObject.GetComponentInParent<Rigidbody>().position + transform.forward, transform.rotation);
        yield return new WaitForSeconds(attack.attackRecoveryFrames / 30);

        if (newAttack != null)
            Destroy(newAttack.gameObject);

        
        //Debug.Log("rangedenemy moving" + gameObject.GetComponentInParent<ScriptEnemyMovement>().isMovementRunning);
        canAttack = true;
        detect.enabled = true;
        //if (navi != null)
        //{
        //    navi.enabled = true;
        //}
        enabled = false;
    }

    public IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(attack.attackPlaceholderAnimFrames / 30);
        Debug.Log("meleeswing");
        newAttack = Instantiate(attack, gameObject.GetComponentInParent< Rigidbody > ().position + transform.forward *1.5f + transform.up, this.gameObject.GetComponentInParent<Rigidbody>().rotation) as AttackClass;
        //Debug.Log(this.gameObject.GetComponent<FillerAttackScript>());
        
        //!!!!!Turn the attack into a trigger and add a dummy trigger to the player object !!!!
        newAttack.atkScript = this.gameObject.GetComponent<FillerAttackScript>();
        newAttack.GetComponent<Collider>().enabled = true;
        Debug.Log(attack.GetComponent<Collider>().enabled + "attacktemplateenabled");
        //newAttack.GetComponent<Collider>().enabled = false;//disables collider after attack so it doesn't act as a hazard
        Debug.Log("attackcolliderenabled? " + attack.GetComponent<Collider>().enabled);
        //Debug.Log(newAttack.atkScript);
        //Instantiate(attack, gameObject.GetComponentInParent<Rigidbody>().position + transform.forward, transform.rotation);
        yield return new WaitForSeconds(attack.attackRecoveryFrames/30);
        //newAttack.GetComponent<Collider>().enabled = true;
        if (newAttack != null)
        Destroy(newAttack.gameObject);
        
        canAttack = true;
        //gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = true;
        detect.enabled = true;
        //if (navi != null)
        //{
        //    navi.enabled = true;
        //}
        enabled = false;
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if(other.tag == "Player") //add in check for if this enemy can animcancel
    //    {
    //        //StopCoroutine("Attack");
    //        //canAttack = true;
    //        //gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = true; use this line for animatino cancelling enemies
    //    }
    //}

    public IEnumerator FrameCancel()
    {
        yield return new WaitForSeconds(gameObject.GetComponentInParent<ScriptEnemyClass>().reactionTime/60);
        gameObject.GetComponentInParent<ScriptEnemyMovement>().enabled = true;
        //if (navi != null)
        //{
        //    navi.enabled = true;
        //}
        detect.enabled = true;
    }

    void OnDisable() //for some reason, onDisable is not being called because for some reason, .enabled = false does not disable
    {
        Debug.Log("disabled"); //except for the debugs, these debugs get called
        //Instantiate(arrow);

        detect.enabled = false;
        detect.enabled = true;
        Debug.Log("isdetectrunning" + detect.enabled);

        Debug.Log(detect.isColliding + "is colliding1");
        this.enabled = true;
    }
}
