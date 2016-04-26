using UnityEngine;
using System.Collections;

public class ScriptEnemyRotation : MonoBehaviour {
    [Tooltip("The collider of the targeted player.")]
    public Collider playerCollider;
    [Tooltip("How fast can this enemy rotate?")]
    public float speed = 0;
    [Tooltip("Can this enemy rotate currently?")]
    public bool canRotate = false;
    [Tooltip("This enemy's detection script.")]
    public ScriptDetectionRadius detect;
    //public NavMeshAgent navi;
    // Use this for initialization
    void Enabled()
    {
        Debug.Log("canRotate" + gameObject.name + " " + canRotate);
    }

    void Start () {
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(canRotate)
        {
            //Debug.Log("rotatetoface");
            Vector3 blahVect = Vector3.RotateTowards(transform.forward, playerCollider.transform.position - transform.position, speed * Time.deltaTime, 0.0f);

            //Vector3.ClampMagnitude(blahVect, detect.detectSphere.radius);
            Debug.DrawRay(transform.position, blahVect, Color.magenta);

            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(blahVect);
            
        }
        
         
    }
}
