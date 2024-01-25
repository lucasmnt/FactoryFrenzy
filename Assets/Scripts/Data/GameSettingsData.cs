using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsData
{
    int numberOfRounds;
    int numberOfPlayers;
    float timeLimit;
    float timeAfterLimit;
    bool enableMinigame;

    public GameSettingsData(int numberOfRounds, int numberOfPlayers, float timeLimit, float timeAfterLimit, bool enableMinigame)
    {
        this.numberOfRounds=numberOfRounds;
        this.numberOfPlayers=numberOfPlayers;
        this.timeLimit=timeLimit;
        this.timeAfterLimit=timeAfterLimit;
        this.enableMinigame=enableMinigame;
    }



    //mapsToPlay;
}
