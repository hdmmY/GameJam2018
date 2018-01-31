using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_gravity;

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
        if(m_character.HasState(CharacterProperty.State.InAir) ||
           m_character.HasState(CharacterProperty.State.Wall))
        {
            m_gravity.m_enabled = true;
        }
        else
        {
            m_gravity.m_enabled = false;
        }
    }
}
