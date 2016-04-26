using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAttackScript : MonoBehaviour {

    public PlayerIndex playerIndex;

    //public GameObject forwardAttackBox;
    public GameObject lanceAttackBox;
    public GameObject leftSideAttackBox;
    public GameObject rightSideAttackBox;
    public GameObject trampleAttackBox;
    public GameObject shieldSphere;

    //will probably be replaced with mechanim
    public float forwardAttackTime = 0.5f;
    public float lanceAttackTime = 1.0f;
    public float sideAttackTime = 0.5f;
    public float shieldTime = 2.0f;

    //attack cooldown time.
    public float cooldownTime = 0.7f;

    public bool canAttack { get; private set; }

    // Use this for initialization
    void Start () {
        canAttack = true;
        //forwardAttackBox.SetActive(false);
        lanceAttackBox.SetActive(false);
        leftSideAttackBox.SetActive(false);
        rightSideAttackBox.SetActive(false);
        trampleAttackBox.SetActive(false);
        if(shieldSphere != null)
            shieldSphere.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void StartLanceAttack()
    {
        StartCoroutine("LanceAttack");
    }

    IEnumerator LanceAttack()
    {

        canAttack = false;
        lanceAttackBox.SetActive(true);
        yield return new WaitForSeconds(lanceAttackTime);
        lanceAttackBox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    public void StartLeftSideAttack()
    {
        StartCoroutine("LeftSideAttack");
    }

    IEnumerator LeftSideAttack()
    {
        canAttack = false;
        leftSideAttackBox.SetActive(true);
        yield return new WaitForSeconds(sideAttackTime);
        leftSideAttackBox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    public void StartRightSideAttack()
    {
        StartCoroutine("RightSideAttack");
    }

    IEnumerator RightSideAttack()
    {
        canAttack = false;
        rightSideAttackBox.SetActive(true);
        yield return new WaitForSeconds(sideAttackTime);
        rightSideAttackBox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    public void StartShield()
    {
        StartCoroutine("ShieldMove");
    }

    IEnumerator ShieldMove()
    {
        canAttack = false;
        if(shieldSphere != null)
            shieldSphere.SetActive(true);
        yield return new WaitForSeconds(sideAttackTime);
        shieldSphere.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    /*
    IEnumerator ForwardAttack()
    {
        canAttack = false;
        forwardAttackBox.SetActive(true);
        yield return new WaitForSeconds(forwardAttackTime);
        forwardAttackBox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }
    //*/

    public void TrampleAttackStart()
    {
        //canAttack = false;
        trampleAttackBox.SetActive(true);
        //yield return new WaitForSeconds(lanceAttackTime);
        //trampleAttackBox.SetActive(false);
        //yield return new WaitForSeconds(cooldownTime);
        //canAttack = true;
    }

    public void TrampleAttackEnd()
    {
        trampleAttackBox.SetActive(false);
    }
}
