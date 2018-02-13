using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MoveSystemNeededProperty
{
    public StateProperty m_stateProperty;

    public InputProperty m_inputProperty;

    public MovementProperty m_moveProperty;

    public BodyProperty m_bodyProperty;

    public bool Valid ()
    {
        return m_stateProperty && m_inputProperty && m_moveProperty && m_bodyProperty;
    }
}

public class MoveSystem : MonoBehaviour
{
    public List<MoveSystemNeededProperty> m_entities;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate ()
    {
        foreach (var entity in m_entities)
        {
            if (entity.Valid ())
            {
                Move (entity);
            }
        }
    }

    private void Move (MoveSystemNeededProperty neededProperty)
    {

    }
}