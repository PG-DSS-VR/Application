using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class LocalPlayerReference : GenericSingletonClass<LocalPlayerReference> {
    private GameObject _localPlayer;

    public GameObject LocalPlayer {
        get {
            if (_localPlayer is null) {
                Debug.LogError("Local player is not assigned");
            }

            return _localPlayer;
        }
        set => _localPlayer = value;
    }
    /*
        private void Start()
        {
            foreach (var asset in GetComponent<InputActionManager>().actionAssets)
            {
                Debug.Log(asset.name);
            }

        }

        private void Update()
        {
            Debug.Log(GetComponent<InputActionManager>().isActiveAndEnabled);
            foreach (var asset in GetComponent<InputActionManager>().actionAssets)
            {
                Debug.Log(asset.name);
            }
        }*/
}