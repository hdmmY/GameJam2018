using UnityEngine;

public class testRotation : MonoBehaviour
{
    public Vector2 m_lookDirection;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        _rb.MoveRotation(Quaternion.LookRotation(new Vector3(m_lookDirection.x, 0, m_lookDirection.y), Vector3.up));
    }
}
