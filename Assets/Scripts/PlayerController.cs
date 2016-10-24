using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void respawnPlayer() {
        if(player == null)
        {
            Instantiate(player);
        }
    }
}
