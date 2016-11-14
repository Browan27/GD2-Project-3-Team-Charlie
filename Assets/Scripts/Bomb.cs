using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    GameObject player;
    int playerNumber;
	public GameObject Explosion;

    public void Initialize(int p){
        playerNumber = p;
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag ("Player" + playerNumber);

        transform.position = player.transform.position + (player.transform.forward * 7);
        transform.rotation = player.transform.rotation;
        gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, 50), ForceMode.Impulse);
	}
	
	// Update is called once per frame


    void OnCollisionEnter(Collision col){
        if (col.collider.CompareTag ("Ground") || col.collider.CompareTag("Player" + playerNumber)) {
			GameObject explosion = (GameObject)Instantiate (Explosion, transform.position, transform.rotation);
			Destroy (explosion, 3);
        }
        else{
            GameObject.Destroy(this.gameObject);
        }
    }
}
