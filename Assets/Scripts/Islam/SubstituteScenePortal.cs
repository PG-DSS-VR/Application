using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SubstituteScenePortal : MonoBehaviour
{
    public string newScene;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = PhotonView.Get(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "sceneTrigger" && GlobalVarsIslam.interactiveElementActive == false)
        {
            if (PhotonNetwork.IsMasterClient) {
                triggerCalled();
            } else {
                PhotonView view = PhotonView.Get(this);
                view.RPC("triggerCalled", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    public void triggerCalled() {
        PhotonNetwork.LoadLevel(newScene);
    }
}
