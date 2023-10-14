using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine.Events;

public class StartRoom : MonoBehaviourPunCallbacks {

    #region Private Serializable Fields

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The Ui Text to inform the user about the connection progress")]
    [SerializeField]
    private GameObject feedbackText;

    [Tooltip("The input field where players enter a new room's name")]
    [SerializeField]
    private TMP_InputField newRoomNameInput;

    [Tooltip("The input field where players enter a room's name")]
    [SerializeField]
    private TMP_InputField joinRoomNameInput;

    [Tooltip("The input field where players enter their name")]
    [SerializeField]
    private TMP_InputField playerNicknameInput;

    [Tooltip("Indicates if room is private")]
    [SerializeField]
    private Toggle priv;

    [Tooltip("Indicates if room is public")]
    [SerializeField]
    private Toggle publ;

    public UnityEvent LoadingStarts;
    public UnityEvent CreatingFails;
    public UnityEvent JoiningFails;

    [SerializeField]
    public GameObject InitialScreen;
    [SerializeField]
    public GameObject JoinScreen;
    [SerializeField]
    public GameObject CreateScreen;
    [SerializeField]
    public GameObject EnvironmentScreen;
    [SerializeField]
    public GameObject Panel;
    [SerializeField]
    public GameObject BackSide;

    public string environmentName;


    public Toggle Offline;

    public Button JoinBtn;

    public TMP_Text ToggleWarning;

    #endregion

    #region Private Fields

    private bool isConnecting = false;
    private bool isCreatingRoom = false;

    private string gameVersion = "1";

    private string roomNameToConnectTo;

    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.gameVersion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region public methods


    public void checkToggle() {
        if (this.publ.isOn && this.priv.isOn) {
            ToggleWarning.gameObject.SetActive(true);
        } else {
            ToggleWarning.gameObject.SetActive(false);
        }
    }

    public void OfflineMode() {
        PhotonNetwork.OfflineMode = Offline.isOn;
        JoinBtn.enabled = !Offline.isOn;
    }

    private byte maxPlayer;


    private void CreateRoomInternal(string newRoomName) {
        Debug.Log("Trying to create room");

        //feedbackText.SetActive(true);
        //controlPanel.SetActive(false);

        maxPlayer = (byte)1;
        if (publ.isOn) {
            maxPlayer = (byte)20;
        }

        if(publ.isOn && priv.isOn) {
            Debug.Log("Not allowed to have both toggles active at the same time!");
            return;
        }

        isConnecting = true;
        isCreatingRoom = true;

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.CreateRoom(newRoomName, new RoomOptions { MaxPlayers = maxPlayer });
            if (PhotonNetwork.OfflineMode) {
                PhotonNetwork.LoadLevel(environmentName);
            }
        } else {
            CreatingFails.Invoke();
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    [ContextMenu("Create Room")]
    public void CreateRoom(string name) {
        environmentName = name;
        byte maxPlayer = (byte)1;
        if (publ.isOn) {
            maxPlayer = (byte)20;
        }

        if (publ.isOn && priv.isOn) {
            Debug.Log("Not allowed to have both toggles active at the same time!");
            return;
        }
        LoadingStarts.Invoke();
        CreateRoomInternal(newRoomNameInput.text);
    }

    public void Connect() {
        //feedbackText.SetActive(true);
        //controlPanel.SetActive(false);
        string roomName = joinRoomNameInput.text;

        roomNameToConnectTo = roomName;
        LoadingStarts.Invoke();


        isConnecting = true;
        isCreatingRoom = false;

        if (PhotonNetwork.IsConnected) {
            Debug.Log("Trying to join room " + roomName);
            PhotonNetwork.JoinRoom(roomName);
        } else {
            JoiningFails.Invoke();
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    public GameObject nameEmptyError;

    public void CreateRoomPanel() {
        if (string.IsNullOrWhiteSpace(playerNicknameInput.text)) {
            nameEmptyError.SetActive(true);
        } else {
            InitialScreen.SetActive(false);
            CreateScreen.SetActive(true);
            nameEmptyError.SetActive(false);
        }
    }

    public void ChooseEnvironment() {
        CreateScreen.SetActive(false);
        EnvironmentScreen.SetActive(true);
    }

    public void BackToInitialPanel() {
        CreateScreen.SetActive(false);
        JoinScreen.SetActive(false);
        EnvironmentScreen.SetActive(false);
        InitialScreen.SetActive(true);
    }

    public void ChooseFinish() {
        EnvironmentScreen.SetActive(false);
        Panel.SetActive(false);
        BackSide.SetActive(false);
    }

    public void PlayOffline() {
        InitialScreen.SetActive(false);
        OfflineMode();
        ChooseEnvironment();
    }

    public void JoinRoomPanel() {
        if (string.IsNullOrWhiteSpace(playerNicknameInput.text)) {
            nameEmptyError.SetActive(true);
        } else {
            InitialScreen.SetActive(false);
            JoinScreen.SetActive(true);
            nameEmptyError.SetActive(false);
        }
    }

    #endregion

    #region Monobehaviour PUN Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.LocalPlayer.NickName = playerNicknameInput.text;
        Debug.Log("OnConnectedToMaster(). Client is connected to master lobby and ready to join a room.");
        if (isConnecting) {
            if (isCreatingRoom) {
                PhotonNetwork.CreateRoom(roomNameToConnectTo,
                    new RoomOptions { });
            } else {
                Debug.Log("Trying to join a room " + roomNameToConnectTo);
                PhotonNetwork.JoinRoom(roomNameToConnectTo);
            }
        } else {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("No random room available. Creating a new one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
        isConnecting = false;
        isCreatingRoom = false;
        JoiningFails.Invoke();

        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("Join room failed: " + message);

        isConnecting = false;
        isCreatingRoom = false;
        JoiningFails.Invoke();

        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to create a room: " + message);

        isConnecting = false;
        isCreatingRoom = false;
        CreatingFails.Invoke();

        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);
    }

    /// <summary>
    /// Called after disconnecting from the Photon server.
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogError("Disconnected from Server.");
        if (!(feedbackText is null))
            feedbackText.gameObject.SetActive(false);
        if (!(controlPanel is null))
            controlPanel.SetActive(true);

        // #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
        isConnecting = false;
        isCreatingRoom = false;
    }

    /// <summary>
    /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
    /// </summary>
    /// <remarks>
    /// This method is commonly used to instantiate player characters.
    /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
    ///
    /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
    /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
    /// enough players are in the room to start playing.
    /// </remarks>
    public override void OnJoinedRoom() {
        Debug.Log("Client joined a room as Player " + PhotonNetwork.CurrentRoom.PlayerCount);

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        //--> Only executed after the room was created
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("Loading MainEnvironment");


            // #Critical
            // Load the Lobby Level. 
            PhotonNetwork.LoadLevel(environmentName);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        PhotonNetwork.LoadLevel("InitialScene");
        Destroy(this.gameObject);
    }

    #endregion
}
