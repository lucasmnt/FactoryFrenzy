using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 5f;
    public LayerMask groundMask;

    public Camera playerCam;
    public GameObject camHolder1st;
    public GameObject camHolder3rd;
    public Vector3 camOffset;
    public float cameraSwitchSpeed = 5f;
    public PlayerPOV currentPOV = PlayerPOV.ThirdPerson;

    private Rigidbody rb;
    private bool isGrounded;

    [SerializeField]
    public LayerMask interactLayerMask;
    public float interactRange = 10f;

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
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;

        if (IsLocalPlayer)
        {
            // Activer les cam�ras seulement pour le joueur local
            playerCam.gameObject.SetActive(true);
            camHolder1st.SetActive(true);
            camHolder3rd.SetActive(true);
        }
        else
        {
            // D�sactiver les cam�ras pour les joueurs distants
            playerCam.gameObject.SetActive(false);
            camHolder1st.SetActive(false);
            camHolder3rd.SetActive(false);
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerMovement();
        HandlePlayerJump();
        HandlePlayerLook();
        HandleTryInteract();
        HandleCameraSwitching();
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

        // Si votre Rigidbody est cin�matique, utilisez directement la fonction Translate
        if (rb.isKinematic)
        {
            transform.Translate(movement*speed*Time.deltaTime);
        }
        else
        {
            // Sinon, utilisez la v�locit� seulement pour les corps non cin�matiques
            rb.velocity=new Vector3(movement.x*speed, rb.velocity.y, movement.z*speed);
        }
    }

    private void HandleCameraSwitching()
    {
        if (!IsOwner) return;  // Seul le propri�taire devrait g�rer le changement de cam�ra

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
}