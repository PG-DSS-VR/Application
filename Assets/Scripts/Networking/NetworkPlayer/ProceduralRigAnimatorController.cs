using UnityEngine;

namespace ProceduralNetworkPlayer.Scripts {
    public class ProceduralRigAnimatorController : MonoBehaviour {
        [Range(0, 1)] public float smoothing = 0.3f;

        private Animator animator;
        //public float speedThreshold = 0.1f;

        private Vector3 previousPos;

        private ProceduralRig proceduralRig;

        private float oldDirectionX = 0;

        private float oldDirectionY = 0;

        // Start is called before the first frame update
        void Start() {
            animator = GetComponent<Animator>();
            proceduralRig = GetComponent<ProceduralRig>();
            previousPos = proceduralRig.head.vrTarget.position;
        }

        // Update is called once per frame
        void Update() {
            var position = proceduralRig.head.vrTarget.position;
            Vector3 headsetSpeed = (position - previousPos) / Time.deltaTime;
            headsetSpeed.y = 0;
            Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
            previousPos = position;
            float directionX = Mathf.Clamp(headsetLocalSpeed.x, -1, 1);
            float directionY = Mathf.Clamp(headsetLocalSpeed.z, -1, 1);
            oldDirectionX = Mathf.Lerp(oldDirectionX, directionX, smoothing);
            oldDirectionY = Mathf.Lerp(oldDirectionY, directionY, smoothing);

            animator.SetFloat("DirectionX", oldDirectionX);
            animator.SetFloat("DirectionY", oldDirectionY);
        }
    }
}