using UnityEngine;

[ExecuteInEditMode]
public class LockPosition : MonoBehaviour
{
    public Transform m_target;

    private void Update()
    {
        m_target.position = transform.position;
    }

    private void FixedUpdate()
    {
        m_target.position = transform.position;
    }
}
