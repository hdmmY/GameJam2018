using UnityEngine;

public class PlayerPullDownAnimation : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyRelativeForce m_liftHands;

    public float m_coolDown;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (m_input.Attack)
        {
            if (_timer > m_coolDown)
            {
                _timer = 0f;
                m_liftHands.m_enabled = true;
            }         
            else
            {
                m_liftHands.m_enabled = false;
            }
        }
        else
        {
            m_liftHands.m_enabled = false;
        }
    }
}
