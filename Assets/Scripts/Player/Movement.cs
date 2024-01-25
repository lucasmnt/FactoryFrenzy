using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Movement : NetworkBehaviour
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
    private Animator animator = null;
    
    public enum PlayerPOV
    {
        FirstPerson,
        ThirdPerson,
        External,
        Cinematic
    }

    public override void OnNetworkSpawn()
    {
        camHolder1st.SetActive(false);
        camHolder3rd.SetActive(IsOwner);
        base.OnNetworkSpawn(); 
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator=GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;

        // V�rifier si le jeu est en mode hors ligne (pas de r�seau)
        if (!NetworkManager.Singleton.IsServer)
        {
            // Activer les cam�ras pour tous les joueurs en mode hors ligne
            playerCam.gameObject.SetActive(true);
            camHolder1st.SetActive(true);
            camHolder3rd.SetActive(true);
        }
        else
        {
            // Si le jeu est en mode r�seau, activer les cam�ras uniquement pour le joueur local
            if (IsLocalPlayer)
            {
                playerCam.gameObject.SetActive(true);
                camHolder1st.SetActive(true);
                camHolder3rd.SetActive(true);
            }
            else
            {
                playerCam.gameObject.SetActive(false);
                camHolder1st.SetActive(false);
                camHolder3rd.SetActive(false);
            }
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerWriting();
        if (isWriting == false)
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
            // Cr�ez un rayon depuis la cam�ra vers l'avant
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            // V�rifiez s'il y a une collision avec un objet portant l'interface IInteractable
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

            // Vérifiez si la touche "Shift" est enfoncée pour courir
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // Déterminez la vitesse actuelle en fonction de la marche ou de la course
            float currentSpeed = isRunning ? (speed * 1.2f) : speed;

            Vector3 movement = transform.forward * vertical + transform.right * horizontal;

            if (rb.isKinematic)
            {
                transform.Translate(movement * currentSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = new Vector3(movement.x * currentSpeed, rb.velocity.y, movement.z * currentSpeed);
            }

            float relativeSpeed = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            if(relativeSpeed > 0f && !isRunning){
                animator.SetBool("IsWalking", true);
            }
            
            animator.SetBool("IsRunning", isRunning);
        }
    }


    private void HandleCameraSwitching()
    {
        // V�rifiez si le jeu est en mode multijoueur ou hors ligne
        if (NetworkManager.Singleton!=null&&!IsOwner)
        {
            // Seul le propri�taire devrait g�rer le changement de cam�ra en mode multijoueur
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Inversez l'�tat entre premi�re personne et troisi�me personne
            currentPOV=(currentPOV==PlayerPOV.FirstPerson) ? PlayerPOV.ThirdPerson : PlayerPOV.FirstPerson;

            // D�finir la position cible en fonction de l'�tat actuel
            Vector3 targetPosition = (currentPOV==PlayerPOV.FirstPerson) ? camHolder1st.transform.position : camHolder3rd.transform.position;

            // Commencer la transition de cam�ra en ajustant directement la position
            playerCam.transform.position=targetPosition;
        }
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
            // Cr�ez un rayon depuis la cam�ra vers l'avant
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            // V�rifiez s'il y a une collision avec un objet portant l'interface IInteractable
            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable!=null)
                {
                    // Appel � la m�thode Interact de l'objet
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