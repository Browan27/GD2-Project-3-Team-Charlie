using UnityEngine;
using System.Collections;
using System.IO;


public class vehicleMovement : MonoBehaviour {

    public int vehicleClass;
    public int playerNumber;

    private int hp;
    private int defaultHP;

    private float maxSpeed;
    private float tempSpeed;
    private float speed;
    private float acceleration;
    private float rotationSpeed;
    private float translation;
    private float boostTimer;
    private float slowTimer;
    private float invincTimer;
    private float standardSFriction;
    private float standardDFriction;

    private bool hasItem;
    private bool boost;
    private bool slow;
    private bool inOil;
    private bool isShielded;
    private bool invincible;
    public bool loser;

    private Collider item;

    public Texture noItem;
    private Texture display;

    public GameObject BombPrefab;
    public GameObject OilPrefab;
    public GameObject MinePrefab;
    public GameObject CaltropPrefab;
    public GameObject TurretPrefab;
    public GameObject SawPrefab;
    public Transform spawner;

    private GlobalController GC;
    
    public AudioClip CarAccelerating;
    public AudioClip CarDriving;
    
    private AudioSource acceleratingAS;
    private AudioSource drivingAS;

    // Use this for initialization
    void Start () {
        GC = GameObject.Find("GlobalController").GetComponent<GlobalController>();

        display = noItem;
        hasItem = false;
        boost = false;
        inOil = false;
        isShielded = false;
        invincible = false;
        loser = false;
        boostTimer = 0f;
        slowTimer = 0f;
        invincTimer = 0f;
        standardDFriction = GetComponent<Collider> ().material.dynamicFriction;
        standardSFriction = GetComponent<Collider> ().material.staticFriction;
        
        acceleratingAS = AddAudio(CarAccelerating, true, false, 0.6f);
		drivingAS = AddAudio(CarDriving, true, false, 0.6f);

        translation = 0;
        speed = 0.0f;
        switch (vehicleClass) {
        case 0:
            //Motorcycle
            maxSpeed = 6.0f;
            acceleration = 20.0f;
            rotationSpeed = 120f;
            hp = 2;
            defaultHP = hp;
            break;
        case 1:
            //Car
            maxSpeed = 1.5f;
            acceleration = 1.5f;
            rotationSpeed = 110f;
            hp = 3;
            defaultHP = hp;
            break;
        case 2:
            //Truck
            maxSpeed = 4.0f;
            acceleration = 20.0f;
            rotationSpeed = 100f;
            hp = 4;
            defaultHP = hp;
            break;
        default:
            //Assume Car
            maxSpeed = 1.5f;
            acceleration = 0.25f;
            rotationSpeed = 110f;
            hp = 3;
            break;
        }
        tempSpeed = maxSpeed;
        
        transform.position = spawner.transform.position;
    }
	
    // Update is called once per frame
    void Update () {
        if (invincTimer > 0) {
            invincTimer -= 1 * Time.deltaTime;
        }

        if (invincTimer <= 0) {
            invincible = false;
            gameObject.GetComponent<Light> ().enabled = false;
        }

        if(boost == true)
        {
            speed = maxSpeed * 2;
        }

        if (slow == true)
        {
            maxSpeed = tempSpeed / 4 * 3;
        }

        if (boostTimer > 0){
            boostTimer -= 1 * Time.deltaTime;
        }
        if(boostTimer < 0 && boost == true) {
            boost = false;
        }

        if (slowTimer > 0)
        {
            slowTimer -= 1 * Time.deltaTime;
        }
        if (slowTimer < 0 && boost == true)
        {
            slow = false;
            maxSpeed = tempSpeed;
        }

        if (hp == 0) {
            transform.position = spawner.transform.position;
            hp = defaultHP;
            loser = true;
        }
        if (speed > maxSpeed)
        {
            speed -= acceleration * Time.deltaTime * 1.25f;
        }

        if (inOil)
        {
            if(item != null)
            {
                maxSpeed = tempSpeed / 2;
                speed /= 2;
            }
            else
            {
                item = null;
                inOil = false;
                maxSpeed = tempSpeed;
            }
        }

        if (Input.GetButton ("Accelerate" + playerNumber) && speed <= maxSpeed) {
            speed += acceleration * Time.deltaTime * 3;
        } else if (speed > 0 && !Input.GetButton ("Accelerate" + playerNumber)) {
            speed -= acceleration * Time.deltaTime * 1.25f;
        }

        if (Input.GetButton("Reverse" + playerNumber) && speed >= 0) {
            speed -= acceleration * Time.deltaTime * 4;
        } else if (Input.GetButton ("Reverse" + playerNumber) && speed >= -(maxSpeed)) {
            speed -= acceleration * Time.deltaTime * 2;
        } else if (speed < 0 && !Input.GetButton ("Reverse" + playerNumber)) {
            speed += acceleration * Time.deltaTime;
        }

        if (Input.GetButton ("Accelerate" + playerNumber) || Input.GetButton ("Reverse" + playerNumber)) {
            PlayDriving();
        }
        
        if (!Input.GetButton ("Accelerate" + playerNumber) && !Input.GetButton ("Reverse" + playerNumber) && (speed > -0.01 && speed < 0.01)) {
            StopAccelerate();
            StopDriving();
        }
        
        if (!Input.GetButton ("Accelerate" + playerNumber) && !Input.GetButton ("Reverse" + playerNumber)) {
            speed = 0;
        }

        if (Input.GetButton ("Item" + playerNumber) && hasItem == true) {
            SpawnItem();
            hasItem = false;
            display = noItem;
        }

        translation = speed;
        float xRotation = Input.GetAxis ("Right Vertical" + playerNumber) * rotationSpeed * 1.5f;
        float yRotation = Input.GetAxis("Left Horizontal" + playerNumber) * rotationSpeed;
        float zRotation = Input.GetAxis("Right Horizontal" + playerNumber) * rotationSpeed * 1.5f;

        xRotation *= Time.deltaTime;
        yRotation *= Time.deltaTime;
        zRotation *= Time.deltaTime;

        gameObject.GetComponent<Rigidbody>().AddRelativeForce((translation / 2 * yRotation), 0, translation, ForceMode.Force);
        transform.Rotate(xRotation, yRotation, zRotation);
        
        if(loser) {
            gameObject.GetComponent<Light> ().enabled = true;
        }
    }
    
    public void gotSlowed() {
        slow = true;
        slowTimer = 3f;
    }
    
    public void gotSmashed() {
        hp -= 1;
    }

    void OnCollisionEnter(Collision col){
        if(col.collider.CompareTag("Ramp")){
            transform.rotation = Quaternion.Slerp (transform.rotation, col.transform.rotation, 0);
        }
        if (col.collider.CompareTag ("Hazard")) {
            if (isShielded) {
                isShielded = false;
                gameObject.GetComponent<Light> ().enabled = false;
            } else if (!invincible) {
                hp -= 1;
            }
        }
        
        for (int i = 0; i < GlobalController.numPlayers; i++) {
            GameObject player = GameObject.FindGameObjectWithTag ("Player" + (i+1));
            if(player.GetComponent<Collider>() == col.collider) {
                if(invincible) {
                    player.GetComponent<vehicleMovement>().gotSmashed();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("Pick Up") && hasItem == false)
        {
            int pick = Random.Range (0, 11);
            display = other.gameObject.GetComponent<ItemBoxRotator> ().pickups [pick];
            other.gameObject.SetActive (false);
            hasItem = true;
        }

        if (other.gameObject.CompareTag ("Boost")) {
            speed = maxSpeed * 3;
        }

        if (other.gameObject.CompareTag ("Slow Pad")) {
            slow = true;
            slowTimer = 3f;
        }

        if (other.gameObject.CompareTag ("Grav")) {
            gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, -30, 0), ForceMode.Impulse);
        }

        if (other.gameObject.CompareTag ("Death")) {
            transform.position = spawner.transform.position;
            loser = true;
        }

        if (other.gameObject.CompareTag ("Oil")) {
            if (isShielded) {
                isShielded = false;
                gameObject.GetComponent<Light> ().enabled = false;
            } else if(!invincible && !inOil){
                inOil = true;
                item = other;
            }
        }

        if(other.gameObject.CompareTag("Caltrop")){
            if (isShielded) {
                isShielded = false;
                gameObject.GetComponent<Light> ().enabled = false;
            } else if(!invincible){
                GetComponent<Collider> ().material.dynamicFriction = 0;
                GetComponent<Collider> ().material.staticFriction = 0;
            }
        }

        if (other.gameObject.CompareTag ("Wrench")) {
            hp = defaultHP;
            GetComponent<Collider> ().material.dynamicFriction = standardDFriction;
            GetComponent<Collider> ().material.staticFriction = standardSFriction;
            other.gameObject.SetActive (false);
        }

        if (other.gameObject.CompareTag ("Saw")) {
            if (isShielded) {
                isShielded = false;
                gameObject.GetComponent<Light> ().enabled = false;
            } else if (!invincible) {
                hp -= 1;
            }
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag ("Oil")) {
            inOil = false;
            maxSpeed = tempSpeed;
        }
    }

    void OnGUI(){
        switch (playerNumber) {
        case 1:
            GUI.DrawTexture (new Rect (0, 0, 128, 80), display);
            GUI.Label(new Rect(0, 100, 200, 200), speed.ToString());
            GUI.Label(new Rect(0, 110, 200, 200), hp.ToString());
            GUI.Label(new Rect(0, 120, 200, 200), tempSpeed.ToString());
            GUI.Label(new Rect(0, 130, 200, 200), maxSpeed.ToString());
            break;
        case 2:
            GUI.DrawTexture (new Rect (Screen.width / 2, 0, 128, 80), display);
            GUI.Label(new Rect(Screen.width / 2, 100, 200, 200), speed.ToString());
            GUI.Label(new Rect(Screen.width / 2, 110, 200, 200), hp.ToString());
            //GUI.Label(new Rect(Screen.width / 2, 120, 200, 200), inOil.ToString());
            break;
        }
    }

    void SpawnItem(){
        switch (display.name) {
        case "bomb":
            GameObject b = Instantiate (BombPrefab);
            b.GetComponent<Bomb> ().Initialize (playerNumber);
            break;
        case "nitro":
            speed = maxSpeed * 2;
            boost = true;
            boostTimer = 3f;
            break;
        case "oil":
            GameObject o = Instantiate (OilPrefab);
            o.GetComponent<Oil> ().Initialize (playerNumber);
            break;
        case "mine":
            GameObject m = Instantiate (MinePrefab);
            m.GetComponent<Mine>().Initialize(playerNumber);
            break;
        case "caltrop":
            GameObject c = Instantiate (CaltropPrefab);
            c.GetComponent<Caltrop>().Initialize(playerNumber);
            break;
        case "turret":
            GameObject t = Instantiate (TurretPrefab);
            t.GetComponent<Turret>().Initialize(playerNumber);
            break;
        case "saw":
            GameObject s = Instantiate (SawPrefab);
            GameObject a = Instantiate (SawPrefab);
            s.GetComponent<Saw> ().Initialize (gameObject, 2);
            a.GetComponent<Saw> ().Initialize (gameObject, -2);
            break;
        case "shield":
            isShielded = true;
            invincTimer = 5f;
            gameObject.GetComponent<Light> ().enabled = true;
            break;
        case "invinc":
            invincible = true;
            invincTimer = 5f;
            gameObject.GetComponent<Light> ().enabled = true;
            break;
        case "slow":
            for (int i = 0; i < GlobalController.numPlayers; i++) {
                GameObject player = GameObject.FindGameObjectWithTag ("Player" + (i+1));
                if(playerNumber == (i+1)) {}
                else {
                    player.GetComponent<vehicleMovement>().gotSlowed();
                }
            }
            break;
        default:
            GameObject d = Instantiate (BombPrefab);
            d.GetComponent<Bomb> ().Initialize (playerNumber);
            break;
        }
    }

	private AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip; 
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol; 
        newAudio.priority = playerNumber;
		return newAudio;
	}

	private void PlayAccelerate() {
		if(!acceleratingAS.isPlaying) {
			acceleratingAS.Play();
		}
	}
    
    private void StopAccelerate() {
		if(acceleratingAS.isPlaying) {
			acceleratingAS.Stop();
		}
	}
    
    private void PlayDriving() {
		if(!drivingAS.isPlaying) {
			drivingAS.Play();
		}
	}
    
    private void StopDriving() {
		if(drivingAS.isPlaying) {
			drivingAS.Stop();
		}
	}
}