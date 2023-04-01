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
    public void MasterExitRoom()
    {
        StartCoroutine(Exit());
    }

    IEnumerator Exit() {
        if (PhotonNetwork.IsMasterClient) {
            yield return new WaitForSecondsRealtime(1);
        }
        PhotonNetwork.LeaveRoom();
    }
}
