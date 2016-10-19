using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        //Vector3 foward = new Vector3 (0, 0, 5);
        //foward = gameObject.transform.InverseTransformPoint(foward);

        player = GameObject.FindGameObjectWithTag ("Player");
        //float playerRotation = player.gameObject.transform.rotation.y;
        //Quaternion bombRotation = new Quaternion (0, playerRotation, 0, 0);

        transform.position = player.transform.position + (player.transform.forward * 2);
        transform.rotation = player.transform.rotation;
        gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, 50), ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
