using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviourPun
{
    public UnityEvent showFirstMap;
    public UnityEvent showSecondMap;
    public UnityEvent showThirdMap;
    public UnityEvent showFourthMap;
    public UnityEvent showFifthMap;
    public UnityEvent showSixthMap;

    // Start is called before the first frame update
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "Safa_Marwa":
            case "Kaaba":
            case "Kaaba_Inside":
            case "Landing_Room":
                showFirstMap.Invoke();
                break;
            case "Tent_City":
                showSecondMap.Invoke();
                break;
            case "Pillars_Current":
            case "Pillars_Previous":
                showFifthMap.Invoke();
                break;
            case "Arafat":
                showThirdMap.Invoke();
                break;
            case "Muzdalifah":
                showFourthMap.Invoke();
                break;
            case "Departure_Room":
                showSixthMap.Invoke();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
