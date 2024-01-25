using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour, IPlayable
{
    [Header("Player Data")]
    //[SerializeField] private PlayerData playerData = new PlayerData(new FixedString32Bytes("Roger"));
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
            //Debug.Log($"Data: PlayerName: {playerData.playerName}, HasFinished: {hasFinished}");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (hasFinished)
        {
            if (!isDancing)
            {
                isDancing=!isDancing;

                // Generate a random integer between 1 and 4
                int randomNumber = UnityEngine.Random.Range(1, 5);

                animator.SetInteger("IsDancing", randomNumber);
            }
        }
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
        currentCheckpoint=newCheckpoint;
    }

    public void StickToPlatform(Vector3 platformMovement)
    {
        transform.position+=platformMovement;
    }

    [ClientRpc]
    public void HasFinishedClientRpc()
    {
        hasFinished=true;
        //playerData.hasFinished=true;
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