using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    
    private Text data;
    private GameObject DC;

	// Use this for initialization
	void Start () {
        data = GetComponent<Text>();
        DC = GameObject.Find("DataController");
	}
	
	// Update is called once per frame
	void Update () {
        data.text = "" + DC.GetComponent<DataController>().getPlayers();
	}
}
