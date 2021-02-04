using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components and Assignables
    public Rigidbody body;

    public Transform groundCheck;

    // Movement
    public float moveSpeed = 30.0f;
    public float jumpForce = 10.0f;

    // Diection
    Vector3 direction = Vector3.zero;

    // Heading
    float sensitivity = 100.0f;

    Vector3 rotX = Vector3.zero;

    // Inputs
    float x, z;
    bool jump, grounded;

    // Layers
    public LayerMask groundMask;

    // Movement Restricting var(s)
    public float maxMag = 20.0f;

    // Dists to check
    float groundCheckRadius = 0.2f;


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
        // Checks
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // Collects inputs for movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");

        // Creates the basis for movement
        direction = transform.forward * z + transform.right * x;
        direction.Normalize();

        // Creates the basis for a rotation
        rotX = new Vector3(0, Input.GetAxis("Mouse X"), 0);
    }

    // Movement
    private void Movement()
    {
        // Makes the player jump
        if (jump && grounded) Jump();

        // Moves player in a direction
        body.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        // Rotates the player
        Quaternion look = Quaternion.Euler(rotX * sensitivity * Time.fixedDeltaTime);
        body.MoveRotation(transform.rotation * look);
    }

    // Jumping
    private void Jump()
    {
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
