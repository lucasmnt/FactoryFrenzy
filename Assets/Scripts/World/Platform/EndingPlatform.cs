using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EndingPlatform : EditorObjects
{
    public RoundManager roundManager;

    void Start()
    {
        roundManager=GetComponent<RoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other!=null)
        {
            TryHandleCollision(other.GetComponent<IPlayable>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider!=null)
        {
            TryHandleCollision(collision.collider.GetComponent<IPlayable>());
        }
    }

    private void TryHandleCollision(IPlayable player)
    {
        if (player!=null&&!player.GetFinishedState())
        {
            roundManager.AddPlayerToArrivalList(player.GetPlayerNumber());
            player.HasFinishedClientRpc();
            EndGameClientRpc();
        }
    }

    [ClientRpc]
    public void EndGameClientRpc()
    {
        StartCoroutine(StartGameEndTimer());
        Debug.Log("Fin de la partie détectée !");
    }

    private IEnumerator StartGameEndTimer()
    {
        yield return new WaitForSeconds(10f);
        // Ajoutez le code de fin de partie ici
        Debug.Log("Fin de la partie !");
    }
}