using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTES:
// Use this model for Runni Boi. It encourages 

public class PlayerController : MonoBehaviour
{
    // Components and Assignables
    public Rigidbody body;

    public Transform groundCheck;

    // Movement
    float moveForce = 30.0f;
    float magThreshold = 0.02f;
    float xMag, zMag;

    float jumpForce = 10.0f;

    // Diection and heading
    Vector3 direction = Vector3.zero;

    // Inputs
    float x, z;
    bool jump, grounded;

    // Layers
    public LayerMask groundMask;

    // Dists to check
    float groundCheckRadius = 0.2f;

    // Movement Restricting var(s)
    float maxMag = 20.0f;
    float drag = 10.0f;
    float angularDrag = 10.05f;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
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
        // Checks for ground
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // Collects inputs for movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");

        // Takes care of player traveling faster than intended
        if (z > 0 && body.velocity.z > maxMag) z = 0;
        if (z < 0 && body.velocity.z < -maxMag) z = 0;
        if (x > 0 && body.velocity.x > maxMag) x = 0;
        if (x < 0 && body.velocity.x < -maxMag) x = 0;

        // Finidng the magitudes of movement
        xMag = body.velocity.x;
        zMag = body.velocity.z;

        // Creates a 3D direction from x and z
        direction = transform.forward * z + transform.right * x;
        direction.Normalize();
    }

    private void Movement()
    {
        // Makes the player jump
        if (jump && grounded) Jump();

        // Moves the player object
        body.AddForce(direction * moveForce, ForceMode.VelocityChange);

        // Slows the player
        ApplyDrag();
    }

    private void Jump()
    {
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Physical operations
    // This mathod taken, renamed and very slightly modified from Dani's CounterMovement method in his PlayerController.cs script
    private void ApplyDrag()
    {
        // Adds drag in the opposite direction of movement
        if ((Mathf.Abs(xMag) > magThreshold) && ((Mathf.Abs(x) < 0.05f) || (x < 0) || (x > 0)))
        {
            body.AddForce(transform.right * -drag * xMag);
        }
        if ((Mathf.Abs(zMag) > magThreshold) && ((Mathf.Abs(z) < 0.05f) || (z < 0) || (z > 0)))
        {
            body.AddForce(transform.forward * -drag * zMag);
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    */
}
