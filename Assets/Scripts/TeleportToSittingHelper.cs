using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportToSittingHelper : MonoBehaviour
{
    [SerializeField] private TeleportationAnchor teleportationAnchor;
    [SerializeField] private string sitDownAnimationTrigger;
    [SerializeField] private string standUpAnimationTrigger;
    [SerializeField] private Transform sittingPosition;
    public XROrigin player;

    private PhotonView _playerOnSeat;
    private int _playerOnSeatViewID;
    //private SittingGroup _sittingGroup;
    private int _id;

    private Vector3 oldValue;

    

    public void Initialize()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OpenXRPlayerCameraChild"))
        {
        }
    }

    public void OnTeleportHere(TeleportingEventArgs e)
    {
        player = GetComponent<XROrigin>();
        _playerOnSeatViewID = _playerOnSeat.ViewID;
        //say everyone, that the player is no on my seat
        //say all others, that this player is no longer on their seat
        //_playerOnSeat.GetComponent<NetworkPlayerRPCs>().DoBufferedRPCCallToAll("SitDownPlayer", sitDownAnimationTrigger,
        //    sittingPosition.position, sittingPosition.forward);
        //SET HEIGHT!
        var movement = player.GetComponent<ContinuousMoveProviderBase>();
        movement.enabled = false;
        var currPosition = player.transform.position;
        currPosition.x = sittingPosition.transform.position.x;
        currPosition.z = sittingPosition.transform.position.z;
        player.transform.position = currPosition;
        Debug.Log("SitDown");
    }


    public void FreeSeat(int photonViewID, int newSeatID)
    {
        Debug.Log("Free Seat called on "+_id);
        if (_playerOnSeatViewID!=-1 && _playerOnSeatViewID == photonViewID)
        {
            //
            Debug.Log("Free Seat executed on "+_id+" newseat: "+newSeatID);
            //Player is standing somewhere else
            if (newSeatID == -1)
            {
                Debug.Log("Free Seat executed with no new seat on "+_id);

                player = GetComponent<XROrigin>();
                _playerOnSeatViewID = _playerOnSeat.ViewID;
                //say everyone, that the player is no on my seat
                //say all others, that this player is no longer on their seat
                //_playerOnSeat.GetComponent<NetworkPlayerRPCs>().DoBufferedRPCCallToAll("SitDownPlayer", sitDownAnimationTrigger,
                //    sittingPosition.position, sittingPosition.forward);
                //SET HEIGHT!
                var movement = player.GetComponent<ContinuousMoveProviderBase>();
                movement.enabled = true;
                var currPosition = player.transform.position;
                oldValue = currPosition;
                currPosition.x = sittingPosition.transform.position.x;
                currPosition.z = sittingPosition.transform.position.z;
                player.transform.position = currPosition;
                Debug.Log("StandUp with no seat");
            }
            //Player sits on a new Seat, do not stand up, but free that seat
            else if (newSeatID != _id)
            {
                Debug.Log("Free Seat executed with new seat on "+_id);
                teleportationAnchor.enabled = true;
                _playerOnSeatViewID = -1;
                _playerOnSeat = null;
                Debug.Log("StandUp");
            }
        }
    }


    public void BlockSeat(int playerOnSeatViewID)
    {
        _playerOnSeatViewID = playerOnSeatViewID;
        teleportationAnchor.enabled = false;
    }
}