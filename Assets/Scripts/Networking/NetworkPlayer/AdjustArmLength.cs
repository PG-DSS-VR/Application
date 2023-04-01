using UnityEngine;

public class AdjustArmLength : MonoBehaviour
{
    [SerializeField] private Transform lowerArm;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform target;
    [SerializeField] private Axis localAxisToScale;
    [SerializeField] private CalculateArmLength armLengthOnModelProvider;

    private float _maxPlayerArmLength;
    private float _armLengthOnModel;
    private Vector3 _localScaleLower;

    

    public enum Axis
    {
        x,
        y,
        z
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _maxPlayerArmLength = (PlayerPrefs.GetFloat("PlayerWidth",2f) / 2)+0.1f;
        _armLengthOnModel = armLengthOnModelProvider.ArmLength;
        _localScaleLower = lowerArm.localScale;
    }

    void LateUpdate()
    {
        float targetDistance = Mathf.Abs((target.position - lowerArm.position).magnitude);
        if (targetDistance > _maxPlayerArmLength)
        {
            targetDistance = _maxPlayerArmLength;
        }
        Vector3 newScaleLower = _localScaleLower;
        if (targetDistance > _armLengthOnModel)
        {
            float scale = (targetDistance / _armLengthOnModel);
            Vector3 scaleMask = Vector3.zero;
            Vector3 scaleValue = Vector3.zero;
            switch (localAxisToScale)
            {
                case Axis.x:
                {
                    scaleMask = new Vector3(0, 1, 1);
                    scaleValue = new Vector3(1, 0, 0);
                    break;
                }
                case Axis.y:
                {
                    scaleMask = new Vector3(1, 0, 1);
                    scaleValue = new Vector3(0, 1, 0);
                    break;
                }
                case Axis.z:
                {
                    scaleMask = new Vector3(1, 1, 0);
                    scaleValue = new Vector3(0, 0, 1);
                    break;
                }
            }

            newScaleLower = new Vector3(scaleMask.x * _localScaleLower.x + scaleValue.x * scale,
                scaleMask.y * _localScaleLower.y + scaleValue.y * scale,
                scaleMask.z * _localScaleLower.z + scaleValue.z * scale);
        }

        lowerArm.localScale = newScaleLower;
        hand.localScale = new Vector3(hand.localScale.x / newScaleLower.x, hand.localScale.y / newScaleLower.y, hand.localScale.z / newScaleLower.z);
    }
}