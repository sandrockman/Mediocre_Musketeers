using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MountCollisionScript : MonoBehaviour
{

    public GameObject oldPlayerObj;
    public PlayerIndex playerIndex;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("collide");
        if(other.transform.tag == "Player")
        {
            //Debug.Log("player");
            if (other.transform.name == "Player_Dismounted_Control(Clone)" ||
                other.transform.name == "Player_Dismounted_Control") 
            {
                //Debug.Log("dismount");
                if(other.transform.GetComponent<XCharacterControllerDismount>().playerIndex == playerIndex)
                {
                    Debug.Log("executing...");
                    //*
                    Vector3 tempLoc = transform.position;
                    tempLoc.y += 3f;
                    oldPlayerObj.transform.position = tempLoc;
                    oldPlayerObj.GetComponent<PlayerStatScript>().Health = 
                        oldPlayerObj.GetComponent<PlayerStatScript>().maxHealth / 2f;
                    oldPlayerObj.SetActive(true);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    //*/
                }
            }
        }
    }
}