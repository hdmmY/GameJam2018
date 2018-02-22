using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public bool m_beingGrabbed = false;

    public Vector3 m_grabPosOffset;

    public Quaternion m_grabRotation;

    public Vector3 m_grabTarget;

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube (transform.TransformPoint (m_grabPosOffset), Vector3.one * 0.15f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube (transform.TransformPoint (m_grabTarget), Vector3.one * 0.15f);
    }

}