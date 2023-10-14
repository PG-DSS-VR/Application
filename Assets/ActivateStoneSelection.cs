using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateStoneSelection : MonoBehaviour
{
    public GameObject stoneSelection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        stoneSelection.SetActive(true);
    }
}
