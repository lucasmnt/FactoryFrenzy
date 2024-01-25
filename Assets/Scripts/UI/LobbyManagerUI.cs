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

public class LobbyManagerUI : MonoBehaviour
{
    // gameObjects holding pannels
    public GameObject lobbyMainPannel;
    public GameObject lobbyCreationPanel;
    public GameObject lobbyJoinPanel;
    public GameObject LobbyConnectedPannel;

    // create params
    public TMP_InputField lobbyNameInput;
    public TMP_InputField maxPlayersInput;
    public Toggle privateToggle;
    private Lobby hostLobby;
    private float heartbeatTimer;
    
    // connected params
    public TMP_Text lobbyCodeCreated;
    public TMP_Text lobbyNameCreated;
    public TMP_Text connectedPlayersCreated;
    public ScrollRect lobbyListScrollView;

    // join params
    public TMP_InputField lobbyCodeInput;
    public GameObject lobbyButtonPrefab;

    // player params
    public TMP_InputField playerNameInput;
    private string playerName;

    // launching params
    public GameObject networkManagerRef;

    // Start is called before the first frame update
    async void Start()
    {
        playerNameInput.text="Roger"+UnityEngine.Random.Range(0, 99);

        OpenMainLobbyPanel();

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn+=() => {
            Debug.Log("Signed in "+AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    public void OpenCreateLobbyPanel()
    {
        lobbyCreationPanel.SetActive(true);
        lobbyJoinPanel.SetActive(false);
        lobbyMainPannel.SetActive(false);
        LobbyConnectedPannel.SetActive(false);
        networkManagerRef.SetActive(false);
    }

    public void OpenJoinLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(true);
        lobbyMainPannel.SetActive(false);
        LobbyConnectedPannel.SetActive(false);
        networkManagerRef.SetActive(false);
    }

    public void OpenMainLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(false);
        lobbyMainPannel.SetActive(true);
        LobbyConnectedPannel.SetActive(false);
        networkManagerRef.SetActive(false);
    }

    public void OpenConnectedLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(false);
        lobbyMainPannel.SetActive(false);
        LobbyConnectedPannel.SetActive(true);
        networkManagerRef.SetActive(true);
    }

    public void LaunchGame()
    {
        try
        {
            // Assurez-vous que networkManagerRef n'est pas nul avant d'accéder à sa propriété SceneManager
            if (networkManagerRef!=null)
            {
                // Assurez-vous d'avoir une référence à votre NetworkManager
                NetworkManager networkManager = networkManagerRef.GetComponent<NetworkManager>();

                // Si le joueur est l'hôte, lancez le serveur (Host)
                //if (networkManager.IsHost)
                //{
                    StartHost();
                //}
                // Si le joueur est un client, lancez le client
                //else
                //{
                //    StartClient();
                //}

                // Chargez la scène de jeu
                // Utilisez le réseau pour gérer le chargement de la scène si nécessaire
                networkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
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
                IsPrivate = isPrivate,
                Player = newPlayer
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, lobbyMaxPlayers, createLobbyOptions);
            hostLobby = lobby;

            lobbyCodeCreated.text = "Lobby Code : " + lobby.LobbyCode;
            lobbyNameCreated.text = "Lobby Name : " + lobby.Name;

            // Utilisez le lobby comme nécessaire, par exemple, afficher le lobbyCode.
            Debug.Log(" lobbyMaxPlayers : "+lobbyMaxPlayers+", isPrivate : "+createLobbyOptions.IsPrivate.Value);
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

            Debug.Log("Lobbies found: "+queryResponse.Results.Count);

            // Détruit tous les anciens boutons de lobby
            foreach (Transform child in lobbyListScrollView.content.transform)
            {
                Destroy(child.gameObject);
            }

            // Crée des nouveaux boutons de lobby
            for (int i = 0;i<queryResponse.Results.Count;i++)
            {
                Lobby lobby = queryResponse.Results[i];

                GameObject lobbyButton = Instantiate(lobbyButtonPrefab, lobbyListScrollView.content.transform);
                lobbyButton.GetComponent<LobbyCodeHolder>().SetCode(lobby.LobbyCode);

                // Ajuster la position y en fonction de l'index
                float buttonHeight = lobbyButton.GetComponent<RectTransform>().rect.height;
                float yPos = -i*buttonHeight;

                RectTransform buttonRectTransform = lobbyButton.GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition=new Vector2(buttonRectTransform.anchoredPosition.x, yPos);

                // Assurez-vous de configurer correctement le texte du bouton avec les informations du lobby
                lobbyButton.GetComponentInChildren<TMP_Text>().text="Lobby: "+lobby.Name;
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby!=null)
        {
            heartbeatTimer-=Time.deltaTime;
            if (heartbeatTimer<0f)
            {
                float heatbeatTimerMax = 15;
                heartbeatTimer=heatbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
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

    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started");
        }
        else
        {
            Debug.Log("Host failed to Start");
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