using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player_2C : MonoBehaviour {
    // Create public variables for player speed, and for the Text UI game objects
    public float speed;
    public Text countText;
    public Text winText;

    public Text velText;
    public Text accelText;

    public Text reStart;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
    private int count;

    public AudioSource sound;

    //used to determine size of platform
    private float momenSize = 0;

    public AudioSource sound_2;

    public float rbvelocity; //current velocity of rigidbody
    public float rbaccel; //acceleration of rigidbody

    public GameObject platPrefab;
    public GameObject aPlatPrefab;
    public GameObject vPlatPrefab;

    public float lastVelocity;
    Vector3 movement;
    public bool inAir = false;
    // At the start of the game..
    void Start()
    {

        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();

        //gets initial velocity
        rbvelocity = rb.velocity.magnitude;
        rbaccel = 0.0f;
        // Set the count to zero 
        count = 0;

        // Run the SetCountText function to update the UI (see below)
        SetCountText();

        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";
        reStart.text = "";
    }

    // Each physics step..
    void FixedUpdate()
    {


        moveMent();


        rbvelocity = rb.velocity.magnitude;

        //acceleration
        rbaccel = (rbvelocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = rbvelocity;

        SetCountText();

    }

    void Update()
    {

        //make platform ability
        if (Input.GetKeyDown(KeyCode.Slash))
        {

            makePlat();

            //rb.AddForce(-movement * speed * 100);

            //rb.AddForce(-100.0f, 0.0f, -100.0f);



        }

        if (rbvelocity < 54)
        {
            return;
        }
        else
        {
            transform.position = new Vector3(-20, -13, 5);
        }

        if (Input.GetButtonDown("Escape"))
        {

            SceneManager.LoadScene("_Scene_0");

        }

    }


    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);

            if (rbvelocity < 6)
            {

                // Add one to the score variable 'count'
                count = count + 1;
            }
            else if( rbvelocity > 6 && rbvelocity < 17)
            {
                count = count + 2;
            }
            else
            {
                count = count + 3;
            }

            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }
    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        // Update the text field of our 'countText' variable
        countText.text =  count.ToString();

        //velText
        velText.text = "velocity: " + System.Math.Round(rbvelocity, 1).ToString();

        //accelText 
        accelText.text = "Accel: " + System.Math.Round(rbaccel, 1).ToString();

       



        // Check if our 'count' is equal to or exceeded 12
        if (count >= 10)
        {
            // Set the text value of our 'winText'
            winText.text = "Player 2 Wins!";
            reStart.text = "[R]estart";

            GameObject[] kill = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject kGO in kill)
            {
                kGO.SetActive(false);
            }

        }
    }


    void makePlat()
    {                                                        // b

        sound.Play();

        GameObject platGO = Instantiate<GameObject>(platPrefab);

        platGO.transform.position = transform.position;

        rb.AddForce(-movement * rbvelocity * 130f);


    }

    void makeAplat()
    {                                                        // b
        if (rbvelocity > 9.0f)
        {
            momenSize = rbvelocity * Mathf.Sin(rbvelocity / 30 * (Mathf.PI / 2));


            GameObject platGO = Instantiate<GameObject>(aPlatPrefab);
            platGO.transform.localScale += Vector3.one * (momenSize);

            platGO.transform.position = transform.position;

            //Vector3 tempVect = rb.velocity * rbvelocity;
            rb.isKinematic = true;
            transform.position = platGO.transform.position + (Vector3.up * Mathf.Abs(momenSize));
            rb.isKinematic = false;

            rb.AddForce(-movement * momenSize);
        }
        else
        {
            return;
        }






        //



    }

    void makeVplat()
    {                                                        // b

        GameObject platGO = Instantiate<GameObject>(vPlatPrefab);

        platGO.transform.position = transform.position;



    }

    void moveMent()
    {
        if (rb.velocity.y == 0)
        {
            inAir = false;
        }

        float moveHorizontal = Input.GetAxis("Horizontal_2");
        float moveVertical = Input.GetAxis("Vertical_2");

        if (Input.GetButtonDown("Jump_2") && !(inAir))
        {

            sound_2.Play();
            //the cube is going to move upwards in 10 units per second
            rb.velocity = new Vector3(0, 7.5f, 0);
            inAir = true;

        }





        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above


        if (!inAir)
        {
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        }





        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        rb.AddForce(movement * speed);

    }
}

