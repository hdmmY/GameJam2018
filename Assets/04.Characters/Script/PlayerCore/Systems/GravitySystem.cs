using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public ApplyForce m_gravity;     

    private void Update()
    {
        m_gravity.m_enabled = true;    
    }
}
