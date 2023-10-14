using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class HajjEntrance : XRBaseInteractor
{
    public GameObject entrance;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //entrance.SetActive(true);
        //PhotonNetwork.LoadLevel("Tent_City");
        //entrance.SetActive(true);
        button.onClick.Invoke();
    }
}
