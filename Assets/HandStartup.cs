using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandStartup : MonoBehaviour {
    public UnityEvent teleportStart;
    // Start is called before the first frame update
    void Start()
    {
        teleportStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
