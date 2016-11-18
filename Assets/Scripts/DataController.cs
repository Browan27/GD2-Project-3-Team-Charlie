using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {
    private int numPlayers;
    
    
	void Start () {
        DontDestroyOnLoad(this);
        numPlayers = 2;
	}
    
    public void setPlayers(int p) {
        numPlayers = p;
    }
	
    public int getPlayers() {
        return numPlayers;
    }
}
