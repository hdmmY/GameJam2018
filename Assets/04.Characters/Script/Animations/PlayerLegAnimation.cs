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

    [SerializeField]
    private float _initAbsTorqueMutiplier;

    private void Start()
    {
        _initAbsTorqueMutiplier = Mathf.Abs(m_legTorque.m_torqueMutiplier);
    }

    private void Update()
    {
        if (!m_input.HasMoveInput || m_character.HasState(CharacterProperty.State.BeingGrabbed))
        {
            m_legTorque.m_enabled = false;
        }
        else
        {
            m_legTorque.m_enabled = true;

            _animationTimer += Time.deltaTime;

            float ratio = Mathf.Sin(_animationTimer / m_animationSpeed * Mathf.PI * 0.5f);
            m_legTorque.m_torqueMutiplier = Mathf.Sign(m_legTorque.m_torqueMutiplier) * ratio * _initAbsTorqueMutiplier;

            if (_animationTimer >= m_animationSpeed)
            {
                m_legTorque.m_torqueMutiplier = -m_legTorque.m_torqueMutiplier;
                _animationTimer = 0f;
            }
        }
    }
}
