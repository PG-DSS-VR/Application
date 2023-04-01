using UnityEngine;

public class CalculateArmLength : MonoBehaviour
{
    public Transform lowerArm;
    public Transform hand;

    private float _armLength;

    public float ArmLength => _armLength;

    // Start is called before the first frame update
    void Awake()
    {
        _armLength = Mathf.Abs((lowerArm.position - hand.position).magnitude);
    }
}