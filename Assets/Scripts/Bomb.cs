using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    GameObject player;
    int playerNumber;
    int bounces;

    public void Initialize(int p){
        playerNumber = p;
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag ("Player" + playerNumber);

        transform.position = player.transform.position + (player.transform.forward * 3);
        transform.rotation = player.transform.rotation;
        gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, 50), ForceMode.Impulse);
	}
	
	// Update is called once per frame


    void OnCollisionEnter(Collision col){
        if (col.collider.CompareTag ("Ground") || col.collider.CompareTag("Player" + playerNumber)) {
        }
        else{
            GameObject.Destroy(this.gameObject);
        }
    }
}
