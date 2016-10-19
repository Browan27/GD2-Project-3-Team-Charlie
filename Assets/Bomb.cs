using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    private float timer;

	// Use this for initialization
	void Start () {
        timer = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
        Destroy (this, timer);
	}
}
