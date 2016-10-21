using UnityEngine;
using System.Collections;

public class BoxRespawner : MonoBehaviour {

    public GameObject box;
    private bool isActive = true;

	// Use this for initialization
	void Start () {
        InvokeRepeating ("RespawnBox", 1, 10);
	}

    void Update(){
        if (box.activeSelf == false) {
            isActive = false;
        }
    }
	
    void RespawnBox(){
        if (isActive == false) {
            box.gameObject.SetActive (true);
            isActive = true;
        }
    }
}
