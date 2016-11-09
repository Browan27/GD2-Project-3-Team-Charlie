using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {
    
    public static int numPlayers;
    public GameObject player;
    public GameObject[] players;
    public Transform[] playerSpawns = new Transform[4];
    public bool gameOver;
    private float countdown;
    private int victor;

	// Use this for initialization
	void Start () {
        numPlayers = 2;
        setPlayers();
        countdown = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if(!gameOver) { countLosers(); }
        if(countdown > 0 && gameOver) {
            countdown -= 1 * Time.deltaTime;
        } else if(countdown < 0 && gameOver) {
            reset();
        }
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
            players[i].transform.position += Vector3.up * (i * 10.0f);
        }
    }
    
    private void countLosers() {
        int loserCount = 0;
        for (int i = 0; i < players.Length; i++)
        {
            player = GameObject.FindGameObjectWithTag ("Player" + (i+1));
            if(player.GetComponent<vehicleMovement>().loser) {
                loserCount += 1;
            }
        }
        if(loserCount == numPlayers-1) {
            declareWinner();
        }
    }
    
    private void declareWinner() {
        victor = 0;
        
        for (int i = 0; i < players.Length; i++)
        {
            player = GameObject.FindGameObjectWithTag ("Player" + (i+1));
            if(!player.GetComponent<vehicleMovement>().loser) {
                victor = i+1;
            }
        }
        
        gameOver = true;
        countdown = 5f;
    }
    
    private void reset() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
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
    
    void OnGUI(){
        if(gameOver) {
            GUI.Label(new Rect(0, 100, 200, 200), "Player " + victor + " is victorious!");
        }
    }
}
