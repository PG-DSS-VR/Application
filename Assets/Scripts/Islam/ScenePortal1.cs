using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ScenePortal1 : XRBaseInteractor
{
    //public Button button;
    public GameObject substituteCollider;
    public GameObject trigger;

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
        //if (other.gameObject.tag == "portalTrigger") {
            //Debug.Log("Trigger:" + other.gameObject.name);
            //trigger.transform.position = substituteCollider.transform.position;
        //}
    }
}
