using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//do not want to run this script if we do not have a cc controller on the obj
[RequireComponent(typeof(CharacterController))]


public class PlayerController : MonoBehaviour
{

    public float moveSpeed;//10 is good
    private float forwardSpeed;
    private float strafeSpeed;

    private Vector3 speed;
    public Vector3 displaySpeed;

    private CharacterController cC;

    //limit of downmovement
    public float upDownLimit;//60 good start
    private float desiredUpDown;
    private float rotYaw;
    private float rotRoll;
    private Camera camera;

    public float mouseSensitivity;

    public float jumpSpeed;//5-7
    private float verticalVelocity;

    public bool jumped;

    public bool cCisGrounded;

    // track if the player is sprinting or not
    public bool isSprinting;

    // keep track of remaining time to sprint
    public float sprintCounter;

    // keep track of remaining time to wait
    public float sprintWaitCounter;

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1); // crouchScale is for when the object is crouching (half height)
    private Vector3 playerScale = new Vector3(1, 1f, 1); // playerScale is for when the object is standing (normal height)

    public Vector3 respawnPos;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();

        camera = Camera.main;

        jumped = false;

        // initialize sprint and wait times to default values
        sprintCounter = 5f;
        sprintWaitCounter = 0f;

        respawnPos = transform.position;

        levelManager = FindObjectOfType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.x = Screen.width / 2;
        mousePos.y = Screen.height / 2;
        Cursor.lockState = CursorLockMode.Locked;

        //want to check for movement each frame
        MoveAndRotate();
        cCisGrounded = cC.isGrounded;

        if (Input.GetButtonDown("Fire2"))
        {
            Crouch(true); //If Fire2 button is pressed, it sets crouch to true and runs the method, which makes the player crouch
        }

        if (Input.GetButtonUp("Fire2"))
        {
            Crouch(false); //If Fire2 button is pressed again, it sets crouch to false and undoes the Crouch method, which makes the player stand up
        }

        displaySpeed = speed;

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillPlane")
        {
            Debug.Log("here");
            //call our respawn method
            levelManager.Respawn();

        }//*/


        if (other.tag == "Checkpoint")
        {
            respawnPos = other.transform.position;
        }
    }


    //want to check buttons being pushed

    void MoveAndRotate()
    {

        rotYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0f, rotYaw, 0f);

        rotRoll = Input.GetAxis("Mouse Y") * mouseSensitivity;
        desiredUpDown -= rotRoll;

        desiredUpDown = Mathf.Clamp(desiredUpDown, -upDownLimit, upDownLimit);

        camera.transform.localRotation = Quaternion.Euler(desiredUpDown, 0f, 0f);

        forwardSpeed = Input.GetAxis("Vertical") * moveSpeed;

        strafeSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;


        if (cC.isGrounded && Input.GetButtonDown("Jump"))
        {
            //vertical
            verticalVelocity = jumpSpeed;
            jumped = true;
        }

        if (!cC.isGrounded && Input.GetButtonDown("Jump") && jumped)
        {
            verticalVelocity = jumpSpeed;
            jumped = false;
        }



        speed = new Vector3(strafeSpeed, verticalVelocity, forwardSpeed);

        speed = transform.rotation * speed;

        //cc has 2 movements
        //simplemove and move
        //we start with simplemove; we will switch to move later

        cC.Move(speed * Time.deltaTime);

        // currently grounded and pressing sprint
        if (cC.isGrounded && Input.GetButton("Fire3"))
        {
            // call the sprint function because the player is trying to sprint
            Sprint();
        }
        else // jumping or not pressing sprint
        {
            // the player isn't trying to sprint
            if (isSprinting)
            {
                // the player stopped pressing sprint but is still marked as sprinting, this means they stopped early
                // so we need to stop the player from sprinting and start the 1.5 second timer
                isSprinting = false;
                sprintCounter = 0f;
                sprintWaitCounter = 1.5f;
            }
            else
            {
                // we aren't sprinting and haven't been sprinting
                // we need to decrement the wait timer in case the 1.5 second timer was started
                // if this keeps going below 0 it won't matter since it will be reset anyway
                // this also doesn't overlap with the decrement in the sprint method
                // because this can only happen when sprint isn't being called
                sprintWaitCounter -= Time.deltaTime;
            }
        }


    }

    // called when the player presses sprint
    void Sprint()
    {
        if (sprintCounter <= 0f)
        {
            // the sprint timer reached 0 so the player ran out of time to sprint
            // they need to be stopped even though they still want to sprint
            if (isSprinting) // currently are sprinting, need to stop
            {
                // stop player from sprinting and start the wait timer to 3 seconds
                // they kept going too long so they need to wait the full 3 seconds, not 1.5
                isSprinting = false;
                sprintWaitCounter = 3f;
                sprintCounter = 0f;
            }
        }

        // the wait timer reached 0 so now the player is allowed to sprint
        if (sprintWaitCounter <= 0f)
        {
            // the player sprinting needs to start since they want to sprint and they have waited long enough
            if (!isSprinting) // not sprinting yet, need to start
            {
                // start player sprinting and set the sprint timer to 5 seconds to allow them to sprint for some time
                isSprinting = true;
                sprintCounter = 5f;
                sprintWaitCounter = 0f;
            }
        }

        // if the sprint timer is above 0 it needs to tick down
        if (sprintCounter > 0f)
        {
            // decrease the timer, Time.deltaTime works since Sprint is called every frame
            sprintCounter -= Time.deltaTime;
        }

        // if the wait timer is above 0 it needs to tick down
        if (sprintWaitCounter > 0f)
        {
            // decrease the timer, Time.deltaTime works since Sprint is called every frame
            sprintWaitCounter -= Time.deltaTime;
        }

        // if we are sprinting we need to change the player's speed
        if (isSprinting)
        {
            //the speed should be multiplied by 1.5 overall to make the player faster
            speed = new Vector3(100, verticalVelocity, 100);
            Debug.Log("speed" + speed);
        }
    }

    void Crouch(bool crouching)
    {
        //takes if we want to crouch or not
        if (crouching)
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z); //this is a function which transforms the y positon of player by -0.5f
        }
        else
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); //this is a function which transforms the y position of player by +0.5f
        }
    }



}
