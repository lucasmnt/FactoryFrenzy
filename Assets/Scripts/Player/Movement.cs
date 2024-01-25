using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;
    public LayerMask groundMask;

    [Header("Camera")]
    public Camera playerCam;
    public GameObject camHolder1st;
    public GameObject camHolder3rd;
    public float cameraSwitchSpeed = 5f;
    public PlayerPOV currentPOV = PlayerPOV.ThirdPerson;

    [Header("Interaction")]
    public LayerMask interactLayerMask;
    public float interactRange = 10f;

    [SerializeField]
    private Animator animator = null;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isWriting = false;
    private CustomTextEditor customTextEditor;

    public enum PlayerPOV
    {
        FirstPerson,
        ThirdPerson,
        External,
        Cinematic
    }

    public override void OnNetworkSpawn()
    {
        // Désactivez la caméra à la première personne pour tous les joueurs sauf le propriétaire
        camHolder1st.SetActive(false);
        camHolder3rd.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();
        Cursor.lockState=CursorLockMode.Locked;
        rb.freezeRotation=true;

        if (!NetworkManager.Singleton.IsServer)
        {
            EnableCameras();
        }
        else
        {
            if (IsLocalPlayer)
            {
                EnableCameras();
            }
            else
            {
                DisableCameras();
                AudioListener audioListener = GetComponentInChildren<AudioListener>();
                if (audioListener!=null)
                {
                    audioListener.enabled=false;
                }
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (!isWriting)
        {
            CheckGrounded();
            HandlePlayerWriting();
            HandlePlayerMovement();
            HandlePlayerJump();
            HandleCameraSwitching();
            HandlePlayerLook();
            HandleTryInteract();
        }
    }

    private void CheckGrounded()
    {
        isGrounded=Physics.Raycast(transform.position, Vector3.down, 0.1f, groundMask);
        Debug.DrawRay(transform.position, Vector3.down*1.1f, isGrounded ? Color.green : Color.red);
    }

    private void HandlePlayerWriting()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IWritable writable = hit.collider.GetComponent<IWritable>();
                if (writable!=null)
                {
                    customTextEditor=hit.collider.GetComponent<CustomTextEditor>();
                    if (customTextEditor!=null)
                    {
                        customTextEditor.Write(!customTextEditor.IsWriting);
                        Debug.Log("Toggle de l'état d'écriture : "+customTextEditor.IsWriting);
                    }
                }
            }
        }
    }

    private void HandlePlayerMovement()
    {
        if (isWriting) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? (speed*1.2f) : speed;
        Vector3 movement = transform.forward*vertical+transform.right*horizontal;

        if (rb.isKinematic)
        {
            transform.Translate(movement*currentSpeed*Time.deltaTime);
        }
        else
        {
            rb.velocity=new Vector3(movement.x*currentSpeed, rb.velocity.y, movement.z*currentSpeed);
        }

        float relativeSpeed = Mathf.Clamp01(Mathf.Abs(horizontal)+Mathf.Abs(vertical));

        if (relativeSpeed>0f&&!isRunning)
        {
            animator.SetBool("IsWalking", true);
        }

        animator.SetBool("IsRunning", isRunning);
    }

    private void HandleCameraSwitching()
    {
        if (NetworkManager.Singleton!=null&&!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentPOV=(currentPOV==PlayerPOV.FirstPerson) ? PlayerPOV.ThirdPerson : PlayerPOV.FirstPerson;
            Vector3 targetPosition = (currentPOV==PlayerPOV.FirstPerson) ? camHolder1st.transform.position : camHolder3rd.transform.position;
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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up*mouseX*sensitivity);
        playerCam.transform.Rotate(Vector3.left*mouseY*sensitivity);

        Quaternion currentRotation = playerCam.transform.localRotation;
        float clampedXRotation = Mathf.Clamp(currentRotation.x, -0.59f, 0.59f);
        currentRotation=new Quaternion(clampedXRotation, currentRotation.y, currentRotation.z, currentRotation.w);
        playerCam.transform.localRotation=currentRotation;
    }

    private void HandleTryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable!=null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void SetIsWriting(bool b)
    {
        isWriting=b;
        Debug.Log(b);
    }

    private void EnableCameras()
    {
        playerCam.gameObject.SetActive(true);
        camHolder1st.SetActive(true);
        camHolder3rd.SetActive(true);
    }

    private void DisableCameras()
    {
        playerCam.gameObject.SetActive(false);
        camHolder1st.SetActive(false);
        camHolder3rd.SetActive(false);
    }
}
