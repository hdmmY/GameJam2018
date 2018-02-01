using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public Transform m_target;

    public Transform m_origin;

    private Quaternion _baseRotation;

    private void Start()
    {
        _baseRotation = m_target.rotation;
    }

    private void Update()
    {
        if (m_target != null)
        {
            m_target.rotation = m_origin.rotation * _baseRotation;
        }
    }

    private void FixedUpdate()
    {
        if (m_target != null)
        {
            m_target.rotation = m_origin.rotation * _baseRotation;
        }
    }

}
