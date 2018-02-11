using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[System.Serializable]
public class ForceApplier
{
    public Vector3 m_force;

    public Rigidbody m_rig;

    [VerticalGroup("1")]
    public ForceMode m_forceMode;

    public enum Axis
    {
        World,
        Local
    }

    [VerticalGroup("1")]
    public Axis m_forecAxis;


    public void ChangeForce(float x, float y, float z)
    {
        m_force.x = x;
        m_force.y = y;
        m_force.z = z;
    }
}

public class ApplyForce : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    [ListDrawerSettings(Expanded = false)]
    public List<ForceApplier> m_forceAppliers;

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
                Vector3 force = forceApplier.m_force;

                if (forceApplier.m_forecAxis == ForceApplier.Axis.Local)
                {
                    forceApplier.m_rig.AddRelativeForce(force * m_forceMutiplier, forceApplier.m_forceMode);
                }
                else
                {
                    forceApplier.m_rig.AddForce(force * m_forceMutiplier, forceApplier.m_forceMode);
                }
            }
        }
    }

    public void AddInverseImplusForce(float impluseMagnitude)
    {
        impluseMagnitude = Mathf.Abs(impluseMagnitude);

        foreach (var forceApplier in m_forceAppliers)
        {
            forceApplier.m_rig.AddForce(
                -forceApplier.m_rig.velocity * forceApplier.m_rig.mass * impluseMagnitude,
                ForceMode.Impulse);
        }                                         
    }
}
