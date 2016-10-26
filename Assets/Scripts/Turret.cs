using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    GameObject player;
    int playerNumber;

    public void Initialize(int p){
        playerNumber = p;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player" + playerNumber);
        transform.position = player.transform.position - (player.transform.forward * 2);
        transform.position -= new Vector3 (0, 1f);
    }
}
