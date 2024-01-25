using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private float heartbeatTimer;
    private string playerName;

    // Start is called before the first frame update
    async void Start()
    {
        /*await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName="Johny"+UnityEngine.Random.Range(10,99);*/
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 10;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate=true,


            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby=lobby;

            Debug.Log("Lobby Created! "+lobby.Name+" "+lobby.MaxPlayers);
            Debug.Log("Lobby code : "+lobby.LobbyCode);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found : "+queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
                Debug.Log(lobby.Name+" "+lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined lobby !");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void PrintPlayers(Lobby lobby)
    {
        try
        {
            foreach (Unity.Services.Lobbies.Models.Player player in lobby.Players)
            {
                Debug.Log(player.Id);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Unity.Services.Lobbies.Models.Player GetNewPlayer()
    {
        return new Unity.Services.Lobbies.Models.Player
        {
            Data=new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer-=Time.deltaTime;
            if (heartbeatTimer <0f)
            {
                float heatbeatTimerMax = 15;
                heartbeatTimer = heatbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*HandleLobbyHeartbeat();

        if (Input.GetKeyDown(KeyCode.T))
            CreateLobby();

        if (Input.GetKeyDown(KeyCode.G))
            ListLobbies();

        if (Input.GetKeyDown(KeyCode.B))
            JoinLobbyByCode(hostLobby.LobbyCode);
        */

    }
}
