using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayable
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
