using Photon.Pun;
using UnityEngine;

public class PhotonViewTakeover : MonoBehaviour {
    [SerializeField] private PhotonView photonView;


    public void TakeOwnership() {
        Debug.Log("Took Ownership");
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }
}