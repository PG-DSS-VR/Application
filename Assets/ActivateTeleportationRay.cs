using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleporation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    void Update()
    {
        leftTeleportation.SetActive(leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleporation.SetActive(rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
