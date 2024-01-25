using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : INetworkSerializable
{
    public FixedString32Bytes playerName;
    public bool hasFinished;

    public PlayerData(FixedString32Bytes playerName)
    {
        this.playerName=playerName;
        this.hasFinished=false;
    }

    public PlayerData(FixedString32Bytes playerName, bool hasFinished)
    {
        this.playerName=playerName;
        this.hasFinished=hasFinished;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref hasFinished);
    }

}
