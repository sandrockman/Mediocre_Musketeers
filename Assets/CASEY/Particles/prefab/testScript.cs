using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour {

    public GameObject player;

    public Vector3 startRot;

    public float playerRotY;

    // Use this for initialization
    void Start () {
        startRot = GetComponent<ParticleSystem>().startRotation3D;

	}
	
	// Update is called once per frame
	void Update () {
        startRot = GetComponent<ParticleSystem>().startRotation3D;

        playerRotY = player.transform.eulerAngles.y;
        playerRotY = (playerRotY < 180f ? playerRotY + 180f : playerRotY - 180f);
        playerRotY = playerRotY * Mathf.PI / 180f;
        startRot.y = playerRotY ;
        GetComponent<ParticleSystem>().startRotation3D = startRot;
        
	}
}
