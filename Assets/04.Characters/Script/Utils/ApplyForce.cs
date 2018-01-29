using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class ApplyForce : MonoBehaviour
{
    [Multiline]
    public string m_description;

    [System.Serializable]
    public struct ForceApplier
    {
        public Vector3 m_force;

        public Rigidbody m_rig;

        public ForceMode m_forceMode;
    }

    [ListDrawerSettings(Expanded = false)]
    public List<ForceApplier> m_forceAppliers;

    public int m_forceMutiplier = 1;

    private void FixedUpdate()
    {
        foreach (var forceApplier in m_forceAppliers)
        {
            if (forceApplier.m_rig != null)
            {
                forceApplier.m_rig.AddForce(
                    forceApplier.m_force * m_forceMutiplier, forceApplier.m_forceMode);
            }
        }
    }
}
