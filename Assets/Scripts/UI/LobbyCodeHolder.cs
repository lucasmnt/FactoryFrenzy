using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyCodeHolder : MonoBehaviour
{
    private string code;

    public string GetCode()
    {
        return code;
    }

    public void SetCode(string s)
    {
        code = s;
    }

    public async void SelectedJoinLobby(GameObject go)
    {
        try
        {
            LobbyCodeHolder holder = go.GetComponent<LobbyCodeHolder>();

            await Lobbies.Instance.JoinLobbyByCodeAsync(holder.GetCode());
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
