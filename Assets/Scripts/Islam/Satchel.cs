using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Satchel : MonoBehaviour
{
    bool triggerEntered = false;
    GameObject currentPebble = null;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Muzdalifah")
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter (Collider other)
    {
        currentPebble = other.gameObject;
        if (currentPebble.tag == "Pebble") 
        {
            currentPebble.SetActive(false);
            //triggerEntered = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //triggerEntered = false;
        //currentPebble = null;
    }

    public void ReleasePebble ()
    {
        if (triggerEntered)
        {
            currentPebble.SetActive(false);
            currentPebble = null;
        }
    }
}
