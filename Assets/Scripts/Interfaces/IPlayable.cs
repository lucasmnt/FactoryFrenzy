using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable
{
    public PlayerNumber HasFinished();
    public Transform GetPlayerTransform();
    public bool GetFinishedState();
    public void Fell();
    public void UpdateCurrentCheckpoint(GameObject newCheckpoint);
    public void StickToPlatform(Vector3 moveSpeed);
}
