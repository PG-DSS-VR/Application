using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class CatholicPort : XRBaseInteractor
{
    public UnityEvent loading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        loading.Invoke();
    }
}
