using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_gravity;

    private float _inAirTime;

    private float _wallTime;

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

        if(m_character.HasState(CharacterProperty.State.Wall))
        {
            _wallTime += Time.deltaTime;
        }
        else
        {
            _wallTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if((m_character.HasState(CharacterProperty.State.InAir) && _inAirTime > 0.2f) ||
           (m_character.HasState(CharacterProperty.State.Wall) && _wallTime > 0.2f))
        {
            m_gravity.m_enabled = true;
        }
        else
        {
            m_gravity.m_enabled = false;
        }
    }
}
