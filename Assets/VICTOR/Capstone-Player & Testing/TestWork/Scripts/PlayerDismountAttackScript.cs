using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerDismountAttackScript : MonoBehaviour {

    public PlayerIndex playerIndex;

    public GameObject frontAttackBox;
   
    //will probably be replaced with mechanim
    public float forwardAttackTime = 0.5f;
    
    //attack cooldown time.
    public float cooldownTime = 0.7f;

    public bool canAttack { get; private set; }

    // Use this for initialization
    void Start()
    {
        canAttack = true;
        //forwardAttackBox.SetActive(false);
        frontAttackBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAttack()
    {
        StartCoroutine("ForwardAttack");
    }

    IEnumerator ForwardAttack()
    {

        canAttack = false;
        frontAttackBox.SetActive(true);
        yield return new WaitForSeconds(forwardAttackTime);
        frontAttackBox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }
}
