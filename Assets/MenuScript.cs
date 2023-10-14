using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Samples.VrHoops;
using Photon.Pun;
using Photon.Realtime;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviourPun
{
    public UnityEvent openMenu;
    public UnityEvent closeMenu;
    public UnityEvent showFPS;
    public UnityEvent hideFPS;
    public UnityEvent showMap;
    public UnityEvent hideMap;

    public PhotonView _photonView;

    private bool menuOpen = false;
    private bool fpsOpen = false;
    private bool mapVisible = false;
    private bool previousGripPressed = false;
    GameObject islamTourguide;
    GameObject catholicTourguide;

    //public GameObject satchel;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Catholic" && SceneManager.GetActiveScene().name != "Catholic_church") {
            islamTourguide = GameObject.Find("Tourguide Islam").transform.GetChild(0).gameObject;
        } else {
            catholicTourguide = GameObject.Find("christ_bot");
        }
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        bool isPressedButton = OVRInput.GetDown(OVRInput.Button.Start);
        bool showFPSButton = OVRInput.GetDown(OVRInput.Button.One);
        bool buttonTwoPressed = OVRInput.GetDown(OVRInput.Button.Two);
        bool buttonThreePressed = OVRInput.GetDown(OVRInput.Button.Three);
        bool buttonFourPressed = OVRInput.GetDown(OVRInput.Button.Four);
        //bool gripPressed = OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
        if (isPressedButton) {
            StartCoroutine(HandleMenuPress());
        }
        if (showFPSButton) {
            StartCoroutine(HandleButtonOnePress());
        }
        if (buttonTwoPressed)
        {
            StartCoroutine(HandleButtonTwoPress());
        }
        if (buttonThreePressed)
        {
            StartCoroutine(HandleButtonThreePress());
        }
        if (buttonThreePressed) {
            StartCoroutine(HandleButtonFourPress());
        }
        //if (previousGripPressed && !gripPressed) 
        //{
        //    HandleGripReleased();
        //    previousGripPressed = gripPressed;
        //}
    }

    private IEnumerator HandleMenuPress() {
        if (menuOpen) {
            closeMenu.Invoke();
            menuOpen = false;
            yield return new WaitForSeconds(1.0f);
        } else {
            openMenu.Invoke();
            menuOpen = true;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator HandleButtonOnePress() {
        if (fpsOpen) {
            hideFPS.Invoke();
            fpsOpen = false;
            yield return new WaitForSeconds(1.0f);
        } else {
            showFPS.Invoke();
            fpsOpen = true;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator HandleButtonTwoPress()
    {
        if (SceneManager.GetActiveScene().name != "Islam")
        {

            if (mapVisible)
            {
                hideMap.Invoke();
                mapVisible = false;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                showMap.Invoke();
                mapVisible = true;
                yield return new WaitForSeconds(1.0f);
            }
        } else
        {
            GameObject bot = GameObject.Find("Bot");
            if (!bot)
            {
                bot = GameObject.Find("Bot Regular");
            }
            if(islamTourguide != null) {
                islamTourguide.GetComponent<followPlayerIslam>().StopAudio();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator HandleButtonThreePress() {
        if (islamTourguide != null) {
            islamTourguide.GetComponent<followPlayerIslam>().StopAudio();
        }
        if(catholicTourguide != null) {
            catholicTourguide.GetComponent<followPlayer>().AbortAnimation();
            PhotonView view = FindObjectOfType<XROrigin>().GetComponentInParent<PhotonView>();
            view.RPC("Abort", RpcTarget.All);
        }
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator HandleButtonFourPress() {
        if (islamTourguide != null) {
            islamTourguide.GetComponent<followPlayerIslam>().StopAudio();
        }
        if (catholicTourguide != null) {
            catholicTourguide.GetComponent<followPlayer>().PauseAnimation();
            PhotonView view = FindObjectOfType<XROrigin>().GetComponentInParent<PhotonView>();
            //view.RPC("Pause", RpcTarget.All);
        }
        yield return new WaitForSeconds(1.0f);
    }

    //private void HandleGripReleased()
    //{
    //    if (SceneManager.GetActiveScene().name == "Muzdalifah")
    //    {
    //        satchel.GetComponent<Satchel>().ReleasePebble();
    //    }
    //}


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
            if (PhotonNetwork.OfflineMode) {
                PhotonNetwork.LoadLevel("InitialScene");
                return;
            }
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
        if (PhotonNetwork.OfflineMode) {
            PhotonNetwork.LoadLevel("InitialScene");
            return;
        }
        if (PhotonNetwork.IsMasterClient) {
            _photonView.RPC("MasterExitRoom", RpcTarget.All);
        }
    }

    [PunRPC]
    public void StartRoom() {
        if (SceneManager.GetActiveScene().name.Contains("Islam")){
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.LoadLevel("Islam");
            } else {
                _photonView.RPC("StartRoom", RpcTarget.MasterClient);
            }
        } else if (SceneManager.GetActiveScene().name.Contains("Catholic")) {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.LoadLevel("Catholic");
            } else {
                _photonView.RPC("StartRoom", RpcTarget.MasterClient);
            }
        }
    }
}
