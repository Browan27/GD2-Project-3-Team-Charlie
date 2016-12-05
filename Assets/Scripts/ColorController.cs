using UnityEngine;
using System.Collections;

public class ColorController : MonoBehaviour {
    public Color[] colors = new Color[4];
    
	// Use this for initialization
	void Start () {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", colors[transform.parent.GetComponent<vehicleMovement>().playerNumber-1]);
	}
}
