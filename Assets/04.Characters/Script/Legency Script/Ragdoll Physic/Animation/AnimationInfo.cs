using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public struct AnimationInfo
{
    public enum AnimationType
    {
        Torque,
        Force
    };

    public enum Direction
    {
        CharacterRight,
        CharacterForward,
        CharacterUp,
        SelfRight,
        SelfForward,
        SelfUp,
        WorldRight,
        WorldForward,
        WorldUp,
        MainBodyVelocity
    };

    public AnimationType m_type;

    public Direction m_direction;

    public float m_forwardForceMultiplier;

    public float m_backForceMutiplier;

    public float m_smoothing;

    public bool m_isLeftSide;

    [BoxGroup("Pace Mutipliers")]
    public float m_walkMutiplier;

    [BoxGroup("Pace Mutipliers")]
    public float m_runMutiplier;

    [BoxGroup("Pace Mutipliers")]
    public float m_backMutiplier;

    [BoxGroup("Pace Mutipliers")]
    public float m_sideMutiplier;
}
