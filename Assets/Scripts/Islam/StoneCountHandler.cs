using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoneCountHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI display;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Pillars_Current")
        {
            display.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void IncreasePebbleCount(int pebbleCount)
    {
        display.GetComponent<StoneCanvas>().IncreasePebbleCount(pebbleCount);
    }
}
