using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

    GameObject player;
    int playerNumber;

    public void Initialize(int p){
        playerNumber = p;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player" + playerNumber);
        transform.position = player.transform.position - (player.transform.forward * 6);
        //transform.position -= new Vector3 (0, f);
    }

    void OnCollisionEnter(Collision col){
        col.rigidbody.AddExplosionForce (50, transform.position, 20);
        GameObject.Destroy(this.gameObject);
    }
}
