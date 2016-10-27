using UnityEngine;
using System.Collections;

public class WrenchRespawner : MonoBehaviour {

    public GameObject box;
    private bool isActive = true;

    // Use this for initialization
    void Start () {
        InvokeRepeating ("RespawnWrench", 1, 10);
    }

    void Update(){
        if (box.activeSelf == false) {
            isActive = false;
        }
    }

    void RespawnWrench(){
        if (isActive == false) {
            box.gameObject.SetActive (true);
            isActive = true;
        }
    }
}
