using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTES: Use Rigidbody.movePosition() instead of Rigidbody.addForce() to side-step all the extra manipulations.

public class PlayerController : MonoBehaviour
{
    // NOTES: 
    //        * The math in the FindVelocity is code I found on YouTube when looking for videos on how Quake's movement worked. I don't remember the video, but I
    //          am sure the viewer of this code can find the exact code format I found. Otherwise, (when I remember) I will add the link to the specific video.
    //        * The code (variable names, etc.) in the FindVelocity method is very similar to the code I found on YouTube; however, I changed the variable names,
    //          so, hopefully, what is happening is easier for other - an myself - to better understand what is going on.

    // Components and Assignables
    public Rigidbody body;
    public Transform groundCheck;
    public CapsuleCollider capsule;

    // Movement vars
    float moveForce = 100.0f;
    float jumpForce = 13.0f;
    float termVelocity = 30.0f;

    Vector3 wishedDir;

    // Rotation vars
    float sensitivity = 100.0f;

    // Inputs
    float x, z, mouseX;
    bool jump, slide;

    // Ground checking
    public float groundCheckRadius = 0.2f;
    //bool grounded;

    public LayerMask groundMask;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInputs();   
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void GetInputs()
    {
        float jumpMult = 0f;

        // Inputs : Movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");
        slide = Input.GetKeyDown(KeyCode.LeftShift);

        if (jump) jumpMult = 1f;

        // Creates the vector the player wishes to move along
        wishedDir = transform.right * x + transform.forward * z + transform.up * jumpMult;
        wishedDir.Normalize();
        // NOTE: body.velocity = new Vector3(wishedDir.x * moveForce, wishedDir.y * jumpForce, wishedDir.z * moveForec);

        // Inputs : Rotation
        mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
    }

    private void Movement()
    {
        // Rotates the player
        body.MoveRotation(transform.rotation * Quaternion.Euler(new Vector3(0, mouseX, 0) * Time.deltaTime));

        if (Grounded() && jump) Jump();

        // Moves player along their current velovity
        body.velocity = FindVelocity(body.velocity, wishedDir);
    }

    private void Jump()
    {
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // 
    private Vector3 FindVelocity(Vector3 currVelocity, Vector3 wishedDir)
    {
        float currSpeed = currVelocity.magnitude;

        float speedToAdd = moveForce - currSpeed;
        speedToAdd = Mathf.Max(Mathf.Min(speedToAdd, termVelocity * Time.deltaTime), 0);

        return currVelocity + wishedDir * speedToAdd;
    }

    private bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    */}
