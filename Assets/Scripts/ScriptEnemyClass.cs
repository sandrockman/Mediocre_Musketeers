using UnityEngine;
using System.Collections;

public class ScriptEnemyClass : MonoBehaviour
{
    //insert tooltip here: "Which type of enemy is this?"
    
    public enum EnemyType {BasicMelee, BasicRanged, MidMelee, MidRanged, Boss };
    //insert tooltip: "How far away can this enemy detect the player?"
    [Tooltip("Distance this enemy can detect player.")]
    public SphereCollider sightRange;
    [Tooltip("Stores the default radius of sightRange.")]
    public float baseSightRangeFloat;
    [Tooltip("Stores the current radius of sightRange.")]
    public float SightRangeFloat;
    [Tooltip("How alert is this enemy by default?")]
    public int baseAlertness = 5;
    [Tooltip("How alert is this enemy? (0-10) \nAlertness lower than 5 reduces detection and attack radii, greater than 5 increases those radii. \n0 is asleep. 10 is Omniscience. -1 means you need to set this. \nCurrently an int because I can't expose it otherwise.")]
    public int Alertness = -1;
    //public enum AlertnessState { Asleep, Zoned_Out, Tired, Busy, Distracted, Normal, Forwarned, Alert, Ready_For_Anything, High_Alert, Omniscient};
    
    //tooltip: "How far away can the enemy strike the player?"
    [Tooltip("How far away can this enemy strike the player?")]
    public SphereCollider attackRange;
    [Tooltip("Stores the default radius of attackRange.")]
    public float baseAttackRangeFloat;
    [Tooltip("Stores the current radius of attackRange.")]
    public float AttackRangeFloat;
    //tooltip
    //[Tooltip("What is the animation of the attack?")] //these two mean nothing since attacks now have indivual animations
    //public Animation attackAnimation;
    [Tooltip("What animation is played when this enemy dies?")]
    public Animation deathAnimation;
    //[Tooltip("asdf")]
    //public int attackDamage; //perhaps this is irrelevant at this point as each attack has its own damage value now; still it can be used for other things
    [Tooltip("How much health does this enemy have?")]
    public int health = 1;
    [Tooltip("How much health does this enemy start with?")]
    public int startingHealth = 100;
    [Tooltip("How much armor does this enemy have?")]
    public int armor;
    [Tooltip("How fast can this enemy move?")]
    public float speed;
    //tooltip: "How many frames of delay does this enemy have before it can react to the player?"
    [Tooltip("How many frames of delay does this enemy have before it can react to the player?")]
    public int reactionTime;
    //tooltip: "Can this enemy cancel its animations?"
    [Tooltip("Can this enemy cancel its animations?")]
    public bool canFrameCancel;

    //public Projectile[10];
    [Tooltip("This enemy's detection script:")]
    public ScriptDetectionRadius detector;
    [Tooltip("This enemy's movement script:")]
    public ScriptEnemyMovement eMove;

    //inform designers to not fuck with these variable
    [Tooltip("DO NOT EDIT! Is this enemy's movement script running?")]
    public bool isMovementRunning = false;
    [Tooltip("DO NOT EDIT! Is this enemy's attack script running?")]
    public bool isAttackRunning = false;

    [Tooltip("Holds all instances of status effects to be applied to this enemy.")]
    public ScriptStatusHolder statHold;

    public GameObject onDeathObject; //spawn this on death

    //public NavMeshAgent navi;

    
	// Use this for initialization
	void Start () {
        eMove = GetComponent<ScriptEnemyMovement>();
        detector = GetComponentInChildren<ScriptDetectionRadius>();
		health = startingHealth;
        baseAttackRangeFloat = attackRange.radius;
        baseSightRangeFloat = sightRange.radius;
        AttackRangeFloat = baseAttackRangeFloat;
        SightRangeFloat = baseSightRangeFloat;
        if(Alertness == -1) //if alertness has not been set
        Alertness = baseAlertness;
        AlertnessApplication(Alertness);
        //if(navi != null)
        {
            //AgentAssignment();
            //navi.velocity = Vector3.zero;
            //navi.Stop();
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		//wtf, this is instantly triggering...
		if (health <= -1)
        {
            DeathStuff();
        }
			
	}

    

    public IEnumerator EnemyBasicMovement()
    {
        Debug.Log("EnemyBasicMovement");
        if(isMovementRunning != true && eMove.isMovementRunning != true)
        {
            eMove.enabled = true;
        }
        //includes functionality of movement
        isMovementRunning = true;
        //this.gameObject.transform.Translate(detector.thePlayer.position);
        //do the movmement with lerping instead
        eMove.isMovementRunning = true;
        return null;
    }

    public IEnumerator EnemyBasicAttack()
    {
        //includes functionality of attacking
        isAttackRunning = true;
        return null;
    }

    public void EnemyRemoval()
    {
        //includes functionality of removing dead enemies from the game
        Destroy(this.gameObject);
    }

    public void AgentAssignment()
    {
        //navi.acceleration = 5;
        //navi.speed = speed;
        //navi.updateRotation = false;
        //navi.angularSpeed = speed;
        
    }

    void AlertnessApplication(int howAlert) //call this whenever Alertness is changed
    {
        RevertAlertnessApplication();
        switch(howAlert)
        {
            case 5: //if this enemy has the default alertness
                ApplyAndRevertAlertnessApplication(1); //pass through 1, after the reset, this will cause the radii to revert to default
                break; //exit the switch
            case 0:
                ApplyAndRevertAlertnessApplication(0.04f);
                break;
            case 1:
                ApplyAndRevertAlertnessApplication(0.1f);
                break;
            case 2:
                ApplyAndRevertAlertnessApplication(0.2f);
                break;
            case 3:
                ApplyAndRevertAlertnessApplication(0.5f);
                break;
            case 4:
                ApplyAndRevertAlertnessApplication(0.75f);
                break;
            case 6:
                ApplyAndRevertAlertnessApplication(1.25f);
                break;
            case 7:
                ApplyAndRevertAlertnessApplication(1.5f);
                break;
            case 8:
                ApplyAndRevertAlertnessApplication(1.75f);
                break;
            case 9:
                ApplyAndRevertAlertnessApplication(2);
                break;
            case 10:
                ApplyAndRevertAlertnessApplication(10);
                break;
            default:
                ApplyAndRevertAlertnessApplication(1);
                break;
        }
    }

    public void ChangeAlertness(int changeTo) //call this when you want to change alertness, and pass through the value you want alertness to become
    {
        Alertness = changeTo;
        AlertnessApplication(Alertness); //calls the AlertnessApplication function and passes the new alertness value through

    }

    public void ResetAlertness() //reverts alertness to it's base value
    {
        Alertness = baseAlertness;
    }

    public void RevertAlertnessApplication() //call this whenever you need to reset float values; do this each time you alter the radius to prevent compounding changes
    {
        SightRangeFloat = baseSightRangeFloat;
        AttackRangeFloat = baseAttackRangeFloat;
    }

    void ApplyAndRevertAlertnessApplication(float modifier)
    {
        sightRange.radius = SightRangeFloat;
        sightRange.radius *= modifier;
        //Debug.Log(baseAttackRangeFloat + "baseattackrangefloat" + modifier + "modifier");
        
        if(baseAttackRangeFloat > 4 && modifier > 1) //if this is a ranged enemy and the modifier would increase the attack range
        {
            attackRange.radius = AttackRangeFloat;
            attackRange.radius *= modifier;
        }
        
    }

    public void DefaultAlertness() //sets Alertness to a global default of 5
    {
        ChangeAlertness(5);
    }

    public void DefaultBaseAlertness() //changes the baseAlertness to the global default of 5; used when baseAlertness starts out as a different value
    {
        baseAlertness = 5;
        DefaultAlertness();
    }

    public void InstaKill()
    {
        health = -1;
    }

    public void DeathStuff()
    {
        Destroy(this.gameObject);
        Instantiate(onDeathObject);
    }
}
