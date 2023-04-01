using Photon.Pun;
using UnityEngine;

namespace ProceduralNetworkPlayer.Scripts {
    public class RegisterLocalPlayer : MonoBehaviour {
        // Start is called before the first frame update
        void Awake() {
            if (GetComponent<PhotonView>().IsMine) {
                LocalPlayerReference.Instance.LocalPlayer = gameObject;
            }
        }
    }
}