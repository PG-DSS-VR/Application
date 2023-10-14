using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayerRPCs : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void MasterExitRoom() {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    public void Abort() {
        GameObject bot = GameObject.Find("christ_bot");
        bot.GetComponent<followPlayer>().AbortAnimation();
    }

    //[PunRPC]
    //public void Pause() {
    //    GameObject bot = GameObject.Find("christ_bot");
    //    bot.GetComponent<followPlayer>().PauseAnimation();
    //}

    [PunRPC]
    public void Entrance() {
        GameObject bot = GameObject.Find("christ_bot");
        bot.GetComponent<followPlayer>().entranceAnimationRPC();
    }

    [PunRPC]
    public void Confession() {
        GameObject bot = GameObject.Find("christ_bot");
        bot.GetComponent<followPlayer>().confessionAnimationRPC();
    }

    [PunRPC]
    public void ConfessionTwo() {
        GameObject bot = GameObject.Find("christ_bot");
        bot.GetComponent<followPlayer>().botSitInConfessionRPC();
    }
}
