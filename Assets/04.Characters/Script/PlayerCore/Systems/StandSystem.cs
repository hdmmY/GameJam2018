using UnityEngine;

public class StandSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_standUpForce;

    public ApplyForce m_standDownForce;

    public ApplyForce m_footStickForce;

    private void Update()
    {   
        if(m_character.HasState(CharacterProperty.State.Jump))
        {
            m_footStickForce.m_enabled = false;
            m_standUpForce.m_enabled = false;
            m_standDownForce.m_enabled = false;

            return;
        }


        if (m_character.HasState(CharacterProperty.State.Ground) ||
           (m_character.HasState(CharacterProperty.State.InAir) && m_character.InAirTime < 0.15f))
        {
            m_footStickForce.m_enabled = !m_input.HasMoveInput;
            m_standUpForce.m_enabled = true;
            m_standDownForce.m_enabled = true;

        }
        else
        {
            m_footStickForce.m_enabled = false;
            m_standUpForce.m_enabled = false;
            m_standDownForce.m_enabled = false;
        }
    }

    private void OnDisable()
    {
        m_footStickForce.m_enabled = false;
        m_standUpForce.m_enabled = false;
        m_standDownForce.m_enabled = false;
    }
}
