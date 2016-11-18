using UnityEngine;
using System.Collections;

public class Caltrop : MonoBehaviour {

    GameObject player;

    int playerNumber;

    float lifetime = 5;

    public void Initialize(int p){
        playerNumber = p;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player" + playerNumber);
        transform.position = player.transform.position - (player.transform.forward * 11);
        transform.position -= new Vector3(0,0.55f);
    }

    void Update(){
        lifetime -= 1 * Time.deltaTime;
        if (lifetime <= 0) {
            Destroy (gameObject);
        }
    }
}