using UnityEngine;

public class ControlProperty : BaseProperty
{
    /// <summary>
    /// Current move direction
    /// </summary>
    public Vector3 m_direction;

    /// <summary>
    /// Raw move direction
    /// </summary>
    public Vector3 m_rawDirection;

    public Vector3 velocity;

    /// <summary>
    /// Body move direction
    /// </summary>
    public Vector3 m_moveDirection;

    /// <summary>
    /// Head look direction
    /// </summary>
    public Vector3 m_lookDirection;

    /// <summary>
    /// Character do nothing, and not being hurt or grabbed
    /// </summary>
    public bool m_idle;

    /// <summary>
    /// Timer for idle
    /// </summary>
    public float m_idleTimer;

    /// <summary>
    /// Whether the character is grounded
    /// </summary>
    public bool m_ground;

    public float m_groundCheckDelay = 0.1f;

    public float m_offGroundDelay = 0.2f;

    /// <summary>
    /// Base force that apply to body
    /// </summary>
    public float m_applyForce;

    public bool m_run;

    public float m_runTimer;

    public bool m_jump;

    /// <summary>
    /// Hold the jump button timer
    /// </summary>
    public float m_jumpTimer;

    
    public float m_jumpDelay = 0.8f;

    public float m_fallTimer;

}