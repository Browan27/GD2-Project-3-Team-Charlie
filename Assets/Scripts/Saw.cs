using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour {

    GameObject player;

    float lifetime = 5;
    float rotationSpeed = 90;
    float space;

    public void Initialize(GameObject p, float s){
        player = p;
        space = s;
    }

    // Use this for initialization
    void Start () {
        transform.parent = player.transform;
        transform.position = player.transform.position - (player.transform.forward * space);
    }

    void Update(){
        transform.RotateAround (player.transform.position, new Vector3 (0, 1, 0), rotationSpeed * Time.deltaTime);
        lifetime -= 1 * Time.deltaTime;
        if (lifetime <= 0) {
            Destroy (gameObject);
        }
    }
}
