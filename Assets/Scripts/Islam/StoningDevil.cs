using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class StoningDevil : MonoBehaviour
{
    //public GameObject testCube;
    public GameObject bot;
    public GameObject canvas;
    GameObject player;

    ///*public*/ TMPro.TextMeshProUGUI statOverlay;

    int pebbleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject canvas = GameObject.Find("StoneCounterCanvas");
        //statOverlay = canvas.GetComponent<TMPro.TextMeshProUGUI>();
        //Debug.Log("StoneCounter: " + statOverlay);
        //statOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pebble") 
        {
            pebbleCount++;
            //testCube.SetActive(true);
            other.gameObject.SetActive(false);
            bot.GetComponent<followPlayerIslam>().PlayTriggerAudio("stoneThrowing", null);
            player = FindObjectOfType<XROrigin>().transform.parent.gameObject;
            player.GetComponent<StoneCountHandler>().IncreasePebbleCount(pebbleCount);
        }
    }
}
