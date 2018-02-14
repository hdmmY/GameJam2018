using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class MovementProperty : BaseProperty
{
    public float m_cycleSpeed;

    public float m_cycleTimer;

    public Pose m_leftLegPose = Pose.Straight;

    public Pose m_rightLegPose = Pose.Straight;

    public Pose m_leftArmPose = Pose.Straight;

    public Pose m_rightArmPose = Pose.Straight;

    public float m_runForce;

    public static Vector3 RunVecForce10 = new Vector3 (0, 10, 0);

    public static Vector3 RunVecForce5 = new Vector3 (0, 5, 0);

    public static Vector3 RunVecForce2 = new Vector3 (0, 2, 0);
}

public enum Pose
{
    Bent,
    Forward,
    Straight,
    Behind
}

public enum Side
{
    Left,
    Right
}