using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NewLevel : MonoBehaviour
{

    private bool loading = false;
    // Start is called before the first frame update
    void Start()
    {
        loading = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void PhotonLoadNewScene(string name) {
        if (loading) {

        } else {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.LoadLevel(name);
                loading = true;
            } else {
                PhotonView view = PhotonView.Get(this);
                view.RPC("PhotonLoadNewScene", RpcTarget.MasterClient, name);
            }
        }
    }
}
