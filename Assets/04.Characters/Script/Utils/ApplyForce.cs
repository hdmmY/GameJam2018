using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[System.Serializable]
public struct ForceApplier
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
}

public class ApplyForce : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    [ListDrawerSettings(Expanded = false)]
    public List<ForceApplier> m_forceAppliers;

    public float m_forceMutiplier = 1;

    private void FixedUpdate()
    {
        foreach (var forceApplier in m_forceAppliers)
        {
            if (forceApplier.m_rig != null)
            {
                Vector3 force = forceApplier.m_force;

                if (forceApplier.m_forecAxis == ForceApplier.Axis.Local)
                {
                    force = forceApplier.m_rig.transform.TransformVector(force);
                }

                forceApplier.m_rig.AddForce(force * m_forceMutiplier, forceApplier.m_forceMode);
            }
        }
    }
}
