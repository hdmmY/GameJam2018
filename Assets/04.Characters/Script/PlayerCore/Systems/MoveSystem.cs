using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_movementForce;

    [Range(0, 1), SerializeField]
    private float _forceDampWhenHoldSomething = 0.5f;

    private float _initForceMutiplier;

    private float _inAirTime;

    private void Start()
    {
        _initForceMutiplier = m_movementForce.m_forceMutiplier;
    }

    private void Update()
    {
        if (m_character.HasState(CharacterProperty.State.InAir))
        {
            _inAirTime += Time.deltaTime;
        }
        else
        {
            _inAirTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (m_character.HasState(CharacterProperty.State.BeingGrabbed))
        {
            m_movementForce.m_enabled = false;
            return;
        }

        m_movementForce.m_enabled = true;
        ApplyMoveForce();

        if (m_character.HasState(CharacterProperty.State.HoldSomething) ||
            (m_character.HasState(CharacterProperty.State.InAir) && _inAirTime > 0.3f) ||
            m_character.HasState(CharacterProperty.State.Wall))
        {
            m_movementForce.m_forceMutiplier = _initForceMutiplier * _forceDampWhenHoldSomething;
        }
        else
        {
            m_movementForce.m_forceMutiplier = _initForceMutiplier;
        }
    }

    private void ApplyMoveForce()
    {
        Vector2 inputVector = m_input.Move;

        foreach(var forceApplier in m_movementForce.m_forceAppliers)
        {
            forceApplier.ChangeForce(inputVector.x, 0, inputVector.y);
        }
    }
}
