using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[System.Serializable]
public class RelativeForceApplier
{
    public Vector3 m_force;

    public Rigidbody m_rig;

    public ForceMode m_forceMode;

    public Transform m_coordinate;

    public void ChangeForce(float x, float y, float z)
    {
        m_force.x = x;
        m_force.y = y;
        m_force.z = z;
    }
}



public class ApplyRelativeForce : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    public List<RelativeForceApplier> m_forceAppliers;

    public float m_forceMutiplier = 1;

    [ReadOnly]
    public bool m_enabled = false;

    private void FixedUpdate()
    {
        if (!m_enabled) return;

        foreach (var forceApplier in m_forceAppliers)
        {
            if (forceApplier.m_rig != null)
            {
                if (forceApplier.m_coordinate != null)
                {
                    forceApplier.m_rig.AddForce(
                        forceApplier.m_coordinate.TransformVector(forceApplier.m_force) * m_forceMutiplier,
                        forceApplier.m_forceMode);

                }
                else
                {
                    forceApplier.m_rig.AddForce(
                        forceApplier.m_force * m_forceMutiplier,
                        forceApplier.m_forceMode);
                }
            }
        }
    }
}
