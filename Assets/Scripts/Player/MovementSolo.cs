using System.Collections;
using UnityEngine;

public class MovementSolo : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;
    public LayerMask groundMask;

    public Camera playerCam;
    public GameObject camHolder1st;
    public GameObject camHolder3rd;
    public Vector3 camOffset;
    public float cameraSwitchSpeed = 5f;
    public PlayerPOV currentPOV = PlayerPOV.ThirdPerson;

    [SerializeField]
    private Rigidbody rb;
    private bool isGrounded;

    private bool isWriting = false;
    private CustomTextEditor customTextEditor;

    [SerializeField]
    public LayerMask interactLayerMask;
    public float interactRange = 10f;

    [SerializeField]
    private Animator animator;

    public enum PlayerPOV
    {
        FirstPerson,
        ThirdPerson,
        External,
        Cinematic
    }

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();
        Cursor.lockState=CursorLockMode.Locked;
        rb.freezeRotation=true;

        playerCam.gameObject.SetActive(true);
        camHolder1st.SetActive(true);
        camHolder3rd.SetActive(true);
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerWriting();
        if (isWriting==false)
        {
            HandlePlayerMovement();
            HandlePlayerJump();
            HandleCameraSwitching();
            HandlePlayerLook();
            HandleTryInteract();
        }
    }

    private void CheckGrounded()
    {
        // Check if the player is grounded
        isGrounded=Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f, groundMask);

        // Debug the raycast
        Debug.DrawRay(transform.position, Vector3.down*1.1f, isGrounded ? Color.green : Color.red);

        // Optionally, you can log information about the hit point
        if (isGrounded)
        {
            Debug.Log("Grounded at position: "+hit.point);
        }
    }

    private void HandlePlayerWriting()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Create a ray from the camera forward
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            // Check for collision with an object implementing the IInteractable interface
            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IWritable writable = hit.collider.GetComponent<IWritable>();
                if (writable!=null)
                {
                    customTextEditor=hit.collider.GetComponent<CustomTextEditor>();
                    if (customTextEditor!=null)
                    {
                        customTextEditor.Write(!customTextEditor.IsWriting);
                        Debug.Log("Toggled Writing State: "+customTextEditor.IsWriting);
                    }
                }
            }
        }
    }

    private void HandlePlayerMovement()
    {
        if (!isWriting)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = transform.forward*vertical+transform.right*horizontal;

            if (rb.isKinematic)
            {
                transform.Translate(movement*speed*Time.deltaTime);
                // Déclencher l'animation de marche
                animator.SetFloat("Speed", movement.magnitude);
            }
            else
            {
                rb.velocity=new Vector3(movement.x*speed, rb.velocity.y, movement.z*speed);
                // Déclencher l'animation de marche
                animator.SetFloat("Speed", rb.velocity.magnitude);
            }
        }
    }

    private void HandleCameraSwitching()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Invert the state between first person and third person
            currentPOV=(currentPOV==PlayerPOV.FirstPerson) ? PlayerPOV.ThirdPerson : PlayerPOV.FirstPerson;

            // Set the target position based on the current state
            Vector3 targetPosition = (currentPOV==PlayerPOV.FirstPerson) ? camHolder1st.transform.position : camHolder3rd.transform.position;

            // Start the camera transition by adjusting the position directly
            playerCam.transform.position=targetPosition;
        }
    }

    private void HandlePlayerJump()
    {
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
        playerCam.transform.Rotate(Vector3.left*mouseY*sensitivity);

        // Clamp vertical camera rotation to prevent flipping
        Quaternion currentRotation = playerCam.transform.localRotation;
        float clampedXRotation = Mathf.Clamp(currentRotation.x, -0.59f, 0.59f);
        currentRotation=new Quaternion(clampedXRotation, currentRotation.y, currentRotation.z, currentRotation.w);
        playerCam.transform.localRotation=currentRotation;
    }

    private void HandleTryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Create a ray from the camera forward
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            // Check for collision with an object implementing the IInteractable interface
            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable!=null)
                {
                    // Call the Interact method of the object
                    interactable.Interact();
                }
            }
        }
    }

    public void SetIsWriting(bool b)
    {
        this.isWriting=b;
        Debug.Log(b);
    }
}