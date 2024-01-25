using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour, IPlayable
{
    [SerializeField]
    public PlayerNumber playerNumber;

    [SerializeField]
    public bool hasFinished = false;

    [SerializeField, Range(0f, 200f)]
    private float heatlh = 200f;

    [SerializeField] 
    LayerMask raycastLayerMask;

    [SerializeField]
    private GameObject startingCheckpoint;

    [SerializeField]
    private GameObject currentCheckpoint;

    [SerializeField]
    private GameObject fallingBoxDetectionPrefab;

    [SerializeField]
    private Animator animator = null;
    private bool isDancing = false;

    void Start()
    {
        this.startingCheckpoint = GameObject.FindGameObjectWithTag("Start");
        SetupFallingBoxDetection();
    }

    private void SetupFallingBoxDetection()
    {
        float newY = transform.position.y-50;
        // Instancier le FallingBoxDetection et le lier au joueur
        GameObject fallingBoxDetection = Instantiate(fallingBoxDetectionPrefab, new Vector3(transform.position.x, newY, transform.position.z), Quaternion.identity);
        fallingBoxDetection.GetComponent<FallingBoxDetection>().SetPlayer(transform);
    }

    void Update()
    {
        #region Input
        if (Input.GetKeyDown(KeyCode.F))
        {
           
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if(hasFinished){
            if(!isDancing){
                isDancing = !isDancing;

                // Generate a random integer between 1 and 4
                int randomNumber = UnityEngine.Random.Range(1, 5);

                animator.SetInteger("IsDancing", randomNumber);
            }          
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
            checkpointPosition.y+=4f; // Augmentez la coordonn�e y pour �viter de se t�l�porter dans la plateforme
            transform.position=checkpointPosition;
        }
        else
        {
            // Si le point de contr�le actuel n'est pas d�fini, vous pouvez choisir de le faire revenir � une position par d�faut.
            // Par exemple, le centre de la sc�ne.
            Vector3 startingPosition = startingCheckpoint.transform.position;
            startingPosition.y+=4f; // Augmentez la coordonn�e y pour �viter de se t�l�porter dans la plateforme
            transform.position=startingPosition;
        }
    }

    public void UpdateCurrentCheckpoint(GameObject newCheckpoint)
    {
        this.currentCheckpoint=newCheckpoint;
    }

    public void StickToPlatform(Vector3 platformMovement)
    {
        // D�place le joueur avec la plateforme
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

    public string GetPlayerNumberToString()
    {
        return this.playerNumber.ToString();
    }
}
