using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayable
{
    [SerializeField]
    public PlayerNumber playerNumber;

    [SerializeField]
    public bool hasFinished = false;

    [SerializeField, Range(0f, 100f)] 
    public float interactionDistance = 0f;

    [SerializeField, Range(0f, 5f)] 
    private const float maxHoldTime = 2f; // temps maximum pour force max.

    [SerializeField, Range(0f, 5f)] 
    private float minDistance = 1f; // Set your minimum distance here

    [SerializeField, Range(0f, 10f)] 
    private float maxDistance = 5f; // Set your maximum distance here

    [SerializeField, Range(0f, 100f)] 
    private float itemRotationSpeed = 10f;

    [SerializeField, Range(0f, 200f)]
    private float heatlh = 200f;

    [SerializeField] 
    LayerMask raycastLayerMask;

    [SerializeField]
    private GameObject startingCheckpoint;

    [SerializeField]
    private GameObject currentCheckpoint;

    void Start()
    {
        this.startingCheckpoint = GameObject.FindGameObjectWithTag("Start");
    }

    void Update()
    {
        #region Input
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast from the main camera
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, interactionDistance, raycastLayerMask))
            {
                // Check if the hit object implements the IInteractable interface
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
        #endregion
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying=false;
        #else
            Application.Quit();
        #endif
    }

    // IPlayable interface implementation
    public void Fell()
    {
        if (currentCheckpoint!=null)
        {
            Vector3 checkpointPosition = currentCheckpoint.transform.position;
            checkpointPosition.y+=4f; // Augmentez la coordonnée y pour éviter de se téléporter dans la plateforme
            transform.position=checkpointPosition;
        }
        else
        {
            // Si le point de contrôle actuel n'est pas défini, vous pouvez choisir de le faire revenir à une position par défaut.
            // Par exemple, le centre de la scène.
            Vector3 startingPosition = startingCheckpoint.transform.position;
            startingPosition.y+=4f; // Augmentez la coordonnée y pour éviter de se téléporter dans la plateforme
            transform.position=startingPosition;
        }
    }

    public void UpdateCurrentCheckpoint(GameObject newCheckpoint)
    {
        this.currentCheckpoint=newCheckpoint;
    }

    public void StickToPlatform(Vector3 platformMovement)
    {
        // Déplace le joueur avec la plateforme
        transform.position+=platformMovement;
    }

    public PlayerNumber HasFinished()
    {
        this.hasFinished=true;
        return this.playerNumber;
    }

    public bool GetFinishedState()
    {
        return this.hasFinished;
    }

    public Transform GetPlayerTransform()
    {
        return this.gameObject.transform;
    }
}
