using UnityEngine;
using Sirenix.OdinInspector;

public class testSignedAngle : MonoBehaviour
{
    [Range(0, 360)]
    public float testAngle;

    [ReadOnly]
    public float result;

    private Vector3 testVector;


    private void Update()
    {
        testVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * testAngle), 0, Mathf.Sin(Mathf.Deg2Rad * testAngle));

        result = Vector3.SignedAngle(Vector3.forward, testVector, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, Vector3.forward);
        Gizmos.DrawLine(Vector3.zero, testVector);
        Gizmos.DrawCube(Vector3.zero, Vector3.one * 0.1f);
        Gizmos.DrawCube(testVector, Vector3.one * 0.1f);
    }
}
