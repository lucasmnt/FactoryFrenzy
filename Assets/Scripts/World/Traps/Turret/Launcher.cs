using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : EditorObjects
{
    // Enum to define the possible states
    public enum LauncherState
    {
        Inactive,
        LookingForTarget,
        ShootingAtTarget
    }

    Transform _Player;
    float dist;
    public float howClose;
    public Transform head, barrel;
    public GameObject _projectile;
    public float fireRate, nextFire;
    public bool lockedPlayer = false;
    public float lockedTime = 5f;
    public float inactiveTime = 2f;
    public float maxTargetDistance = 30f;
    public bool isShooting = false;

    // Variable to store the current state
    private LauncherState currentState = LauncherState.Inactive;
    private float stateChangeTime;

    // List to store potential targets
    private List<Transform> potentialTargets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial state to Inactive
        currentState=LauncherState.Inactive;
        stateChangeTime=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the state based on the current conditions
        UpdateState();
    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, barrel.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward*1500);
        Destroy(clone, 10);
    }

    void LockPlayer(IPlayable player)
    {
        // Vérifier la distance avant de verrouiller
        float playerDistance = Vector3.Distance(transform.position, player.GetPlayerTransform().position);
        if (playerDistance<=maxTargetDistance)
        {
            _Player=player.GetPlayerTransform();
        }
    }

    void LookAtPlayer()
    {
        dist=Vector3.Distance(_Player.position, transform.position);

        if (dist<=howClose)
        {
            head.LookAt(_Player);
        }
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case LauncherState.Inactive:
                // Check conditions to transition to LookingForTarget state
                if (Time.time-stateChangeTime>=inactiveTime)
                {
                    currentState=LauncherState.LookingForTarget;
                    stateChangeTime=Time.time;
                }
                break;

            case LauncherState.LookingForTarget:
                // Check conditions to transition to ShootingAtTarget state
                if (potentialTargets.Count>0)
                {
                    _Player=GetClosestTarget();
                    currentState=LauncherState.ShootingAtTarget;
                    stateChangeTime=Time.time;
                }
                break;

            case LauncherState.ShootingAtTarget:
                // Check conditions to transition back to Inactive state
                if (Time.time-stateChangeTime>=lockedTime)
                {
                    currentState=LauncherState.Inactive;
                    stateChangeTime=Time.time;
                    lockedPlayer=false;
                    potentialTargets.Clear(); // Clear the list when transitioning to Inactive state
                }
                else
                {
                    // Continue shooting or looking at the target
                    LookAtPlayer();
                    if (Time.time>=nextFire)
                    {
                        nextFire=Time.time+1f/fireRate;
                        Shoot();
                    }
                }
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!lockedPlayer)
        {
            if (other)
            {
                // Check if the hit object implements the IInteractable interface
                IPlayable player = other.GetComponent<IPlayable>();
                if (player!=null)
                {
                    LockPlayer(player);
                    lockedPlayer=true;
                    potentialTargets.Add(player.GetPlayerTransform()); // Add the player to the list of potential targets
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!lockedPlayer)
        {
            if (other)
            {
                // Check if the hit object implements the IInteractable interface
                IPlayable player = other.GetComponent<IPlayable>();
                if (player!=null)
                {
                    LockPlayer(player);
                    lockedPlayer=true;
                    if (!potentialTargets.Contains(player.GetPlayerTransform()))
                    {
                        potentialTargets.Add(player.GetPlayerTransform()); // Add the player to the list of potential targets if not already in the list
                    }
                }
            }
        }
    }

    private Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Transform target in potentialTargets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance<closestDistance)
            {
                closestDistance=distance;
                closestTarget=target;
            }
        }

        return closestTarget;
    }
}