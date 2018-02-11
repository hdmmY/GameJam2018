using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Flags]
public enum State
{
    None = 0,
    Stand = 1,
    Run = 2,
    Jump = 4,
    Fall = 8,
    Stun = 16,
    Dead = 32,
}

public class StateProperty : BaseProperty
{
    public State m_state;

    [ShowInInspector, ReadOnly]
    public float StandTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }

    public float RunTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }

    [ShowInInspector, ReadOnly]
    public float StunTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }

    [ShowInInspector, ReadOnly]
    public float JumpTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }

    [ShowInInspector, ReadOnly]
    public float FallTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }

    [ShowInInspector, ReadOnly]
    public float DeadTime
    {
        get;
        /// <summary>
        /// Only can be set by StateUpdateSystem
        /// </summary>
        set;
    }
}