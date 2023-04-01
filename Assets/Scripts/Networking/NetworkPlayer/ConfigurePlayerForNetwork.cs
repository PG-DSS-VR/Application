using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class ConfigurePlayerForNetwork : MonoBehaviour {
    [Header("Local Player")]
    [SerializeField]
    private List<GameObject> localPlayerEnabled;

    [SerializeField] private List<GameObject> localPlayerDisabled;

    [Space, Header("Network Player")]
    [SerializeField]
    private List<GameObject> networkPlayerEnabled;

    [SerializeField] private List<GameObject> networkPlayerDisabled;

    public UnityEvent localPlayerEnabledEvent;
    public UnityEvent networkPlayerEnabledEvent;


    private PhotonView _photonView;

    // Start is called before the first frame update
    void Awake() {
        _photonView = GetComponent<PhotonView>();

        //Player is the local player
        if (_photonView.IsMine) {
            foreach (var go in localPlayerEnabled) {
                go.SetActive(true);
            }
            localPlayerEnabledEvent.Invoke();

            foreach (var go in localPlayerDisabled) {
                go.SetActive(false);
            }
        }
        //Player is Network player
        else {
            foreach (var go in networkPlayerDisabled) {
                go.SetActive(false);
            }
            networkPlayerEnabledEvent.Invoke();

            foreach (var go in networkPlayerEnabled) {
                go.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update() {
    }
}