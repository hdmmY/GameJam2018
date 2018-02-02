using UnityEngine;

public class PlayerLegAnimation : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyTorque m_legTorque;

    [Range(0, 1)]
    public float m_animationSpeed;

    private float _animationTimer;

    private float _inAirTime;

    private void Update()
    {
        Vector2 inputVector = m_input.Move;

        if (inputVector == Vector2.zero)
        {
            m_legTorque.m_enabled = false;
        }
        else
        {
            m_legTorque.m_enabled = true;

            _animationTimer += Time.deltaTime;
            if (_animationTimer >= m_animationSpeed)
            {
                m_legTorque.m_torqueMutiplier = -m_legTorque.m_torqueMutiplier;
                _animationTimer = 0f;
            }
        }
    }
}
