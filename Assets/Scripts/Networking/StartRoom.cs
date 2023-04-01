using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using System;

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

    //TODO make customizable through script
    [Tooltip("The dropdown where players can choose the Environment")]
    [SerializeField]
    private TMP_Dropdown environment;

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
        if (this.publ.enabled && this.priv.enabled) {
            ToggleWarning.gameObject.SetActive(true);
        } else {
            ToggleWarning.gameObject.SetActive(false);
        }
    }

    public void OfflineMode() {
        PhotonNetwork.OfflineMode = Offline.isOn;
        JoinBtn.enabled = !Offline.isOn;
    }
   
    private void CreateRoomInternal(string newRoomName) {
        Debug.Log("Trying to create room");

        //feedbackText.SetActive(true);
        //controlPanel.SetActive(false);

        roomNameToConnectTo = newRoomName;
        byte maxPlayer = (byte)1;
        if (publ.enabled) {
            maxPlayer = (byte)20;
        }

        if(publ.enabled && priv.enabled) {
            Debug.Log("Not allowed to have both toggles active at the same time!");
            return;
        }

        isConnecting = true;
        isCreatingRoom = true;

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.CreateRoom(newRoomName, new RoomOptions { MaxPlayers = maxPlayer });
            if (PhotonNetwork.OfflineMode) {
                PhotonNetwork.LoadLevel(environment.captionText.text);
            }
        } else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    [ContextMenu("Create Room")]
    public void CreateRoom() {
        CreateRoomInternal(newRoomNameInput.text);
    }

    public void Connect() {
        //feedbackText.SetActive(true);
        //controlPanel.SetActive(false);

        string roomName = joinRoomNameInput.text;

        roomNameToConnectTo = roomName;

        isConnecting = true;
        isCreatingRoom = false;

        if (PhotonNetwork.IsConnected) {
            Debug.Log("Trying to join room " + roomName);
            PhotonNetwork.JoinRoom(roomName);
        } else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
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

        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("Join room failed: " + message);

        isConnecting = false;
        isCreatingRoom = false;

        //feedbackText.gameObject.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to create a room: " + message);

        isConnecting = false;
        isCreatingRoom = false;

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
            PhotonNetwork.LoadLevel(environment.captionText.text);
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
