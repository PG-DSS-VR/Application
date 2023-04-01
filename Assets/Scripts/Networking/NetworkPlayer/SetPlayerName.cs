using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class SetPlayerName : MonoBehaviour {
    [SerializeField] private TextMeshPro playerName;
    // Start is called before the first frame update
    void Start() {
        playerName.text = GetComponent<PhotonView>().Owner.NickName;
    }

    // Update is called once per frame
    void Update() {

    }
}
