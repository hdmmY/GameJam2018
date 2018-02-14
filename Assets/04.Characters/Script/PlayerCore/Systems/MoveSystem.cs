using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MoveSystemNeededProperty
{
    public StateProperty m_stateProperty;

    public MovementProperty m_moveProperty;

    public BodyProperty m_bodyProperty;

    public ControlProperty m_controlProperty;

    public bool Valid ()
    {
        return m_stateProperty && m_moveProperty && m_bodyProperty && m_controlProperty;
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
                if (entity.m_stateProperty.HasState (State.Run)) Run (entity);
                if (entity.m_stateProperty.HasState (State.Jump)) Jump(entity);
            }
        }
    }

    private void Run (MoveSystemNeededProperty neededProperty)
    {
        var state = neededProperty.m_stateProperty;
        var control = neededProperty.m_controlProperty;
        var movement = neededProperty.m_moveProperty;
        var body = neededProperty.m_bodyProperty;

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
        RunCycleMainBody (control, movement, body);

        RunCyclePoseLeg (Side.Left, movement.m_leftLegPose, control, body);
        RunCyclePoseLeg (Side.Right, movement.m_rightLegPose, control, body);
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
                body[BodyPart.Anchor].BodyRigid.maxAngularVelocity = 20;
                body[BodyPart.Anchor].BodyRigid.angularVelocity = dir * 20f;
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
            movement.m_runForce = Mathf.Clamp (movement.m_runForce + Time.fixedDeltaTime, 0f, 1f);
        }
        else
        {
            movement.m_runForce = Mathf.Clamp (movement.m_runForce - Time.fixedDeltaTime, 0f, 1f);
        }

        body[BodyPart.Torso].BodyRigid.AddForce (
            (MovementProperty.RunVecForce10 + 0.3f * control.m_direction * movement.m_runForce) * control.m_applyForce,
            ForceMode.VelocityChange);
        body[BodyPart.Hip].BodyRigid.AddForce (
            (-MovementProperty.RunVecForce5 + 0.3f * control.m_direction * movement.m_runForce) * control.m_applyForce,
            ForceMode.VelocityChange);
        body[BodyPart.Anchor].BodyRigid.AddForce (-MovementProperty.RunVecForce5 * control.m_applyForce, ForceMode.VelocityChange);

        body[BodyPart.Anchor].BodyRigid.AddForce (
            control.m_direction * control.m_applyForce, ForceMode.VelocityChange);

        ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.forward,
            control.m_lookDirection, 0.1f, 5f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Head], body[BodyPart.Head].BodyTransform.up,
            Vector3.up, 0.1f, 5f * control.m_applyForce);

        ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.forward,
            control.m_direction + Vector3.down, 0.1f, 10f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.up,
            Vector3.up, 0.1f, 10f * control.m_applyForce);

        ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.forward,
            control.m_direction, 0.1f, 10f * control.m_applyForce);
        ApplyForceUtils.AlignToVector (body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.up,
            Vector3.up, 0.1f, 10f * control.m_applyForce);
    }

    private void RunCyclePoseLeg (Side side, Pose pose, ControlProperty control, BodyProperty body)
    {
        Transform hip = body[BodyPart.Hip].BodyTransform;

        Transform sideLeg = null;
        Transform unsideKnee = null;

        Rigidbody sideLegRig = null;
        Rigidbody sideKneeRig = null;
        Rigidbody unsideLegRig = null;

        switch (side)
        {
            case Side.Left:
                sideLeg = body[BodyPart.LeftLeg].BodyTransform;
                unsideKnee = body[BodyPart.RightKnee].BodyTransform;
                sideLegRig = body[BodyPart.LeftLeg].BodyRigid;
                unsideLegRig = body[BodyPart.RightLeg].BodyRigid;
                sideKneeRig = body[BodyPart.LeftKnee].BodyRigid;
                break;
            case Side.Right:
                sideLeg = body[BodyPart.RightLeg].BodyTransform;
                unsideKnee = body[BodyPart.LeftKnee].BodyTransform;
                sideLegRig = body[BodyPart.RightLeg].BodyRigid;
                unsideLegRig = body[BodyPart.LeftLeg].BodyRigid;
                sideKneeRig = body[BodyPart.RightKnee].BodyRigid;
                break;
        }

        switch (pose)
        {
            case Pose.Bent:
                ApplyForceUtils.AlignToVector (sideLegRig, -sideLeg.up, control.m_direction, 0.1f, 4f * control.m_applyForce);
                break;
            case Pose.Forward:
                if (control.m_run)
                {
                    ApplyForceUtils.AlignToVector (sideLegRig, -sideLeg.up, control.m_direction, 0.1f, 4f * control.m_applyForce);
                    sideLegRig.AddForce (-control.m_direction * 2f * control.m_applyForce);
                    sideKneeRig.AddForce (control.m_direction * 2f * control.m_applyForce);
                }
                else
                {
                    ApplyForceUtils.AlignToVector (sideLegRig, -sideLeg.up, control.m_direction - hip.up / 2, 0.1f, 4f * control.m_applyForce);
                    sideLegRig.AddForce (-control.m_direction * 2f, ForceMode.VelocityChange);
                    sideKneeRig.AddForce (control.m_direction * 2f, ForceMode.VelocityChange);
                }
                break;
            case Pose.Straight:
                ApplyForceUtils.AlignToVector (sideLegRig, sideLeg.up, Vector3.up, 0.1f, 4f * control.m_applyForce);
                sideLegRig.AddForce (hip.up * 2f * control.m_applyForce);
                sideKneeRig.AddForce (-hip.up * 2f * control.m_applyForce);
                break;
            case Pose.Behind:
                if (control.m_run)
                {
                    ApplyForceUtils.AlignToVector (sideLegRig, sideLeg.up, control.m_direction * 2f, 0.1f, 4f * control.m_applyForce);
                    body[BodyPart.Hip].BodyRigid.AddForce (MovementProperty.RunVecForce2 * control.m_applyForce, ForceMode.VelocityChange);
                    body[BodyPart.Anchor].BodyRigid.AddForce (-MovementProperty.RunVecForce2 * control.m_applyForce, ForceMode.VelocityChange);
                    sideKneeRig.AddForce (-body[BodyPart.Hip].BodyTransform.forward, ForceMode.VelocityChange);
                }
                else
                {
                    ApplyForceUtils.AlignToVector (sideLegRig, sideLeg.up, control.m_direction * 2f, 0.1f, 4f * control.m_applyForce);
                }
                break;
        }
    }

    private void Jump (MoveSystemNeededProperty entity)
    {
        var state = entity.m_stateProperty;
        var control = entity.m_controlProperty;
        var body = entity.m_bodyProperty;

        Debug.Log(Time.time.ToString() + state.m_stateChanged);
        
        if (state.m_stateChanged)
        {
            state.m_stateChanged = false;

            float runFactor = control.m_run ? 0.2f : 0.4f;

            if (control.m_jump)
            {
                Vector3 hipVelocity = body[BodyPart.Hip].BodyRigid.velocity;
                if (hipVelocity.y < 2f)
                {
                    body[BodyPart.Torso].BodyRigid.AddForce (control.m_direction * runFactor, ForceMode.VelocityChange);
                    body[BodyPart.Hip].BodyRigid.AddForce (control.m_direction * runFactor, ForceMode.VelocityChange);

                    float torsoY = body[BodyPart.Torso].BodyTransform.position.y;
                    float hipY = body[BodyPart.Hip].BodyTransform.position.y;

                    if (torsoY > hipY)
                    {
                        Vector3 jumpForce = Vector3.up * 30f;
                        body[BodyPart.Torso].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
                        body[BodyPart.Hip].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
                    }
                    else
                    {
                        Vector3 jumpForce = Vector3.up * 15f;
                        body[BodyPart.Torso].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
                        body[BodyPart.Hip].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
                    }
                }
            }
            control.m_jump = false;
        }

        if (InputUtils.ValidMove (new Vector2 (control.m_rawDirection.x, control.m_rawDirection.z)))
        {
            body[BodyPart.Torso].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);
            body[BodyPart.Hip].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);

            ApplyForceUtils.AlignToVector(
                body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.up,
                Vector3.up, 0.1f, 10f);
            ApplyForceUtils.AlignToVector (
                body[BodyPart.Torso], body[BodyPart.Torso].BodyTransform.forward,
                control.m_direction + Vector3.down, 0.1f, 10f);
            ApplyForceUtils.AlignToVector(
                body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.up,
                Vector3.up, 0.1f, 10f);
            ApplyForceUtils.AlignToVector (
                body[BodyPart.Hip], body[BodyPart.Hip].BodyTransform.forward,
                control.m_direction + Vector3.up, 0.1f, 10f);

            ApplyForceUtils.AlignToVector (
                body[BodyPart.LeftArm], body[BodyPart.LeftArm].BodyTransform.up,
                body[BodyPart.Torso].BodyTransform.right + body[BodyPart.Torso].BodyTransform.forward,
                0.1f, 4f);
            ApplyForceUtils.AlignToVector (
                body[BodyPart.LeftElbow], body[BodyPart.LeftArm].BodyTransform.up,
                body[BodyPart.Torso].BodyTransform.right - body[BodyPart.Torso].BodyTransform.forward,
                0.1f, 4f);

            ApplyForceUtils.AlignToVector (
                body[BodyPart.RightArm], body[BodyPart.RightArm].BodyTransform.up, 
                -body[BodyPart.Torso].BodyTransform.right + body[BodyPart.Torso].BodyTransform.forward,
                0.1f, 4f);
            ApplyForceUtils.AlignToVector (
                body[BodyPart.RightElbow], body[BodyPart.RightElbow].BodyTransform.up, 
                -body[BodyPart.Torso].BodyTransform.right - body[BodyPart.Torso].BodyTransform.forward,
                0.1f, 4f);

            ApplyForceUtils.AlignToVector(
                body[BodyPart.LeftLeg], body[BodyPart.LeftLeg].BodyTransform.up,
                body[BodyPart.Hip].BodyTransform.up - body[BodyPart.Hip].BodyTransform.forward,
                0.1f, 4f);
            ApplyForceUtils.AlignToVector(
                body[BodyPart.RightLeg], body[BodyPart.RightLeg].BodyTransform.up,
                body[BodyPart.Hip].BodyTransform.up - body[BodyPart.Hip].BodyTransform.forward,
                0.1f, 4f);
            
        }
    }
}