using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {
    
    public static int numPlayers;
    public GameObject player;
    public GameObject[] players;
    public Transform[] playerSpawns = new Transform[4];
    public bool gameOver;

	// Use this for initialization
	void Start () {
        numPlayers = 2;
        setPlayers();
    }
	
	// Update is called once per frame
	void Update () {
        //respawnPlayer();
	}

    void setPlayers()
    {
        players = new GameObject[numPlayers];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = player;
            players[i].GetComponent<vehicleMovement>().playerNumber = i+1;
            players[i].GetComponent<vehicleMovement>().spawner = playerSpawns[i];
            players[i].name = "Player" + (i+1);
            players[i].tag = "Player" + (i+1);
            Instantiate(players[i], playerSpawns[i]);
        }
    }
    

    void respawnPlayer() {
        for(int i = 0; i < players.Length; i++)
        {
            if(!GameObject.Find("Player" + (i+1)))
            {
                Instantiate(players[i], playerSpawns[i]);
            }
        }
    }

    public GameObject[] globalUsedBy(int playerNumber)
    {
        GameObject[] array = new GameObject[players.Length - 1];
        int k = 0;
        for (int i = 1; i < players.Length; i++)
        {
            if (i == playerNumber) { }
            else
            {
                array[k++] = players[i];
            }
        }
        return array;
    }
}
