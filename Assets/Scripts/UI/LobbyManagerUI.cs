using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using TMPro;
using Unity.Services.Core;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class LobbyManagerUI : MonoBehaviour
{
    // Panels
    public GameObject lobbyMainPanel;
    public GameObject lobbyCreationPanel;
    public GameObject lobbyJoinPanel;
    public GameObject lobbyConnectedPanel;

    // Create params
    public TMP_InputField lobbyNameInput;
    public TMP_InputField maxPlayersInput;
    public Toggle privateToggle;
    private Lobby hostLobby;
    private float heartbeatTimer;

    // Connected params
    public TMP_Text lobbyCodeCreated;
    public TMP_Text lobbyNameCreated;
    public TMP_Text connectedPlayersCreated;
    public ScrollRect lobbyListScrollView;

    // Join params
    public TMP_InputField lobbyCodeInput;
    public GameObject lobbyButtonPrefab;

    // Player params
    public TMP_InputField playerNameInput;
    private string playerName;

    // Launching params
    public GameObject networkManagerRef;

    // Sounds
    public AudioListener audioListener;

    // Start is called before the first frame update
    async void Start()
    {
        playerNameInput.text="Roger"+UnityEngine.Random.Range(0, 99);
        OpenMainLobbyPanel();
        await InitializeAuthentication();
        // Désactivez l'AudioListener si ce n'est pas le joueur local
        if (audioListener!=null)
        {
            audioListener.enabled=false;
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async Task InitializeAuthentication()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn+=() =>
        {
            Debug.Log("Signed in "+AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void OpenCreateLobbyPanel()
    {
        SetPanelsState(true, false, false, false, false);
    }

    public void OpenJoinLobbyPanel()
    {
        SetPanelsState(false, true, false, false, false);
    }

    public void OpenMainLobbyPanel()
    {
        SetPanelsState(false, false, true, false, false);
    }

    public void OpenConnectedLobbyPanel()
    {
        SetPanelsState(false, false, false, true, true);
    }

    private void SetPanelsState(bool create, bool join, bool main, bool connected, bool networkManager)
    {
        lobbyCreationPanel.SetActive(create);
        lobbyJoinPanel.SetActive(join);
        lobbyMainPanel.SetActive(main);
        lobbyConnectedPanel.SetActive(connected);
        networkManagerRef.SetActive(networkManager);
    }

    public void LaunchGame()
    {
        try
        {
            if (networkManagerRef!=null)
            {
                NetworkManager networkManager = networkManagerRef.GetComponent<NetworkManager>();
                StartHost(networkManager);
                networkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
                DeactivateCamera();
            }
            else
            {
                Debug.LogError("networkManagerRef is null. Please assign a valid reference in the inspector.");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = lobbyNameInput.text;
            int lobbyMaxPlayers = int.Parse(maxPlayersInput.text);
            bool isPrivate = privateToggle.isOn;

            Unity.Services.Lobbies.Models.Player newPlayer = GetNewPlayer();

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate=isPrivate,
                Player=newPlayer
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, lobbyMaxPlayers, createLobbyOptions);
            hostLobby=lobby;

            UpdateLobbyInfo(lobby);
            Debug.Log($"LobbyMaxPlayers: {lobbyMaxPlayers}, IsPrivate: {createLobbyOptions.IsPrivate.Value}");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void UpdateLobby()
    {
        try
        {
            DisplayConnectedPlayers();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            DestroyOldLobbyButtons();
            CreateNewLobbyButtons(queryResponse);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void DestroyOldLobbyButtons()
    {
        foreach (Transform child in lobbyListScrollView.content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateNewLobbyButtons(QueryResponse queryResponse)
    {
        for (int i = 0;i<queryResponse.Results.Count;i++)
        {
            Lobby lobby = queryResponse.Results[i];
            GameObject lobbyButton = Instantiate(lobbyButtonPrefab, lobbyListScrollView.content.transform);
            lobbyButton.GetComponent<LobbyCodeHolder>().SetCode(lobby.LobbyCode);

            float yPos = -i*lobbyButton.GetComponent<RectTransform>().rect.height;
            RectTransform buttonRectTransform = lobbyButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition=new Vector2(buttonRectTransform.anchoredPosition.x, yPos);

            lobbyButton.GetComponentInChildren<TMP_Text>().text=$"Lobby: {lobby.Name}";
        }
    }

    public async void DirectJoinLobby()
    {
        try
        {
            Unity.Services.Lobbies.Models.Player newPlayer = GetNewPlayer(); // Utilisez la méthode pour obtenir le nouveau joueur avec le nom sauvegardé

            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player=newPlayer
            };
            string lobbyCode = lobbyCodeInput.text;

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            // Utilisez le lobby comme nécessaire après avoir rejoint.
            Debug.Log("Rejoint le lobby! Nom : "+lobby.Name);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void InstantJoinLobby()
    {
        try
        {
            Unity.Services.Lobbies.Models.Player newPlayer = GetNewPlayer(); // Utilisez la méthode pour obtenir le nouveau joueur avec le nom sauvegardé

            QuickJoinLobbyOptions joinLobbyByCodeOptions = new QuickJoinLobbyOptions
            {
                Player=newPlayer
            };

            await LobbyService.Instance.QuickJoinLobbyAsync(joinLobbyByCodeOptions);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void SavePlayerName()
    {
        playerName = playerNameInput.text;
    }

    private void DisplayConnectedPlayers()
    {
        if (hostLobby!=null)
        {
            string connectedPlayersText = "Connected Players:\n";

            foreach (Unity.Services.Lobbies.Models.Player lobbyMember in hostLobby.Players)
            {
                // Vérifiez si le joueur a des données
                if (lobbyMember.Data!=null&&lobbyMember.Data.ContainsKey("PlayerName"))
                {
                    // Récupérez l'objet PlayerDataObject associé au nom du joueur
                    PlayerDataObject playerNameData = lobbyMember.Data["PlayerName"];

                    // Vérifiez si la visibilité est "public" ou "member"
                    if (playerNameData.Visibility==PlayerDataObject.VisibilityOptions.Public||
                        playerNameData.Visibility==PlayerDataObject.VisibilityOptions.Member)
                    {
                        // Ajoutez le nom du joueur au texte des joueurs connectés
                        connectedPlayersText+=playerNameData.Value+"\n";
                    }
                }
            }
            connectedPlayersCreated.text=connectedPlayersText;
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
        if (hostLobby!=null)
        {
            heartbeatTimer-=Time.deltaTime;
            if (heartbeatTimer<0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer=heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private void UpdateLobbyInfo(Lobby lobby)
    {
        lobbyCodeCreated.text=$"Lobby Code: {lobby.LobbyCode}";
        lobbyNameCreated.text=$"Lobby Name: {lobby.Name}";
    }

    private void ActivateCamera()
    {
        Camera.main.enabled=true;
    }

    private void DeactivateCamera()
    {
        Camera.main.enabled=false;
    }

    private void StartHost(NetworkManager networkManager)
    {
        if (networkManager.StartHost())
        {
            Debug.Log("Host started");
        }
        else
        {
            Debug.Log("Host failed to start");
        }
    }

    public void StartServer()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Server started");
        }
        else
        {
            Debug.Log("Server failed to Start");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
        }
        else
        {
            Debug.Log("Client failed to Start");
        }
    }
}