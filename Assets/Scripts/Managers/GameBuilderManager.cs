using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameBuilderManager : NetworkBehaviour
{
    public int numberOfRounds;
    public float timeLimitOfRound;
    public int numberOfPlayer;
    public bool enableMinigames;
    public bool validateSettings;

    // Maps to play
    /*  */
    
    // Setters getters
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

    public void SetEnableMinigames(bool b)
    {
        enableMinigames = b;
    }

    public bool GetEnableMinigames()
    {
        return enableMinigames;
    }

    public void SetValidateSettings(bool b)
    {
        validateSettings = b;
    }

    public bool GetValidateSettings()
    {
        return validateSettings;
    }

    void Start()
    {
        numberOfRounds = 3;
        timeLimitOfRound=300f;
        numberOfPlayer = 1;
        enableMinigames = false;
        validateSettings = false;
    }

    void Update()
    {
        
    }
}
