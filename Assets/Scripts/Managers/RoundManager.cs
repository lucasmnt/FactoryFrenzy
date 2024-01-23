using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RoundManager : MonoBehaviour
{
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
            Debug.Log(playerNumberToAdd+" has arrived");
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

        timerActive=false;
    }
}

