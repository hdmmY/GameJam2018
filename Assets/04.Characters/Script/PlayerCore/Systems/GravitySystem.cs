using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct GravitySystemNeededProperty
{
    public StateProperty m_stateProperty;

    public BodyProperty m_bodyProperty;

    public GravityProperty m_gravityProperty;

    public bool Valid ()
    {
        return m_stateProperty && m_bodyProperty && m_gravityProperty;
    }
}

public class GravitySystem : MonoBehaviour
{
    public List<GravitySystemNeededProperty> m_entities;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate ()
    {
        foreach (var entity in m_entities)
        {
            if (entity.Valid ())
            {
                ApplyGravity (entity);
            }
        }
    }

    private void ApplyGravity (GravitySystemNeededProperty neededProperty)
    {
        var state = neededProperty.m_stateProperty;
        var bodies = neededProperty.m_bodyProperty;
        var gravity = neededProperty.m_gravityProperty.m_gravity;

        var curState = state.m_state;

        if (curState.All ())
        {
            foreach (var body in bodies.m_bodyInfo.Values)
            {
                if (body.BodyRigid != null)
                {
                    body.BodyRigid.AddForce (gravity, ForceMode.Acceleration);
                }
            }
        }
    }
}