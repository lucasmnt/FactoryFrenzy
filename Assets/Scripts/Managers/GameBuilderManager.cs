using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GameBuilderManager : NetworkBehaviour
{
    [SerializeField]
    public UnityEvent setGameSettings;

    public GameManager gm;

    public GameSettingsData gameSettingsData;

    public int numberOfRounds;
    public float timeLimitOfRound;
    public float timeLimitAfterWin;
    public int numberOfPlayer;
    public bool enableMinigames;
    public bool validateSettings;

    [SerializeField]
    GameInfoText gameInfoText;
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

    public void SetGameSettingsData()
    {
        gameSettingsData = new GameSettingsData(numberOfRounds, numberOfPlayer, timeLimitOfRound, timeLimitAfterWin, enableMinigames);
    }

    public GameSettingsData GetGameSettingsData()
    {
        return gameSettingsData;
    }

    public void ValidateGameSettings()
    {
        gm.SaveGameSettings(GetGameSettingsData());
    }

    public void TranslateToRealTypes()
    {
        // Utilisez TryParse pour éviter des erreurs si la conversion échoue
        int.TryParse(gameInfoText.textSource1.text, out numberOfRounds);
        int.TryParse(gameInfoText.textSource2.text, out numberOfPlayer);
        float.TryParse(gameInfoText.textSource3.text, out timeLimitOfRound);
        
        // Conversion pour le booléen en fonction du texte "enabled" ou "disabled"
        string minigamesText = gameInfoText.textSource4.text.ToLower(); // Convertir en minuscules pour la correspondance insensible à la casse
        // Si le texte est "enabled", enableMinigames sera true, sinon false
        enableMinigames=minigamesText=="enabled";

        float.TryParse(gameInfoText.textSource5.text, out timeLimitAfterWin);

        SetGameSettingsData();
        ValidateGameSettings();
        LogGameInfo();
    }

    public void LogGameInfo()
    {
        Debug.Log(numberOfRounds);
        Debug.Log(numberOfPlayer);
        Debug.Log(timeLimitOfRound);
        Debug.Log(enableMinigames);
        Debug.Log(timeLimitAfterWin);
    }

    void Start()
    {
        numberOfRounds = 3;
        timeLimitOfRound=300f;
        timeLimitAfterWin=10f;
        numberOfPlayer = 1;
        enableMinigames = false;
        validateSettings = false;
    }

    void Update()
    {
        
    }
}
