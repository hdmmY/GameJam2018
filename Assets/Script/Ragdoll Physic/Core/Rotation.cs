using UnityEngine;
using Sirenix.OdinInspector;


public class Rotation : MonoBehaviour
{
    public float m_rotationForce;

    [ReadOnly]
    public bool m_lookingRight;

    private CharacterInformation _characterInfo;

    private Rigidbody _hip;

    private void Start()
    {
        _characterInfo = GetComponent<CharacterInformation>();
        _hip = GetComponentInChildren<Hip>().GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Vector3 target;

        if (m_lookingRight)
        {
            target = _hip.position + Vector3.right * 10f;
        }
        else
        {
            target = _hip.position - Vector3.right * 10f;
        }
        target = _hip.transform.InverseTransformPoint(target);

        _hip.AddTorque(Vector3.up * Time.fixedDeltaTime * m_rotationForce * (-target.x), ForceMode.Acceleration);
    }

}
