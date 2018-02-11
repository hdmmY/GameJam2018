using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class StateUpdateSystem : MonoBehaviour
{
    public List<StateProperty> m_stateProperties;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update ()
    {
        foreach (var stateProperty in m_stateProperties)
        {
            UpdateProperty (stateProperty);
        }
    }

    private void UpdateProperty (StateProperty stateProperty)
    {
        State state = stateProperty.m_state;

        if (state.HasState (State.Stand))
        {
            stateProperty.StandTime += Time.deltaTime;
        }
        else
        {
            stateProperty.StandTime = 0f;
        }

        if (state.HasState (State.Run))
        {
            stateProperty.RunTime += Time.deltaTime;
        }
        else
        {
            stateProperty.RunTime = 0f;
        }

        if (state.HasState (State.Stun))
        {
            stateProperty.StunTime += Time.deltaTime;
        }
        else
        {
            stateProperty.StunTime = 0f;
        }

        if (state.HasState (State.Jump))
        {
            stateProperty.JumpTime += Time.deltaTime;
        }
        else
        {
            stateProperty.JumpTime = 0f;
        }

        if (state.HasState (State.Fall))
        {
            stateProperty.FallTime += Time.deltaTime;
        }
        else
        {
            stateProperty.FallTime = 0f;
        }

        if (state.HasState(State.Dead))
        {
            stateProperty.DeadTime += Time.deltaTime;
        }
        else
        {
            stateProperty.DeadTime = 0f;
        }
    }
}