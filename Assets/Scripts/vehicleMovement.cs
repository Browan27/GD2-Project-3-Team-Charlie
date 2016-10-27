using UnityEngine;
using System.Collections;
using System.IO;


public class vehicleMovement : MonoBehaviour {

    public int vehicleClass;
    public int playerNumber;

    private int hp;
    private int defaultHP;

    private float maxSpeed;
    private float speed;
    private float acceleration;
    private float rotationSpeed;
    private float translation;
    private float boostTimer;
    private float standardSFriction;
    private float standardDFriction;

    private bool onGround;
    private bool hasItem;
    private bool boost;
    private bool inOil;

    public Texture noItem;
    private Texture display;

    public GameObject BombPrefab;
    public GameObject OilPrefab;
    public GameObject MinePrefab;
    public GameObject CaltropPrefab;
    public GameObject TurretPrefab;
    public GameObject SawPrefab;
    public GameObject spawner;


    // Use this for initialization
    void Start () {
        display = noItem;
        hasItem = false;
        boost = false;
        inOil = false;
        boostTimer = 0f;
        standardDFriction = GetComponent<Collider> ().material.dynamicFriction;
        standardSFriction = GetComponent<Collider> ().material.staticFriction;

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
    }
	
    // Update is called once per frame
    void Update () {
        
        if(boost == true)
        {
            speed = maxSpeed * 2;
        }
        if (boostTimer > 0){
            boostTimer -= 1 * Time.deltaTime;
        }
        if(boostTimer < 0 && boost == true) {
            boost = false;
        }

        if (hp == 0) {
            transform.position = spawner.transform.position;
            hp = defaultHP;
        }

        //if (onGround) {
            if (speed > maxSpeed)
            {
                speed -= acceleration * Time.deltaTime * 1.25f;
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
        //}
        
        if (!Input.GetButton ("Accelerate" + playerNumber) && !Input.GetButton ("Reverse" + playerNumber) && (speed > -0.01 && speed < 0.01)) {
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
    }

    void OnCollisionEnter(Collision col){
        if(col.collider.CompareTag("Ramp")){
            transform.rotation = Quaternion.Slerp (transform.rotation, col.transform.rotation, 0);
            onGround = true;
        }
        if (col.collider.CompareTag ("Ground")) {
            onGround = true;
        }
        if (col.collider.CompareTag ("Hazard")) {
            hp -= 1;
        }
    }

    void OnCollisionExit(Collision col){
        if (col.collider.CompareTag ("Ramp") || col.collider.CompareTag ("Ground")) {
            onGround = false;
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
            speed /= 2;
        }

        if (other.gameObject.CompareTag ("Grav")) {
            gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, -30, 0), ForceMode.Impulse);
        }

        if (other.gameObject.CompareTag ("Death")) {
            transform.position = spawner.transform.position;
        }

        if (other.gameObject.CompareTag ("Oil")) {
            //inOil = true;
            speed /= 2;
            maxSpeed /= 2;
        }

        if(other.gameObject.CompareTag("Caltrop")){
            GetComponent<Collider> ().material.dynamicFriction = 0;
            GetComponent<Collider> ().material.staticFriction = 0;
        }
        if (other.gameObject.CompareTag ("Wrench")) {
            hp = defaultHP;
            GetComponent<Collider> ().material.dynamicFriction = standardDFriction;
            GetComponent<Collider> ().material.staticFriction = standardSFriction;
            other.gameObject.SetActive (false);
        }
        if (other.gameObject.CompareTag ("Saw")) {
            hp -= 1;
        }
            
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag ("Oil")) {
            //inOil = false;
            maxSpeed *= 2;
        }
    }

    void OnGUI(){
        //GUI.Label (new Rect (50, 100, 200, 200), onGround.ToString ());
        switch (playerNumber) {
        case 1:
            GUI.DrawTexture (new Rect (0, 0, 128, 80), display);
            GUI.Label(new Rect(0, 100, 200, 200), speed.ToString());
            GUI.Label(new Rect(0, 110, 200, 200), hp.ToString());
            //GUI.Label(new Rect(0, 120, 200, 200), boostTimer.ToString());
            break;
        case 2:
            GUI.DrawTexture (new Rect (Screen.width / 2, 0, 128, 80), display);
            GUI.Label(new Rect(Screen.width / 2, 100, 200, 200), speed.ToString());
            GUI.Label(new Rect(Screen.width / 2, 110, 200, 200), hp.ToString());
            //GUI.Label(new Rect(Screen.width / 2, 120, 200, 200), boostTimer.ToString());
            break;
        default:
            GUI.DrawTexture (new Rect (0, 0, 128, 80), display);
            GUI.Label(new Rect(0, 100, 200, 200), speed.ToString());
            GUI.Label(new Rect(0, 110, 200, 200), hp.ToString());
           // GUI.Label(new Rect(0, 120, 200, 200), boostTimer.ToString());
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
        default:
            break;
        
        }
    }
}