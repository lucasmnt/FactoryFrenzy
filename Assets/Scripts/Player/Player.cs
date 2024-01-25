using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour, IPlayable
{
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData = new PlayerData(new FixedString32Bytes("Roger"));
    [SerializeField] private PlayerNumber playerNumber;

    [Header("Gameplay Settings")]
    [SerializeField, Range(0f, 200f)] private float health = 200f;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private GameObject fallingBoxDetectionPrefab;
    [SerializeField] private GameObject startingCheckpoint;
    [SerializeField] private GameObject currentCheckpoint;

    [Header("Debug")]
    [SerializeField] private bool hasFinished = false;

    [SerializeField]
    private Animator animator = null;
    private bool isDancing = false;

    void Start()
    {
        // Find the starting checkpoint if not assigned
        if (startingCheckpoint==null)
            startingCheckpoint=GameObject.FindGameObjectWithTag("Start");

        SetupFallingBoxDetection();
    }

    public override void OnNetworkSpawn()
    {
        // Find the starting checkpoint if not assigned
        if (startingCheckpoint==null)
            startingCheckpoint=GameObject.FindGameObjectWithTag("Start");

        SetupFallingBoxDetection();
    }

    private void SetupFallingBoxDetection()
    {
        // Instantiate FallingBoxDetection and link it to the player
        float newY = transform.position.y-50;
        GameObject fallingBoxDetection = Instantiate(fallingBoxDetectionPrefab, new Vector3(transform.position.x, newY, transform.position.z), Quaternion.identity);
        fallingBoxDetection.GetComponent<FallingBoxDetection>().SetPlayer(transform);
    }

    void Update()
    {
        // Input handling and debugging
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Data: PlayerName: {playerData.playerName}, HasFinished: {hasFinished}");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
<<<<<<< HEAD

        if(hasFinished){
            if(!isDancing){
                isDancing = !isDancing;

                // Generate a random integer between 1 and 4
                int randomNumber = UnityEngine.Random.Range(1, 5);

                animator.SetInteger("IsDancing", randomNumber);
            }          
        }

        #endregion
=======
>>>>>>> 39b3bb23fc7553ce551d0c4679be7b140d281349
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
<<<<<<< HEAD
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
=======
        // Reset player position to the current or starting checkpoint
        Vector3 checkpointPosition = currentCheckpoint!=null ? currentCheckpoint.transform.position : startingCheckpoint.transform.position;
        checkpointPosition.y+=4f; // Adjust y-coordinate to avoid teleporting into the platform
        transform.position=checkpointPosition;
>>>>>>> 39b3bb23fc7553ce551d0c4679be7b140d281349
    }

    public void UpdateCurrentCheckpoint(GameObject newCheckpoint)
    {
        currentCheckpoint=newCheckpoint;
    }

    public void StickToPlatform(Vector3 platformMovement)
    {
<<<<<<< HEAD
        // D�place le joueur avec la plateforme
=======
        // Move the player with the platform
>>>>>>> 39b3bb23fc7553ce551d0c4679be7b140d281349
        transform.position+=platformMovement;
    }

    [ClientRpc]
    public void HasFinishedClientRpc()
    {
        hasFinished=true;
        playerData.hasFinished=true;
    }

    // IPlayable interface methods

    public PlayerNumber GetPlayerNumber()
    {
        return playerNumber;
    }

    public bool GetFinishedState()
    {
        return hasFinished;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    public string GetPlayerNumberToString()
    {
        return playerNumber.ToString();
    }
}