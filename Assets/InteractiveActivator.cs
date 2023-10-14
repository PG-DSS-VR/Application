using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractiveActivator : MonoBehaviour
{
    public GameObject element;
    public GameObject prayerPOI = null;
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

    public void OnTriggerEnter(Collider other)
    {
        view.RPC("InternalActivator", RpcTarget.All);
    }

    [PunRPC]
    public void InternalActivator()
    {
        if (!GlobalVarsIslam.interactiveElementActive)
        {
            element.SetActive(true);
            if (prayerPOI)
            {
                prayerPOI.SetActive(true);
            }
        }
    }
}