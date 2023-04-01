using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonViewTakeover))]
public class InitObject : MonoBehaviour {
    //private ThrowingGameController _controller;
    private Rigidbody _rigidbody;

    private string _basketTag;
    private string _scoreBucketTag;

    private bool _inBasket;
    private bool _isInVelocityCheck;
    private bool _isAttached;

    private float _secondsToWaitForReset;
    private float _startTimeForVelocityCheck;
    private float _secondsToWaitInBucketUntilReset;

    private Vector3 _spawnPoint;

    private PhotonViewTakeover _photonViewTakeover;


    // Start is called before the first frame update
    void Start() {
        //_rigidbody = GetComponent<Rigidbody>();
        _photonViewTakeover = GetComponent<PhotonViewTakeover>();
    }

    // Update is called once per frame
    void Update() {
        //if (!_inBasket && !_isAttached) {
        //    CheckForVelocity();
        //}
    }

    //public void Initialize(ThrowingGameController controller, string basketTag, string scoreBucketTag,
    //    float secondsToWaitForReset, Vector3 spawnPosition, float secondsToWaitInBucketUntilReset) {
    //    _controller = controller;
    //    _basketTag = basketTag;
    //    _scoreBucketTag = scoreBucketTag;
    //    _secondsToWaitForReset = secondsToWaitForReset;
    //    _spawnPoint = spawnPosition;
    //    _secondsToWaitInBucketUntilReset = secondsToWaitInBucketUntilReset;
    //}

    public void AttachToHand() {
        _isAttached = true;
        _photonViewTakeover.TakeOwnership();
    }
    public void DetachFromHand() {
        _isAttached = false;
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag(_basketTag)) {
    //        BasketEntered(other);
    //    } else if (other.CompareTag(_scoreBucketTag)) {
    //        BucketEntered(other);
    //    }
    //}
    //private void BucketEntered(Collider other) {
    //    _controller.ProjectileEnteredBucket(other.GetComponent<ThrowingGameBucket>().Score);
    //    StartCoroutine(WaitForRespawn());
    //    other.GetComponent<ThrowingGameBucket>().ProjectileEntered.Invoke();
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (other.CompareTag(_basketTag)) {
    //        BasketExit(other);
    //    } else if (other.CompareTag(_scoreBucketTag)) {
    //        BucketExit(other);
    //    }
    //}

    private void CheckForVelocity() {
        if (_rigidbody.velocity.sqrMagnitude == 0.0f) {
            if (_isInVelocityCheck) {
                if (Time.time - _startTimeForVelocityCheck >= _secondsToWaitForReset) {
                    _isInVelocityCheck = false;
                    _rigidbody.velocity = Vector3.zero;
                    transform.position = _spawnPoint;
                }
            } else {
                _isInVelocityCheck = true;
                _startTimeForVelocityCheck = Time.time;
            }
        } else {
            _isInVelocityCheck = false;
        }
    }

    private void BasketEntered(Collider other) {
        _inBasket = true;

    }



    private IEnumerator WaitForRespawn() {
        yield return new WaitForSeconds(_secondsToWaitInBucketUntilReset);
        _rigidbody.velocity = Vector3.zero;
        transform.position = _spawnPoint;
    }

    //private void BasketExit(Collider other) {
    //    _inBasket = false;
    //    _controller.ProjectileLeftBasket();
    //}

    private void BucketExit(Collider other) {

    }
}