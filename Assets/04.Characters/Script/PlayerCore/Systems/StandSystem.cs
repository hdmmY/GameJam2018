using System.Collections.Generic;
using UnityEngine;

public struct StandSystemNeededProperty
{
    public BodyProperty m_bodyProperty;

    public StateProperty m_stateProperty;

    public InputProperty m_inputProperty;

    public bool Valid ()
    {
        return m_bodyProperty && m_stateProperty && m_inputProperty;
    }
}

public class StandSystem : MonoBehaviour
{
    public List<StandSystemNeededProperty> m_entities;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate ()
    {
        foreach (var entity in m_entities)
        {
            if (entity.Valid ())
            {
                Stand (entity);
            }
        }
    }

    private void Stand (StandSystemNeededProperty neededProperty)
    {
        var body = neededProperty.m_bodyProperty;
        var input = neededProperty.m_inputProperty;
        var state = neededProperty.m_stateProperty.m_state;

        if (state.HasState (State.Dead) || state.HasState (State.Stun))
        {
            return;
        }

        if (!state.HasState (State.Stand) && !state.HasState (State.Run))
        {
            return;
        }

        
    }

}