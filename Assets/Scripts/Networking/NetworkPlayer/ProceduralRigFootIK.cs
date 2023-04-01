using UnityEngine;

namespace ProceduralNetworkPlayer.Scripts {
    public class ProceduralRigFootIK : MonoBehaviour {
        private Animator animator;

        [Range(0, 1)] public float rightFootPosWeight = 1;
        [Range(0, 1)] public float leftFootPosWeight = 1;
        [Range(0, 1)] public float leftFootRotWeight = 1;
        [Range(0, 1)] public float rightFootRotWeight = 1;

        public float footOffsetY;
        public bool footIKEnabled;

        public LayerMask layerMask;

        // Start is called before the first frame update
        void Start() {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex) {
            if (footIKEnabled) {
                Vector3 rightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);

                if (Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out var hit, 2, layerMask.value)) {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + Vector3.up * footOffsetY);

                    Quaternion rightFootRotation =
                        Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
                } else {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                }

                Vector3 leftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                if (Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out var hit2, 2, layerMask.value)) {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit2.point + Vector3.up * footOffsetY);

                    Quaternion leftFootRotation =
                        Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit2.normal), hit2.normal);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
                } else {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                }
            }
        }
    }
}