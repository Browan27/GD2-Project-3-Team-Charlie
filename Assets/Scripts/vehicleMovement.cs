using UnityEngine;
using System.Collections;
using System.IO;


public class vehicleMovement : MonoBehaviour {

    public int vehicleClass;
    public int playerNumber;

    private float maxSpeed;
    private float speed;
    private float acceleration;
    private float rotationSpeed;
    private float translation;


    private bool hasItem = false;
    //public Texture item = "question";
    public Texture noItem;
    private Texture display;

    // Use this for initialization
    void Start () {
        display = noItem;

        translation = 0;
        speed = 0.0f;
        switch (vehicleClass) {
        case 0:
            //Motorcycle
            maxSpeed = 6.0f;
            acceleration = 20.0f;
            rotationSpeed = 120f;
            break;
        case 1:
            //Car
            maxSpeed = 1.0f;
            acceleration = 0.1f;
            rotationSpeed = 110f;
            break;
        case 2:
            //Truck
            maxSpeed = 4.0f;
            acceleration = 20.0f;
            rotationSpeed = 100f;
            break;
        default:
            //Assume Car
            maxSpeed = 5.0f;
            rotationSpeed = 110f;
            break;
        }
    }
	
    // Update is called once per frame
    void Update () {


        if (Input.GetButton ("Accelerate" + playerNumber) && speed <= maxSpeed) {
            speed += acceleration * Time.deltaTime;
        } else if (speed > 0 && !Input.GetButton ("Accelerate" + playerNumber)) {
            speed -= acceleration * Time.deltaTime;
        } 

        if (Input.GetButton ("Reverse" + playerNumber) && speed >= -maxSpeed) {
            speed -= acceleration * Time.deltaTime;
        } else if (speed < 0 && !Input.GetButton ("Reverse" + playerNumber)) {
            speed += acceleration * Time.deltaTime;
        }

        if (!Input.GetButton ("Accelerate" + playerNumber) && !Input.GetButton ("Reverse" + playerNumber) && (speed > -0.01 && speed < 0.01)) {
            speed = 0;
        }

        if (Input.GetButton ("Item" + playerNumber)) {
            hasItem = false;
            display = noItem;
        }

        translation = speed;
        float xRotation = Input.GetAxis ("Right Vertical") * rotationSpeed;
        float yRotation = Input.GetAxis("Left Horizontal") * rotationSpeed;
        float zRotation = Input.GetAxis("Right Horizontal") * rotationSpeed;

        xRotation *= Time.deltaTime;
        yRotation *= Time.deltaTime;
        zRotation *= Time.deltaTime;

        //transform.Translate(0, 0, translation);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, translation, ForceMode.Force);
        transform.Rotate(xRotation, yRotation, zRotation);
    }

    void OnCollisionEnter(Collision col){
        if(col.collider.CompareTag("Ramp")){
            transform.rotation = Quaternion.Slerp (transform.rotation, col.transform.rotation, 0);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("Pick Up") && hasItem == false)
        {
            int pick = Random.Range (0, 7);
            display = other.gameObject.GetComponent<ItemBoxRotator> ().pickups [pick];
            //display = Resources.Load("Textures/" + item + ".png", typeof(Texture)) as Texture;
            other.gameObject.SetActive (false);
            hasItem = true;
        }

        if (other.gameObject.CompareTag ("Boost")) {
            speed *= 2;
        }

        if (other.gameObject.CompareTag ("Slow Pad")) {
            speed /= 2;
        }

        if (other.gameObject.CompareTag ("Grav")) {
            gameObject.GetComponent<Rigidbody> ().useGravity = !gameObject.GetComponent<Rigidbody> ().useGravity;
        }

    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag ("Boost")) {
            speed /= 2;
        }

        if (other.gameObject.CompareTag ("Slow Pad")) {
            speed *= 2;
        }
    }

    void OnGUI(){
        GUI.Label(new Rect(50, 50, 200, 200), speed.ToString());
        //GUI.DrawTexture(new Rect(0, 0, 128, 80), display);
    }

}