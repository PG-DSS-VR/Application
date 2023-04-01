using System;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spawnRadius;



    // Start is called before the first frame update
    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference.", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer");
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

            //TODO check height of ground and spawn the player on the ground
            Vector2 positionInCircle = spawnRadius * UnityEngine.Random.insideUnitCircle;
            Vector3 spawnPosition = transform.position + new Vector3(positionInCircle.x, 0, positionInCircle.y);
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0);
        }

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

    }

    // Update is called once per frame
    void Update() {

    }
}
