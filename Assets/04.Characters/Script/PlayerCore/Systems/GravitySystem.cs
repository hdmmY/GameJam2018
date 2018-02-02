using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_gravity;

    private void Update()
    {
        if ((m_character.HasState(CharacterProperty.State.InAir) && m_character.InAirTime > 0.15f) ||
            (m_character.HasState(CharacterProperty.State.Wall) && m_character.WallTime > 0.15f))
        {
            m_gravity.m_enabled = true;
        }
        else
        {
            m_gravity.m_enabled = false;
        }    
    }

    private void OnDisable()
    {
        m_gravity.m_enabled = false;
    }
}
