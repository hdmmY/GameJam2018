using UnityEngine;

public class StandSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_standForce;

    private float _inAirTime;   

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
        if (m_character.HasState(CharacterProperty.State.Ground) ||
            m_character.HasState(CharacterProperty.State.HoldSomething) ||
            m_character.HasState(CharacterProperty.State.Wall) || 
            (m_character.HasState(CharacterProperty.State.InAir) && _inAirTime < 0.3f))
        {   
            m_standForce.m_enabled = true;
        }
        else
        {
            m_standForce.m_enabled = false;
        }             
    }
}
