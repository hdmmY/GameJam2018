using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MoveSystemNeededProperty
{
    public StateProperty m_stateProperty;

    public InputProperty m_inputProperty;

    public MovementProperty m_moveProperty;

    public BodyProperty m_bodyProperty;

    public ControlProperty m_controlProperty;

    public bool Valid ()
    {
        return m_stateProperty && m_inputProperty && m_moveProperty && m_bodyProperty && m_controlProperty;
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
        var state = neededProperty.m_stateProperty;
        var control = neededProperty.m_controlProperty;
        var movement = neededProperty.m_moveProperty;
        var body = neededProperty.m_bodyProperty;

        if (!state.HasState (State.Run))
        {
            return;
        }

        if (control.m_run)
        {
            movement.m_cycleSpeed = 0.18f;
        }
        else
        {
            movement.m_cycleSpeed = 0.23f;
        }

        if (state.m_stateChanged)
        {
            if (Random.Range (0, 1) == 1)
            {
                movement.m_leftLegPose = Pose.Bent;
                movement.m_rightLegPose = Pose.Straight;
                movement.m_leftArmPose = Pose.Straight;
                movement.m_rightArmPose = Pose.Bent;
            }
            else
            {
                movement.m_leftLegPose = Pose.Straight;
                movement.m_rightLegPose = Pose.Bent;
                movement.m_leftArmPose = Pose.Bent;
                movement.m_rightArmPose = Pose.Straight;
            }
            state.m_stateChanged = false;
        }

        RunCycleUpdate (movement);
        RunCycleRotateAnchor (control, body);
        RunCycleMainBody(control, movement, body);
    }

    private void RunCycleUpdate (MovementProperty movement)
    {
        if (movement.m_cycleTimer < movement.m_cycleSpeed)
        {
            movement.m_cycleTimer += Time.fixedDeltaTime;
        }
        else
        {
            movement.m_cycleTimer = 0f;

            int pose = (int) movement.m_leftLegPose;
            movement.m_leftLegPose = (Pose) (++pose <= 3 ? pose : 0);

            pose = (int) movement.m_rightLegPose;
            movement.m_rightLegPose = (Pose) (++pose <= 3 ? pose : 0);

            pose = (int) movement.m_leftArmPose;
            movement.m_leftArmPose = (Pose) (++pose <= 3 ? pose : 0);

            pose = (int) movement.m_rightArmPose;
            movement.m_rightArmPose = (Pose) (++pose <= 3 ? pose : 0);
        }
    }

    private void RunCycleRotateAnchor (ControlProperty control, BodyProperty body)
    {
        body[BodyPart.Anchor].BodyRigid.angularVelocity = Vector3.zero;

        Vector3 dir = new Vector3 (control.m_direction.z, 0, -control.m_direction.x);

        if (!control.m_run)
        {
            if (body[BodyPart.Anchor].BodyRigid.velocity.magnitude < 3 * control.m_applyForce)
            {
                body[BodyPart.Anchor].BodyRigid.maxAngularVelocity = 30f;
                body[BodyPart.Anchor].BodyRigid.angularVelocity = dir * 30f;
            }
        }
        else
        {
            if (body[BodyPart.Anchor].BodyRigid.velocity.magnitude < 4 * control.m_applyForce)
            {
                body[BodyPart.Anchor].BodyRigid.maxAngularVelocity = 60f;
                body[BodyPart.Anchor].BodyRigid.angularVelocity = dir * 60f;
            }
        }
    }

    private void RunCycleMainBody (ControlProperty control, MovementProperty movement, BodyProperty body)
    {
        if (control.m_run)
        {
            movement.m_runForce = Mathf.Clamp01 (movement.m_runForce += Time.fixedDeltaTime);
        }
        else
        {
            movement.m_runForce = Mathf.Clamp01 (movement.m_runForce -= Time.fixedDeltaTime);
        }

        body[BodyPart.Torso].BodyRigid.AddForce (
            (MovementProperty.RunVecForce10 + control.m_direction * movement.m_runForce) * control.m_applyForce,
            ForceMode.VelocityChange);
        body[BodyPart.Hip].BodyRigid.AddForce (-(MovementProperty.RunVecForce5 + (control.m_direction * movement.m_runForce) * control.m_applyForce),
            ForceMode.VelocityChange);
        body[BodyPart.Anchor].BodyRigid.AddForce (-MovementProperty.RunVecForce5 * control.m_applyForce, ForceMode.VelocityChange);

        ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.forward,
            control.m_lookDirection, 0.1f, 2.5f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.up,
            Vector3.up, 0.1f, 2.5f * control.m_applyForce);

        ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.forward,
            control.m_direction / 4 + Vector3.down, 0.1f, 4f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.up,
            Vector3.up, 0.1f, 4f * control.m_applyForce);

        ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.forward,
            control.m_direction, 0.1f, 4f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.up,
            Vector3.up, 0.1f, 4f * control.m_applyForce);

    }
}