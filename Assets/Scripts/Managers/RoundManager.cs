using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public GameManager gm;

    [SerializeField]
    public int numberOfPlayers = 0;

    [SerializeField]
    public float timeOfRound = 300f;

    [SerializeField]
    public List<PlayerNumber> arrivalList;

    [SerializeField]
    public bool roundEnding = false;

    private bool timerActive = false;    

    // Start is called before the first frame update
    void Start()
    {
        this.roundEnding=false;
        this.arrivalList.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers=players.Length;
        GetComponent<NetworkObject>().RemoveOwnership();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateNumberOfPlayers();
        }

        CheckForArrival();

        if (roundEnding)
        {
            EndRound();
        }
    }

    public int GetNumberOfPlayers()
    {
        return this.numberOfPlayers;
    }

    public void UpdateNumberOfPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        this.numberOfPlayers = players.Length;
        Debug.Log(numberOfPlayers);
    }

    public void AddPlayerToArrivalList(PlayerNumber playerNumberToAdd)
    {
        if (!arrivalList.Contains(playerNumberToAdd))
        {
            //Debug.Log(OwnerClientId+" has arrived");
            this.arrivalList.Add(playerNumberToAdd);
        }
    }

    public void EndRound()
    {
        StartTimer();
    }
    
    public void CheckForArrival()
    {
        if (arrivalList.Count==numberOfPlayers && numberOfPlayers>=1)
        {
            this.roundEnding=true;
        }
    }

    void StartTimer()
    {
        if (!timerActive)
        {
            StartCoroutine(TimerCoroutine(10f));
        }
    }

    IEnumerator TimerCoroutine(float time)
    {
        timerActive=true;
        yield return new WaitForSeconds(time); // Attendre 10 secondes

        // Faire un truc à la fin genre SceneManager.LoadScene("HubMenu");

        Debug.Log("Finito");
        timerActive=false;
    }

    [ServerRpc]
    public void ServerArrivalNotificationServerRpc(PlayerNumber playerNumberToAdd)
    {
        AddPlayerToArrivalList(playerNumberToAdd);
        Debug.Log("rpclol");
    }
}

