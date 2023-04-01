using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class TeleportController : MonoBehaviour
{
    public GameObject baseController;
    public GameObject teleportation;

    public InputActionReference teleportationActive;

    [Space]
    public UnityEvent teleportStart;
    public UnityEvent teleportEnd;
    public UnityEvent teleportEndKeyboard;

    public void Start()
    {
        teleportationActive.action.performed += TeleporModeActivate;
        teleportationActive.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        if (KeyboardManager.keyboardActive) {
            Invoke("DeactivateTeleporterKeyboard", .1f);
        } else {
            Invoke("DeactivateTeleporter", .1f);
        }
    }

    void DeactivateTeleporter() {
        teleportEnd.Invoke();
    }

    void DeactivateTeleporterKeyboard() {
        teleportEndKeyboard.Invoke();
    }

    private void TeleporModeActivate(InputAction.CallbackContext obj)
    {
        teleportStart.Invoke();
    }
}
