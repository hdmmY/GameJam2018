using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;


[System.Serializable]
public class TorqueApplier
{
    public Vector3 m_torque;

    public Rigidbody m_rig;

    [VerticalGroup("1")]
    public ForceMode m_torqueMode;

    public enum Axis
    {
        World,
        Local
    }

    [VerticalGroup("1")]
    public Axis m_torqueAxis;
}

public class ApplyTorque : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    [ListDrawerSettings(Expanded = false)]
    public List<TorqueApplier> m_torqueAppliers;

    public float m_torqueMutiplier = 1;

    [ReadOnly]
    public bool m_enabled;

    private void FixedUpdate()
    {
        if (m_enabled)
        {
            foreach (var torqueApplier in m_torqueAppliers)
            {
                Vector3 torque = torqueApplier.m_torque;

                if (torqueApplier.m_torqueAxis == TorqueApplier.Axis.Local)
                {
                    torque = torqueApplier.m_rig.transform.TransformVector(torque);
                }

                torqueApplier.m_rig.AddTorque(torque * m_torqueMutiplier, torqueApplier.m_torqueMode);
            }
        }
    }

    public void FreezeRotation()
    {
        StartCoroutine(FreezeRotationForOneFrame());
    }

    private IEnumerator FreezeRotationForOneFrame()
    {
        foreach (var torquApplier in m_torqueAppliers)
        {
            torquApplier.m_rig.freezeRotation = true;
        }

        yield return null;

        foreach (var torquApplier in m_torqueAppliers)
        {
            torquApplier.m_rig.freezeRotation = false;
        }

        Debug.Log("Impluse Torque happen!");
    }
}
