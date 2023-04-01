using System;
using UnityEngine;

namespace ProceduralNetworkPlayer.Scripts {
    [Serializable]
    public class ProceduralObjectMap {
        public Transform vrTarget;
        public Transform rigTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public virtual void Map() {
            rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }
    [Serializable]
    public class ProceduralHeadMap : ProceduralObjectMap {
        public Transform cameraAttachmentPoint;

        public override void Map() {
            rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);//(vrTarget.position-cameraAttachmentPoint.position));// + trackingPositionOffset);
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }

    public class ProceduralRig : MonoBehaviour {
        public ProceduralHeadMap head;
        public ProceduralObjectMap leftHand;
        public ProceduralObjectMap rightHand;

        public Transform headConstraint;
        //public Transform headTransform;

        //[Space] public float zeroLevel;

        //public float ZeroLevel
        //{
        //    get => zeroLevel;
        //    set => zeroLevel = value;
        //}

        //public float kneelingHeight;
        //public float playerKneelingYPosition;

        private Vector3 headBodyOffset;

        //the head is close enough to the ground so that the player is kneeling
        private bool hasKneelingHeight;
        private Animator animator;
        private ProceduralRigFootIK footRig;
        [SerializeField] private float turnDelay;
        private Transform zeroLevelTransformPosition;

        [SerializeField] private bool canChangePosition = true;

        public bool CanChangePosition {
            get => canChangePosition;
            set => canChangePosition = value;
        }


        //[SerializeField] private Transform cameraHeight;
        //[SerializeField] private Transform armatureRoot;


        // Start is called before the first frame update
        void Start() {
            //TODO fix height scaling together with remote

            //use default model height as default
            float cameraPositionOnModel =
                head.cameraAttachmentPoint.parent.localPosition.y + head.cameraAttachmentPoint.localPosition.y;
            float heightScale = PlayerPrefs.GetFloat("PlayerHeight", 1.66f) / cameraPositionOnModel;
            transform.localScale *= heightScale;

            footRig = GetComponent<ProceduralRigFootIK>();
            animator = GetComponent<Animator>();
            //headBodyOffset = transform.position - headConstraint.position;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.magenta;
            var tran = transform.position;

            Gizmos.DrawLine(new Vector3(tran.x, 0, tran.z), new Vector3(tran.x, 0, tran.z + 1));
        }

        // Update is called once per frame
        void Update() {
            //height check
            //determine if the player is standing or kneeling
            /*if (headTransform.position.y - zeroLevel < kneelingHeight)
            {
                //only execute once, that the player starts kneeling
                if (!hasKneelingHeight)
                {
                    hasKneelingHeight = true;
                    var tmpPosition = transform.position;
                    tmpPosition.y = zeroLevel+playerKneelingYPosition;
                    transform.position = tmpPosition;
                    footRig.footIKEnabled = false;
                    animator.SetBool("isKneeling", true);
                }
            }
            else
            {
                if (hasKneelingHeight)
                {
                    footRig.footIKEnabled = true;
                    hasKneelingHeight = false;
                    animator.SetBool("isKneeling", false);
                }
            }
        */
            //var newTransformPosition = transform.position;
            var newHeadPosition = headConstraint.position + (transform.position - head.cameraAttachmentPoint.position);

            //player is kneeling, so only update x and z values and determine, if he is bending
            /*if (hasKneelingHeight)
            {
                newTransformPosition.x = newHeadPosition.x;
                newTransformPosition.z = newHeadPosition.z;
                transform.position = newTransformPosition;
            }*/
            //player is not kneeling, just update the position
            //else
            //{

            //}

            if (canChangePosition) {
                transform.position = newHeadPosition;
                transform.forward = Vector3.Lerp(transform.forward,
                    Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * turnDelay);
            } else {
                //transform.position = newHeadPosition;
                var currPosition = transform.position;
                currPosition.y = newHeadPosition.y;
                transform.position = currPosition;
                //transform.forward = Vector3.Lerp(transform.forward,
                //    Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * turnDelay);
            }

            head.Map();
            leftHand.Map();
            rightHand.Map();
        }

        public void SetPositionAndForwardOnce(Vector3 position, Vector3 forward) {
            transform.forward = forward;
            //transform.position = position;//+head.trackingPositionOffset;
            var currPosition = transform.position;
            currPosition.x = position.x;
            currPosition.z = position.z;
            transform.position = currPosition;

            //transform.TransformPoint(head.trackingPositionOffset);
        }
    }
}