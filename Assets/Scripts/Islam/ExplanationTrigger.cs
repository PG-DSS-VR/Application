using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using Photon.Pun;

public class ExplanationTrigger : XRBaseInteractor
{
    public GameObject bot;
    public string triggerName;
    public bool onTriggerExit;
    public GameObject portalWithTrigger;

    PhotonView view;
    GameObject poi;

    // Start is called before the first frame update
    void Start()
    {
        if (portalWithTrigger != null)
        {
            view = PhotonView.Get(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        poi = null;
        if (this.gameObject.name.Contains("POI"))
        {
            poi = this.gameObject;
        }

        if (!GlobalVarsIslam.interactiveElementActive)
        {
            if (onTriggerExit)
            {
                Debug.Log(this.gameObject.name + ":" + other.gameObject.name + ",Exit");
                bot.GetComponent<followPlayerIslam>().PlayTriggerAudio(triggerName, poi);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        poi = null;
        if (this.gameObject.name.Contains("POI"))
        {
            poi = this.gameObject;
        }

        if (!GlobalVarsIslam.interactiveElementActive)
        {
            if (!onTriggerExit)
            {
                Debug.Log(this.gameObject.name + ":" + other.gameObject.name + ",Enter");
                bot.GetComponent<followPlayerIslam>().PlayTriggerAudio(triggerName, poi);
            }

            if (portalWithTrigger != null)
            {
                this.gameObject.SetActive(false);
                view.RPC("InternalActivatePortal", RpcTarget.All);
            }
        }
        else
        {
            GameObject player = FindObjectOfType<XROrigin>().transform.parent.gameObject;
            player.GetComponent<showInteractiveWarning>().ShowWarning();
        }
    }

    [PunRPC]
    public void InternalActivatePortal()
    {
        portalWithTrigger.SetActive(true);
    }
}
