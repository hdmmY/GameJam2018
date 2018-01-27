using UnityEngine;

public class testTorque : MonoBehaviour
{

    public Vector3 torque;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(torque, ForceMode.Force);
    }
}
