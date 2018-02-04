using UnityEngine;
using System.Collections;

public class JumpSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_jumpForceApplier;

    private void Update()
    {
        if (m_input.JumpWasPressed)
        {
            if (m_character.HasState(CharacterProperty.State.Ground) ||
                m_character.HasState(CharacterProperty.State.Wall) ||
               (m_character.HasState(CharacterProperty.State.InAir) && m_character.InAirTime < 0.1f))
            {
                if (!m_character.HasState(CharacterProperty.State.Jump))
                {
                    StartCoroutine(AddJumpImpulse());
                }
            }
        }
    }

    private IEnumerator AddJumpImpulse()
    {
        m_character.m_state = CharacterProperty.State.Jump;

        m_jumpForceApplier.m_enabled = true;

        Debug.Log("Jump" + Time.time);

        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForEndOfFrame();

            if (i == 2)
            {
                m_jumpForceApplier.m_enabled = false;
            }
        }

        m_character.m_state &= ~CharacterProperty.State.Jump;  
    }
}
