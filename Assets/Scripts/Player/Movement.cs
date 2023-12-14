using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 5f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true; // Freeze rotation to prevent camera tilting
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerMovement();
        HandlePlayerJump();
        HandlePlayerLook();
    }

    private void CheckGrounded()
    {
        // Check if the player is grounded
        isGrounded=Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
    }

    private void HandlePlayerMovement()
    {
        // Player Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward*vertical+transform.right*horizontal;
        rb.velocity=new Vector3(movement.x*speed, rb.velocity.y, movement.z*speed);
    }

    private void HandlePlayerJump()
    {
        // Player Jump
        if (isGrounded&&Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }
    }

    private void HandlePlayerLook()
    {
        // Player Look
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up*mouseX*sensitivity);
        Camera.main.transform.Rotate(Vector3.left*mouseY*sensitivity);

        // Clamp vertical camera rotation to prevent flipping
        Quaternion currentRotation = Camera.main.transform.localRotation;
        float clampedXRotation = Mathf.Clamp(currentRotation.x, -0.59f, 0.59f);
        currentRotation=new Quaternion(clampedXRotation, currentRotation.y, currentRotation.z, currentRotation.w);
        Camera.main.transform.localRotation=currentRotation;
    }
}