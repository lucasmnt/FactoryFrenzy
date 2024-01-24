using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using TMPro;
using Unity.Services.Core;

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

    // Préfab de bouton de lobby (ou texte)
    

    


    private string playerName;

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
    }

    public void OpenJoinLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(true);
        lobbyMainPannel.SetActive(false);
        LobbyConnectedPannel.SetActive(false);
    }

    public void OpenMainLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(false);
        lobbyMainPannel.SetActive(true);
        LobbyConnectedPannel.SetActive(false);
    }

    public void OpenConnectedLobbyPanel()
    {
        lobbyCreationPanel.SetActive(false);
        lobbyJoinPanel.SetActive(false);
        lobbyMainPannel.SetActive(false);
        LobbyConnectedPannel.SetActive(true);
    }


    public async void CreateLobby()
    {
        try
        {
            string lobbyName = lobbyNameInput.text;
            int lobbyMaxPlayers = int.Parse(maxPlayersInput.text);
            bool isPrivate = privateToggle.isOn;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate=isPrivate
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

    public async void UpdateLobby()
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

            Debug.Log("Lobbies found : "+queryResponse.Results.Count);

            // Détruit tous les anciens boutons de lobby
            foreach (Transform child in lobbyListScrollView.content.transform)
            {
                Destroy(child.gameObject);
            }

            // Crée des nouveaux boutons de lobby
            foreach (Lobby lobby in queryResponse.Results)
            {
                GameObject lobbyButton = Instantiate(lobbyButtonPrefab, lobbyListScrollView.content.transform);
                // Assurez-vous de configurer correctement le bouton avec les informations du lobby
                // Vous pouvez utiliser lobbyButton.GetComponent<YourButtonScript>().Setup(lobby);
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
        string lobbyCode = lobbyCodeInput.text;

        try
        {
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);

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
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void SelectedJoinLobby()
    {
        try
        {

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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
                connectedPlayersText+=lobbyMember.Id;
            }

            connectedPlayersCreated.text =connectedPlayersText;
        }
    }
}