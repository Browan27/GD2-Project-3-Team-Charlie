using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    
    // Use this for initialization rect(x,y,w,h)
	void Start () {
        float x = 0f;
        float w = 1f;
        float y = 0f;
        float h = 1f;
        
        if(GlobalController.numPlayers > 2) {
            h = 0.5f;
            if(transform.parent.GetComponent<vehicleMovement>().playerNumber > 2) { y = 0; }
            else { y = 0.5f; }
        }
        
        if(transform.parent.GetComponent<vehicleMovement>().playerNumber % 2 == 0) { x = 0.5f; }
        else { x = -0.5f; }
        
        GetComponent<Camera>().rect = new Rect(x,y,w,h);
    }
}
