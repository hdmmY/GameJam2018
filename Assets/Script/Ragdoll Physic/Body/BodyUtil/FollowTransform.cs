using UnityEngine;
using Sirenix.OdinInspector;


[ExecuteInEditMode]
public class FollowTransform : MonoBehaviour
{
    public Transform m_target;

    public bool m_autoSetOffset;

    [DisableIf("m_autoSetOffset")]
    public Vector3 m_offset;

    public bool m_local;

    private void Start()
    {
        if (m_autoSetOffset)
        {
            if (m_local)
            {
                m_offset = transform.localPosition - m_target.position;
            }
            else
            {
                m_offset = transform.position - m_target.position;
            }
        }
    }


    private void LateUpdate()
    {
        if (m_local)
        {
            transform.localPosition =  m_target.position + m_offset;
        }
        else
        {
            transform.position = m_target.position + m_offset;
        }
    }


}
