using UnityEngine;
using Sirenix.OdinInspector;


public class ItemProperty : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    public Rigidbody m_rigidBody
    {
        get
        {
            return GetComponent<Rigidbody>();
        }
    }

    public Vector3 m_grabOffset;

    [SerializeField, HideInInspector]
    private Quaternion _presetRotation;
    [ShowInInspector]
    public Quaternion m_presetRotation
    {
        get
        {
            return _presetRotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    public Vector3 m_pushDirection;

    public float m_pushForce;

    [ReadOnly]
    public Transform m_followTarget;

    private void Update()
    {
        if(m_followTarget != null)
        {
            transform.localPosition = m_grabOffset;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + m_pushDirection.normalized * 5f);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - m_grabOffset, Vector3.one * 0.15f);
    }
}
