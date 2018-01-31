using UnityEngine;

public class StandSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_standForce;

    private float _inAirTime;

    private float _standingTime;

    private float _initStandForceMutiplier;

    private void Start()
    {
        _initStandForceMutiplier = m_standForce.m_forceMutiplier;
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

        if(m_standForce.m_enabled)
        {
            _standingTime += Time.deltaTime;
        }
        else
        {
            _standingTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (m_character.HasState(CharacterProperty.State.Ground) ||
            m_character.HasState(CharacterProperty.State.HoldSomething) ||
            m_character.HasState(CharacterProperty.State.Wall) || 
            (m_character.HasState(CharacterProperty.State.InAir) && _inAirTime < 0.3f))
        {
            // Gradully apply stand force
            float factor = _standingTime / 2f;
            factor = Mathf.Clamp01(factor);
            factor = Mathf.Sin(factor * Mathf.PI / 2);

            m_standForce.m_forceMutiplier = _initStandForceMutiplier * factor;
            m_standForce.m_enabled = true;
        }
        else
        {
            m_standForce.m_enabled = false;
        }             
    }
}
