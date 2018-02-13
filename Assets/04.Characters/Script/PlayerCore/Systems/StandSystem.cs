using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StandSystemNeededProperty
{
    public BodyProperty m_bodyProperty;

    public StateProperty m_stateProperty;

    public ControlProperty m_controlProperty;

    public bool Valid ()
    {
        return m_bodyProperty && m_stateProperty && m_controlProperty;
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
        var control = neededProperty.m_controlProperty;
        var state = neededProperty.m_stateProperty;

        if (!state.HasState (State.Stand))
        {
            return;
        }

        if (control.m_run)
        {
            ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.forward,
                control.m_lookDirection, 0.1f, 0.25f * control.m_applyForce, true);
            ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.up,
                Vector3.up, 0.1f, 2.5f * control.m_applyForce, true);

            ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.forward,
                control.m_direction + Vector3.down, 0.1f, 5f, true);
            body[BodyPart.Torso].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);

            ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.forward,
                control.m_direction, 0.1f, 5f, true);
            body[BodyPart.Hip].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);

            Debug.Log ("Here!");
        }
        else
        {
            ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.forward,
                control.m_lookDirection, 0.1f, 2.5f * control.m_applyForce, true);
            ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.up,
                Vector3.up, 0.1f, 2.5f * control.m_applyForce, true);

            ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.forward,
                control.m_direction, 0.1f, 4f * control.m_applyForce, true);
            ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.up,
                Vector3.up, 0.1f, 4f * control.m_applyForce, true);

            ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.forward,
                control.m_direction, 0.1f, 4f * control.m_applyForce, true);
            ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.up,
                Vector3.up, 0.1f, 3f * control.m_applyForce, true);

            body[BodyPart.Torso].BodyRigid.AddForce (new Vector3 (0, 4, 0) * control.m_applyForce, ForceMode.VelocityChange);
            body[BodyPart.Hip].BodyRigid.AddForce (new Vector3 (0, -3.5f, 0) * control.m_applyForce, ForceMode.VelocityChange);
        }

        body[BodyPart.Anchor].BodyRigid.angularVelocity = Vector3.zero;
    }
}