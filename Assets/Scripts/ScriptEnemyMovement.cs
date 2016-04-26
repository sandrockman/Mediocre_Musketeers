using UnityEngine;
using System.Collections;


public class ScriptEnemyMovement : MonoBehaviour
{
    [Tooltip("The position of this enemy at the beginning of a given lerp.")]
    public Transform moveStart; //where the enemy starts from
    [Tooltip("The target point for the lerp end.")]
    public Transform moveEnd; //where the enemy is going
    [Tooltip("The position of the targeted player character.")]
    public Vector3 playerLoc; //where the player is
    [Tooltip("The speed this enemy moves.")]
    public float speed = 0.2F; //how fast does the enemy move

    private float startTime; //can be used for lerping //legacy code, remove after release
    private float journeyLength; //can be used for lerping //legacy code remove after release
    [Tooltip("Used to check if this script is running.")]
    public bool isMovementRunning; //check to see if this script is running

    public bool isLerping = false; //check to see if this is lerping //legacy code, remove after release
    Transform targetStart; //previously llama //legacy code, remove after release
    Transform llamatwo; //temp variable during testing //legacy code, remove after release
    [Tooltip("Has this script run at least once since parent instantiation?")]
    public bool hasRun = true; //has this been run at least once since its parent was instantiated
    [Tooltip("Has this enemy run it's ScriptDetectionRadius at least once?")]
    public bool firstAwake = true; //is this the first time scriptdetectionradius has run
    [Tooltip("This holds a transform that doesn't do anything.")]
    public Transform thisParent; //this may be of use...someday
    [Tooltip("This enemy's detection script.")]
    public ScriptDetectionRadius detect; //reference to detectionradius of this enemy
    [Tooltip("This enemy's class script.")]
    public ScriptEnemyClass eClass; //reference to the enemyclass
    [Tooltip("Vector3; holds data of extents of the playerCollider of the target; see also, size/2.")]
    public Vector3 playerExents; //the x,y,z vector that goes from the center of the playercollider to a corner; see also, size/2
    [Tooltip("The targetted player's collider.")]
    public Collider playerCollider; //the collider of the player
    [Tooltip("Vector3; holds data of extents of this enemy's collider; see also, size/2.")]
    public Vector3 myExtents; //the x,y,z vector that goes from the center of the enemy to a corner
    [Tooltip("This enemy's collider.")]
    public Collider myCollider; //the collider of this enemy
    [Tooltip("This enemy's Melee or Ranged radius.")]
    public SphereCollider attackSphere; //the attack sphere collider; used to determine if this enemy is in attack range
    [Tooltip("This enemy's rotation script.")]
    public ScriptEnemyRotation rotate; //the enemy rotation script

    [Tooltip("Vector3 form of moveStart.")]
    Vector3 startPos; //vector form of moveStart
    [Tooltip("Vector3 form of moveEnd.")]
    public Vector3 endPos; //vector form of moveEnd
    [Tooltip("A float used in lerping; represents what % of completion the lerp is. In this case, it is repeatedly reset since the lerp gets reset.")]
    public float fracJourney = 0f; //used for lerping; represents what % of completion the lerp is
    [Tooltip("Caution - Float used to represent how much distance has been traveled since the beginning of the lerp.")]
    float distcovered = 0f; //represents how far since the beginning of the lerp
    [Tooltip("Caution - Float used for calculating lerp. It's magic.")]
    public float lerpTime = 1f; //used for calculating the lerp; it's magic
    [Tooltip("Caution - Float used for calculating lerp. It's magic. Seriously. Lerping is weird.")]
    public float currentLerpTime; //used for calculating the lerp; it's also magic; seriously, lerping is weird

    [Tooltip("Stores a ray used for detecting walls.")]
    public Ray wallRay; //stores a ray used for wall detection
    [Tooltip("Stores a ray used for detecting walls.")]
    public Ray wallRayCenter; //stores a ray used for wall detection
    [Tooltip("Stores a ray used for detecting walls.")]
    public Ray wallRayRight; //stores a ray used for wall detection
    [Tooltip("Cautino - Stores a layermask for wallRay's cast.")]
    public LayerMask wallMask; //the wall layermask is stored here...


    //public NavMeshAgent navi;
    //public bool useNavmesh = false;
    //public bool navcheckonce = false; //if we have switched over to using navigation, true
    //public bool offGrid = false; //set this variable to true during try-catch if the try fails; this means this agent is off of the navmesh and needs to be put back on;
    void Awake()
    {
        //this did something at one point, but it's job was outsourced

    }

    void Start()
    {

        //called at the start of the level
        this.enabled = false; //disables this script; this one line of code prevents many crashes
    }

    // Runs whenever this script becomes enabled; this will only run when the enemy can detect the player, then sets the variables necessary
    void OnEnable()
    {
        //Debug.Log("onenable hasrun" + hasRun); //this is one of many debugs; 
        //Debug.Log(detect.firstAwake);
        if (detect.firstAwake) //if detect hasn't run it's awake function
            hasRun = false; //hasRun set to false
        if (detect.firstAwake != true) //if detect has already run it's awake function
            hasRun = true; //hasRun set to true
        Debug.Log("llamaduck" + hasRun);
        if (hasRun) //if hasRun == (is equal to) true
        {
            //if (firstRun) { thisParent = GetComponentInParent<Transform>(); }
            //else

            //if (this.gameObject.GetComponent<NavMeshAgent>() != null)
            //    navi = this.gameObject.GetComponent<NavMeshAgent>();
            Debug.Log("ONEnableHasRun");
            isMovementRunning = true; //set variable to true
            

            eClass = GetComponent<ScriptEnemyClass>(); //re/assign eClass
            detect = GetComponentInChildren<ScriptDetectionRadius>(); //re/assign detect; 
            playerCollider = detect.colliderHolder; //re/assign playerCollider
            //playerCollider = moveEnd.GetComponentInParent<Collider>();
            moveEnd = playerCollider.transform; //set moveEnd to be equal to the transform of the playerCollider
            playerExents = playerCollider.bounds.extents; //re/assign playerExtents to extents of its collider's bounding box
            myCollider = GetComponentInParent<Collider>(); //re/assign myCollider; gets the collider data from gameObject's parent
            myExtents = myCollider.bounds.extents; //re/assign myExtents; as playerExtents above
            Debug.Log(playerExents + "playerext");
            Debug.Log(myExtents + "enemyextents");
            Debug.Log(myExtents + playerExents); //these 3 lines output the extents of the player, this enemy, and their combined extents 
            targetStart = thisParent; //i'm not sure why this is here; legacy code? probably could remove it...


            startPos = transform.position; //re/assigns startPos to the position of this enemy
            endPos = moveEnd.position - (playerExents + myExtents); //this calculation is used once; sets endpos to be an offset position of moveEnd.position - the combined extents, thus forming a buffer
            Debug.Log(startPos);
            Debug.Log(endPos);
            attackSphere = eClass.attackRange; //re/assigns attackSphere from the eClass variable attackRange
            {
                startTime = Time.time; //re/sets starttime to the system time
                //set end marker to be a the endpoint of a vector with a set magnitude, but a direction of the player object's transform
                playerLoc = moveEnd.position; //re/sets playerLoc to moveEnd.position; it's weird but necessary for the math to work
                targetStart = moveEnd; //re/sets targetStart
                playerLoc = 
                playerLoc = Vector3.ClampMagnitude(playerLoc, speed); //re/sets playerLoc to be a vector3 with magnitude clamped at speed

                //llama.position = playerLoc;

                journeyLength = Vector3.Distance(moveStart.position, targetStart.position); //re/sets journey length
                isLerping = true; //sets isLerping



                startPos = transform.position; //re/assigns startpos
                endPos = moveEnd.position - (playerExents + myExtents); //duplicate of above; no harm done
                if (Mathf.Abs(playerExents.y - myExtents.y) < 1) //if the absolute value of the difference of the y extents is < 1; is there isn't much of a height difference
                    endPos.y = this.gameObject.transform.position.y; //sets the y value endPos to be the current y position; this is used for preventing enemies from sinking into hte ground
                else
                    endPos.y += playerExents.y - myExtents.y; //otherwise, recalculate endPos.y to be more accurate; note the floating point error
                speed = eClass.speed;
                //insert code to set transforms properly?
                rotate.speed = speed; //re/sets speed of rotation
                rotate.playerCollider = playerCollider; //re/sets the playerCollider of rotate
                rotate.detect = detect; //these really just do the same thing for rotate as they do here
                rotate.enabled = true;
                rotate.canRotate = true;
            }

        }
        else { }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("eMove enabled " + enabled);
        if (hasRun)
        {
            //Debug.Log ("islerping" + isLerping);
          //  Debug.Log("playerpos" + moveEnd.position);
            //Debug.Log ("isenabled" + this.enabled);
            //Debug.Log ("distance p-e" + Mathf.Abs (Vector3.Distance (this.transform.position, moveEnd.transform.position)));
            if (this.enabled)
            {
                //if (firstRun) {
                //	firstRun = false;
                //	this.enabled = false;} 
                //else 
                {
                    //Debug.Log(navi);
                    if (isMovementRunning /*&& navi == null*/)
                    {
                        wallRay = new Ray(transform.position, transform.forward * detect.detectSphere.radius); //need to grab local data
                        //Debug.Log(Vector3.forward * detect.detectSphere.radius + "detectraydius");
                        Debug.DrawRay(this.gameObject.transform.position, playerLoc, Color.green);
                        RaycastHit wallRayHit;
                        if(Physics.Raycast(wallRay, out wallRayHit, detect.detectSphere.radius, wallMask))
                        {
                            Debug.Log(wallRayHit.collider.name + " " + wallRayHit.distance);
                            Debug.Log(wallRayHit.collider.tag);
                            if (wallRayHit.collider.transform.tag == "Barrier")
                                this.enabled = false;
                        }
                        
                    }
                    //if (isMovementRunning && navi != null && navi.enabled)
                    //{
                    //    navi.updateRotation = true;
                    //    if (navi.areaMask == 0) //this thing is in binary, more checks to implement in the future
                    //    {
                    //        navi.areaMask = 1;
                    //    }
                    //    //navMesh raycasting is garbage; ignores anything that isn't part of the mesh, meaning we have to rework the mesh or use regular rays
                    //    wallRay = new Ray(transform.position, Quaternion.AngleAxis(30, Vector3.up) * transform.forward * detect.detectSphere.radius); //need to grab local data
                    //    wallRayCenter = new Ray(transform.position,  transform.forward * detect.detectSphere.radius);
                    //    wallRayRight = new Ray(transform.position, Quaternion.AngleAxis(-30, Vector3.up) * transform.forward * detect.detectSphere.radius);

                    //    //Debug.Log(wallRay + "wallray");
                    //    //Debug.DrawRay(this.gameObject.transform.position, playerLoc, Color.green);
                    //    RaycastHit wallRayHit;
                    //    RaycastHit wallRayHitCenter;
                    //    RaycastHit wallRayHitRight;
                    //    Physics.Raycast(wallRay, out wallRayHit, detect.detectSphere.radius, wallMask);
                    //    Physics.Raycast(wallRayCenter, out wallRayHitCenter, detect.detectSphere.radius, wallMask);
                    //    Physics.Raycast(wallRayRight, out wallRayHitRight, detect.detectSphere.radius, wallMask);
                    //    //Debug.Log(wallRayHit.distance + "wallrayhitdistance");
                    //    Debug.DrawRay(transform.position, transform.forward * detect.detectSphere.radius, Color.yellow);
                    //    Debug.DrawRay(transform.position, Quaternion.AngleAxis(30, Vector3.up) * transform.forward * detect.detectSphere.radius, Color.yellow);
                    //    Debug.DrawRay(transform.position, Quaternion.AngleAxis(-30, Vector3.up) * transform.forward * detect.detectSphere.radius, Color.yellow);

                    //    if (Physics.Raycast(transform.position, transform.forward * detect.detectSphere.radius, out wallRayHit, detect.detectSphere.radius, wallMask))
                    //    {
                    //        //RayCastSwitchFunction(wallRay, wallRayHit);
                    //    }
                    //    else if(Physics.Raycast(wallRayCenter, out wallRayHitCenter, detect.detectSphere.radius, wallMask))
                    //    {
                    //        //RayCastSwitchFunction(wallRayCenter, wallRayHitCenter);
                    //    }
                    //    else if(Physics.Raycast(wallRayRight, out wallRayHitRight, detect.detectSphere.radius, wallMask))
                    //    {
                    //        //RayCastSwitchFunction(wallRayRight, wallRayHitRight);
                    //    }
                    //    else
                    //    {
                    //        SwitchToKinematicMovement();
                    //    }
                            

                    }
                //if (isMovementRunning && navi != null && navi.enabled) //this whole thing was a grand experiment, but doesn't work the way we need it to; causing more problems than it solves
                //{
                //    //this one is used for enemies with navmeshagents
                //    if (Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) > attackSphere.radius)
                //    {
                //        //Debug.Log ("lerpcheck" + isLerping);
                //        if (fracJourney < 1) //is the object at the end of the lerp?
                //        {
                //            // Debug.Log(fracJourney + "fracjourney");
                //            // Debug.Log("player distance: " + Vector3.Distance(transform.position, moveEnd.position));
                //            // Debug.Log("endpos" + endPos);
                //            //Debug.Log("mathtarget " + (moveEnd.position - (playerExents + myExtents)));
                //            if (endPos != moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos))
                //            {
                //                //  Debug.Log("reset targeter");
                //                fracJourney = 0;
                //                distcovered = 0;
                //                currentLerpTime = 0; //leave this line out for funky teleports
                //                startPos = transform.position;
                //                //endPos = moveEnd.position - (playerExents + myExtents); //!!!!Try replacing with bounds.closestpoint and mesh.bounds
                //                endPos = moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos);
                //                if (Mathf.Abs(playerExents.y - myExtents.y) < 3)
                //                    endPos.y = this.gameObject.transform.position.y;
                //                else
                //                    endPos.y += playerExents.y - myExtents.y;
                //                //   Debug.Log("Endpos before clamp" + endPos);
                //                //endPos = Vector3.Normalize(endPos);
                //                //endPos *= speed;

                //                //   Debug.Log("endpos after clamp" + endPos);
                //            }

                //            currentLerpTime += Time.deltaTime;
                //            if (currentLerpTime > lerpTime)
                //            {
                //                currentLerpTime = lerpTime;
                //            }
                //            fracJourney = currentLerpTime / lerpTime;
                //            //insert separation functionality
                //            //navi.Move(Vector3.MoveTowards(startPos, endPos, fracJourney * speed));
                //            //Debug.Log(Vector3.MoveTowards(startPos, endPos, fracJourney * speed) + "movetowards stuff");
                //            if(!useNavmesh)
                //            {
                //                if(navi.enabled)
                //                navi.velocity = transform.forward * speed * 1.3f; //divide movetowards by anything and watch the fireworks
                //                //navi.velocity /= 2;
                //                //Debug.Log(navi.velocity + "navivelocity");
                //                //navi.Move(Vector3.MoveTowards(startPos, endPos, fracJourney * speed));
                //                if(navi.enabled)
                //                navi.Stop();
                //            }


                //            //transform.position = Vector3.MoveTowards(startPos, endPos, fracJourney * speed);
                //            //navi.SetDestination(endPos);

                //            Vector3 blahVect = Vector3.RotateTowards(transform.forward, playerCollider.transform.position - transform.position, speed * Time.deltaTime, 0.0f);

                //            //Vector3.ClampMagnitude(blahVect, detect.detectSphere.radius);
                //            Debug.DrawRay(transform.position, blahVect, Color.magenta);

                //            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(blahVect);
                //            //float blahAngle;
                //            //blahAngle = Vector3.Angle(Vector3.forward, endPos);
                //            //if(blahVect)
                //            ////blahAngle = Mathf.Acos(Mathf.PI / 180 * endPos.z / endPos.magnitude) * 180 / Mathf.PI;
                //            //transform.Rotate(Vector3.up, blahAngle, Space.Self);

                //        }
                //        else
                //        {
                //            Debug.Log("reached1");
                //            //wait until player leaves attack range to reset values
                //            //do attack stuff
                //            //Debug.Log("player distance: " + Vector3.Distance(transform.position, moveEnd.position));
                //            if (Vector3.Distance(transform.position, moveEnd.position) > attackSphere.radius)
                //            {
                //                Debug.Log("blahblahblah");
                //                fracJourney = 0;
                //                currentLerpTime = 0;
                //                startPos = transform.position;
                //                endPos = moveEnd.position - (playerExents + myExtents);
                //                if (Mathf.Abs(playerExents.y - myExtents.y) < 1)
                //                    endPos.y = this.gameObject.transform.position.y;
                //                else
                //                    endPos.y += playerExents.y - myExtents.y;
                //                //endPos.magnitude = speed;
                //            }
                //            else { }

                //        }
                //    }
                //}
                if (isMovementRunning) //due to how the navmesh system works, it is essential to NOT use it when unnecessary; it isn't efficient and doesn't even work the way we need it to
                {
                    if (Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) > attackSphere.radius)
                    {
                        Debug.Log("lerpcheck" + isLerping);
                        if (fracJourney < 1) //is the object at the end of the lerp?
                        {
                            // Debug.Log(fracJourney + "fracjourney");
                            // Debug.Log("player distance: " + Vector3.Distance(transform.position, moveEnd.position));
                            // Debug.Log("endpos" + endPos);
                            //Debug.Log("mathtarget " + (moveEnd.position - (playerExents + myExtents)));
                            if (endPos != moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos))
                            {
                                //  Debug.Log("reset targeter");
                                fracJourney = 0;
                                distcovered = 0;
                                currentLerpTime = 0; //leave this line out for funky teleports
                                startPos = transform.position;
                                //endPos = moveEnd.position - (playerExents + myExtents); //!!!!Try replacing with bounds.closestpoint and mesh.bounds
                                endPos = moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos);
                                if (Mathf.Abs(playerExents.y - myExtents.y) < 3)
                                    endPos.y = this.gameObject.transform.position.y;
                                else
                                    endPos.y += playerExents.y - myExtents.y;
                                //   Debug.Log("Endpos before clamp" + endPos);
                                //endPos = Vector3.Normalize(endPos);
                                //endPos *= speed;

                                //   Debug.Log("endpos after clamp" + endPos);
                            }
                            currentLerpTime += Time.deltaTime;
                            if (currentLerpTime > lerpTime)
                            {
                                currentLerpTime = lerpTime;
                            }
                            fracJourney = currentLerpTime / lerpTime;
                            //insert separation functionality
                            transform.position = Vector3.MoveTowards(startPos, endPos, fracJourney * speed);

                            Vector3 blahVect = Vector3.RotateTowards(transform.forward, playerCollider.transform.position - transform.position, speed * Time.deltaTime, 0.0f);

                            //Vector3.ClampMagnitude(blahVect, detect.detectSphere.radius);
                            Debug.DrawRay(transform.position, blahVect, Color.magenta);

                            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(blahVect);
                            //float blahAngle;
                            //blahAngle = Vector3.Angle(Vector3.forward, endPos);
                            //if(blahVect)
                            ////blahAngle = Mathf.Acos(Mathf.PI / 180 * endPos.z / endPos.magnitude) * 180 / Mathf.PI;
                            //transform.Rotate(Vector3.up, blahAngle, Space.Self);

                        }
                        else
                        {
                            Debug.Log("reached1");
                            //wait until player leaves attack range to reset values
                            //do attack stuff
                            //Debug.Log("player distance: " + Vector3.Distance(transform.position, moveEnd.position));
                            if (Vector3.Distance(transform.position, moveEnd.position) > attackSphere.radius)
                            {
                                Debug.Log("blahblahblah");
                                fracJourney = 0;
                                currentLerpTime = 0;
                                startPos = transform.position;
                                endPos = moveEnd.position - (playerExents + myExtents);
                                if (Mathf.Abs(playerExents.y - myExtents.y) < 1)
                                    endPos.y = this.gameObject.transform.position.y;
                                else
                                    endPos.y += playerExents.y - myExtents.y;
                                //endPos.magnitude = speed;
                            }
                            else { }

                        }
                    }
                }

            }
            


            //if (isLerping) {
            ////					float distCovered = (Time.time - startTime) * speed; //test for framerate independence or not
            ////					Debug.Log ("distcovered" + distCovered);
            ////					float fracJourney = distCovered / journeyLength;
            ////					Debug.Log ("fracJourney" + fracJourney);
            ////					if (detect.isDetecting == false) {
            ////						fracJourney = 1f;
            ////					}
            //					//if(transform.Translate(playerExents + playerLoc + myExtents)  )
            //					{
            //						playerLoc = moveEnd.position - transform.position;
            //						targetStart = moveEnd;
            //
            //						playerLoc = Vector3.ClampMagnitude(playerLoc, speed);
            //						Debug.Log("enemymovetargetmoved" + playerLoc);
            //					}
            //					Debug.Log("enemymovetarget" + playerLoc);
            //
            //					transform.Translate(playerLoc);
            //
            //					//transform.position = Vector3.Lerp (moveStart.position, llama.position, fracJourney);
            //					Debug.Log ("enemypos" + transform.position);
            //					if(Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) <= eClass.attackRange){
            //						Debug.Log("tooclose");
            //						isLerping = false;}
            //					if(Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) > detect.detectSphere.radius){
            //						Debug.Log("toofar");
            //						isLerping = false;}
            ////					if (fracJourney == 1f) {
            ////						isLerping = false;
            ////					}
            //					
            //	}
            //				if (isLerping == false && Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) > detect.detectSphere.radius) {
            //
            //					Debug.Log ("edgecase");
            //					startTime = Time.time;
            //					//set end marker to be a the endpoint of a vector with a set magnitude, but a direction of the player object's transform
            ////					playerLoc = moveEnd.position;
            ////					playerLoc = Vector3.ClampMagnitude (playerLoc, speed);
            ////					llama = moveEnd;
            ////					llama.position = playerLoc;
            //					playerLoc = moveEnd.position;
            //					targetStart = moveEnd;
            //					
            //					playerLoc = Vector3.ClampMagnitude(playerLoc, speed);
            //					journeyLength = Vector3.Distance (moveStart.position, targetStart.position);
            //					isLerping = true;
            //					this.enabled = false;
            //				}
            //				else if(isLerping == false && Mathf.Abs(Vector3.Distance(this.transform.position, moveEnd.transform.position)) <= eClass.attackRange)
            //				{
            //					isLerping = true;
            //				}


        }
        else hasRun = true;
       // Debug.Log(hasRun);
    }

    //public IEnumerator EnemyRotation()
    //{
    //    Debug.Log("rotatetoface");
    //    Vector3 blahVect = Vector3.RotateTowards(transform.forward, playerCollider.transform.position - transform.position, speed * Time.deltaTime, 0.0f);

    //    //Vector3.ClampMagnitude(blahVect, detect.detectSphere.radius);
    //    Debug.DrawRay(transform.position, blahVect, Color.magenta);

    //    transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(blahVect);
    //    yield return new WaitForSeconds(0); //am i doing this right?
    //}

    //public void RayCastSwitchFunction(Ray thisRay, RaycastHit whichHit)
    //{
    //    Debug.Log(whichHit.collider.name + " " + whichHit.distance);
    //    Debug.Log(whichHit.collider.tag);
    //    if (whichHit.collider.transform.tag == "Barrier")
    //    {
    //        Debug.Log("usemeshnow");
    //        useNavmesh = true;

    //        if (!navcheckonce)
    //        {
    //            //navi.velocity = Vector3.zero;
    //            //if (navi.enabled)
    //            //{
    //            //    navi.Stop();
    //            //    navi.Resume();
    //            //    navi.ResetPath();
    //            //}
    //            navcheckonce = true;
    //        }
    //        if (navi.enabled)
    //        {
                
    //            navi.destination = (new Vector3(moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos).x, moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos).y + navi.height, moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos).z));
                
    //            rotate.canRotate = false;
    //            navi.updateRotation = true;
    //            //NavMeshPath whichPath = new NavMeshPath();
    //            //if(navi.CalculatePath(moveEnd.GetComponent<Collider>().bounds.ClosestPoint(startPos), whichPath))
    //            //{
    //            //    navi.path = whichPath;
    //            //}
    //        }
                
    //        Debug.Log(navi.destination + "navdest");
    //    }
    //}

    //public void SwitchToKinematicMovement()
    //{
    //    Debug.Log("notusingmesh");
    //    useNavmesh = false;
    //    if (navi.enabled)
    //        navi.Stop();
    //    navcheckonce = false;
    //    rotate.canRotate = true;
    //    navi.updateRotation = false;
    //}
}



//break down into components to insert into main enemy movement based off of boolean checks? No, can call with another script



