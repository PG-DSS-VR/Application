using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCanvas : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreasePebbleCount(int pebbleCount)
    {
        string message = "";

        if (pebbleCount < 7)
        {
            message = $"{pebbleCount} of 7 stones thrown!";
        }
        else if (pebbleCount == 7)
        {
            message = $"All stones have been thrown. You completed the ritual!";
        } else
        {
            message = $"You already completed the ritual, no more stones need to be thrown!";
        }

        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }
}
