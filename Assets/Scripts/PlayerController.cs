using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject player;
    public int playerNumber;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        //respawnPlayer ();
	}

    void respawnPlayer() {
        if(!GameObject.Find("Player" + playerNumber))
        {
            Instantiate(player);
        }
    }
}
