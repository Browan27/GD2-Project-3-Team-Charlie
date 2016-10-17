using UnityEngine;
using System.Collections;

public class ItemBoxRotator : MonoBehaviour {
    public Texture[] pickups = new Texture[8];

    void Update () 
    {
        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
    }
}
