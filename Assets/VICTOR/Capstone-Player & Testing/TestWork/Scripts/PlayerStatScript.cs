using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerStatScript : MonoBehaviour {

    public enum StatusEffects{NONE, DAZED, PARALYZED, POISONED, BLEEDING};

    public float Health { get; set; }
    public float AttackStrength { get; private set; }
    public StatusEffects Status { get; private set; }

    public float initialHealth = 10;
    public float maxHealth = 10;
    public float initAtkStr = 1;
    public StatusEffects initStat = StatusEffects.NONE;
    public bool healthBoost = false;
    public bool fakeDeath = false;

    public GameObject newPlayerControl;
    public GameObject newPlayerMount;

    public GameObject bleedEffect;

    public bool isOnMount;

    // Use this for initialization
    void Awake () {
        Health = initialHealth;
        Status = initStat;
        AttackStrength = initAtkStr;
        if (bleedEffect != null)
            bleedEffect.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    if(Health <= 0)
        {
            DeathFunction();
        }
        if(!healthBoost)
        {
            if(Health > maxHealth)
            {
                Health = maxHealth;
            }
        }
        if(fakeDeath)
        {
            fakeDeath = false;
            DeathFunction();
        }
	}

    void DeathFunction()
    {
        
        //death script
        Debug.Log("Player Died");
        if (isOnMount)
        {
            if (newPlayerControl != null)
            {
                Vector3 tempLoc = transform.position;
                tempLoc.y += 3f;
                tempLoc.z -= 3f;
                GameObject newPlayer = Instantiate(newPlayerControl, tempLoc, Quaternion.identity) as GameObject;
                PlayerIndex pIndex = GetComponent<XCharacterControllerLancer>().playerIndex;
                newPlayer.GetComponent<XCharacterControllerDismount>().playerIndex = pIndex;
                newPlayer.transform.parent = GameObject.Find("PlayerContainer").transform;
                newPlayer.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -8f);
                if(newPlayerMount != null)
                {
                    GameObject newMount = Instantiate(newPlayerMount, transform.position, Quaternion.identity) as GameObject;
                    newMount.GetComponent<MountCollisionScript>().oldPlayerObj = this.gameObject;
                    newMount.GetComponent<MountCollisionScript>().playerIndex = pIndex;
                    newMount.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 15f);
                }
                this.gameObject.SetActive(false);
            }
        }
        else //dismounted
        {
            GameObject.Find("Main Camera").GetComponent<LevelPlayerSetupScript>().LoseGame();
        }
    }

    //minor change here for testing my code; Jason
    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

    void TakeHealing(float hlth)
    {
        Health += hlth;
    }

    void ChangeStatus(StatusEffects newStat)
    {
        Status = newStat;
    }
}
