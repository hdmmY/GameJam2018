using UnityEngine;
using System.Collections;

public class JumpSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_jumpForceApplier;

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
        if (m_input.JumpWasPressed)
        {
            if (m_character.HasState(CharacterProperty.State.Ground) ||
                m_character.HasState(CharacterProperty.State.HoldSomething) ||
                m_character.HasState(CharacterProperty.State.Wall) ||
                (m_character.HasState(CharacterProperty.State.InAir) && _inAirTime < 0.09f))
            {
                StartCoroutine(AddJumpImpulseForOneFrame());
            }
        }
    }

    private IEnumerator AddJumpImpulseForOneFrame()
    {
        m_jumpForceApplier.m_enabled = true;

        yield return new WaitForFixedUpdate();

        yield return new WaitForFixedUpdate();

        m_jumpForceApplier.m_enabled = false;

        Debug.Log("Jump!");
    }
}
