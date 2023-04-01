using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviourPun
{
    public UnityEvent openMenu;
    public UnityEvent closeMenu;

    public PhotonView _photonView;

    private bool menuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        bool isPressedButton = OVRInput.Get(OVRInput.Button.Start);
        if (isPressedButton) {
            if (menuOpen) {
                closeMenu.Invoke();
            } else {
                openMenu.Invoke();
            }
        }
    }


    //public void ExitRoom() {
    //    if (PhotonNetwork.IsMasterClient) {
    //        photonView.RPC("MasterExitRoom", RpcTarget.All);
    //    } else {
    //        PhotonNetwork.LeaveRoom();
    //        PhotonNetwork.LoadLevel("InitialScene");
    //    }
    //}
    public void ExitRoom() {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1) {
            MigrateMaster();
        } else {
            PhotonNetwork.LeaveRoom();
        }
    }

    private void MigrateMaster() {
        var dict = PhotonNetwork.CurrentRoom.Players;
        if (PhotonNetwork.SetMasterClient(dict[dict.Count - 1]))
            PhotonNetwork.LeaveRoom();
        //_photonView.RPC("MasterExitRoom", RpcTarget.All);
    }

    public void CloseRoom() {
        if (PhotonNetwork.IsMasterClient) {
            _photonView.RPC("MasterExitRoom", RpcTarget.All);
        }
    }
}
