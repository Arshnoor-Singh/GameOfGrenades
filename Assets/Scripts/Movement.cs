using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class Movement : MonoBehaviour
{
    public Camera mainCam;
    public GameObject Model;
    public Rigidbody ModelRB;
    public float gravity = 50f;
    public float jumpSpeed = 15f;
    public float speed = 5f;
    

    CharacterController playerControl;
    Rigidbody playerPhysics;
    Animator Anim;


    Vector3 jump = Vector3.zero;
    public Vector3 move;

    float mouseX;
    float mouseY;
    float GamePadX;
    float GamePadY;
    float currYDir = 0, prevYDir = 0;

    bool canThrow = true;
    bool canJump = true;
    bool canSlide = true;
    bool canDive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<CharacterController>();
        playerPhysics = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
    }


    public void playerMovement()
    {
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && !Anim.GetBool("throw"))
        {

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            move = transform.right * x + transform.forward * z;

            Vector3 playerMove = move;

            Model.transform.forward = move;

            playerControl.SimpleMove(playerMove * speed);
        }

    }

    public void playerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerControl.isGrounded)
        {
            //GetComponent<Animator>().Play("Jump");
            jump.y = jumpSpeed; 
        }

        jump.y -= gravity * Time.deltaTime;
       
        playerControl.Move(jump * Time.deltaTime);
    }

    public void playerLookAround()
    {
        //Mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = -(Input.GetAxis("Mouse Y"));

        // Vertical mouse look direction
        currYDir = prevYDir + mouseY;
        currYDir = Mathf.Clamp(currYDir, -20f, 0f);
        //mainCam.transform.Rotate(currYDir - prevYDir, 0, 0);
        prevYDir = currYDir;
        // Horizontal mouse look direction
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetJoystickNames() != null)
        {
            //Gamepad
            GamePadX = Input.GetAxis("Gamepad X");
            GamePadY = -(Input.GetAxis("Gamepad Y"));

            // Vertical mouse look direction
            currYDir = prevYDir + GamePadY;
            currYDir = Mathf.Clamp(currYDir, -20f, 0f);
            mainCam.transform.Rotate(currYDir - prevYDir, 0, 0);
            prevYDir = currYDir;
            // Horizontal mouse look direction
            transform.Rotate(Vector3.up * GamePadX);
        }
    }

    public void playerAnimation()
    {
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && !Anim.GetBool("throw") && !Anim.GetBool("jump") && !Anim.GetBool("slide") && !Anim.GetBool("run-dive"))
        {
           // Anim.applyRootMotion = false;
            Anim.SetBool("run", true);
            Anim.Play("Run");
        }
        else
        {
            Anim.SetBool("run", false);
            //Anim.applyRootMotion = true;
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && Anim.GetBool("run"))
        {
            Anim.Play("Running Jump");
        }

        if (Input.GetButtonDown("Fire1") && canThrow)
        {
            canThrow = false;
            canJump = false;
            canSlide = false;
            canDive = false;
            Anim.SetBool("throw", true);
            Anim.Play("Throw");
        }

        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canThrow = false;
            canJump = false;
            canSlide = false;
            canDive = false;
            Anim.SetBool("jump", true);
            Anim.Play("Running Jump");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canSlide && Anim.GetBool("run"))
        {
            canThrow = false;
            canJump = false;
            canSlide = false;
            canDive = false;
            Anim.SetBool("slide", true);
            Anim.Play("Running Slide");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canDive && Anim.GetBool("run"))
        {
            canThrow = false;
            canJump = false;
            canSlide = false;
            canDive = false;
            Anim.SetBool("run-dive", true);
            //Anim.SetBool("dive-run", false);
            Anim.Play("Stand To Roll");
        }

    }

    public void CanDive()
    {
        Anim.SetBool("run-dive", false);
        canThrow = true;
        canJump = true;
        canSlide = true;
        canDive = true;
    }

    public void CanSlide()
    {
        Anim.SetBool("slide", false);
        canThrow = true;
        canJump = true;
        canSlide = true;
        canDive = true;
    }

    public void CanJump()
    {
        Anim.SetBool("jump", false);
        canThrow = true;
        canJump = true;
        canSlide = true;
        canDive = true;
    }

    public void CanThrow()
    {       
        Anim.SetBool("throw", false);
        canThrow = true;
        canJump = true;
        canSlide = true;
        canDive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        //playerMovement();
        //playerJump();
        playerLookAround();
        playerAnimation();


    }

    private void FixedUpdate()
    {
        playerMovement();
    }

}
