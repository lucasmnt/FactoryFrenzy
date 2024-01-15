using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilderManager : MonoBehaviour
{
    public int numberOfRounds;
    public float timeLimitOfRound;
    public int numberOfPlayer;

    public void SetNumberOfRounds(int nbRounds)
    {
        numberOfRounds = nbRounds;
    }

    public int GetNumberOfRounds()
    {
        return numberOfRounds;
    }

    public void SetTimeLimit(float timeLimit)
    {
        timeLimitOfRound = timeLimit;
    }

    public int GetTimeLimit()
    {
        return numberOfRounds;
    }

    public void SetNumberOfPlayers(int nbPlayer)
    {
        numberOfRounds = nbPlayer;
    }

    public int GetNumberOfPlayers()
    {
        return numberOfRounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfRounds = 1;
        timeLimitOfRound=300f;
        numberOfPlayer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
