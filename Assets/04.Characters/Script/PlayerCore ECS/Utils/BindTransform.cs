using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class BindTransform : MonoBehaviour
{
    public Transform m_targetTransform;

    public Transform m_followerTransform;

    public bool m_bindPosition;

    public bool m_bindRotation;

    [ShowIf ("m_bindPosition")]
    public Vector3 m_positionOffset;

    [ShowIf ("m_bindRotation")]
    public Quaternion m_rotationOffset;

    public enum UpdateMode
    {
        Update,
        LateUpdate,
        FixedUpdate
    }

    public UpdateMode m_updateMode;

    private void Update ()
    {
        if ((m_updateMode & UpdateMode.Update) == UpdateMode.Update)
        {
            Bind ();
        }
    }

    private void LateUpdate ()
    {
        if ((m_updateMode & UpdateMode.LateUpdate) == UpdateMode.LateUpdate)
        {
            Bind ();
        }
    }

    private void FixedUpdate ()
    {
        if ((m_updateMode & UpdateMode.FixedUpdate) == UpdateMode.FixedUpdate)
        {
            Bind ();
        }
    }

    private void Bind ()
    {
        if (m_bindPosition)
        {
            m_followerTransform.position = m_targetTransform.position + m_positionOffset;
        }

        if (m_bindRotation)
        {
            m_followerTransform.rotation = m_targetTransform.rotation * m_rotationOffset;
        }
    }

    [Button ("Preset Rotation Offset", ButtonSizes.Medium)]
    private void PresetRotationOffsetInEditor ()
    {
        m_rotationOffset = m_followerTransform.rotation;
    }
}