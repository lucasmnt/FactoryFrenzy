using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    public int numberOfPlayers = 1;

    [SerializeField]
    public float timeOfRound = 300f;

    [SerializeField]
    public List<PlayerNumber> arrivalList;

    [SerializeField]
    public bool roundEnding = false;

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
        if(!roundEnding)
        {
            EndRound();
        }
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
        if (arrivalList.Count==numberOfPlayers)
        {
            Debug.Log("Le round est terminé !");
            this.roundEnding=true;
            SceneManager.LoadScene("HubMenu");
        }
    }
}
