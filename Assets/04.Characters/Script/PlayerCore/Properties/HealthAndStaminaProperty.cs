using UnityEngine;

/// <summary>
/// Usless currently. -- hdm 2018.02.10
/// </summary>
public class HealthAndStaminaProperty : BaseProperty
{
    #region Public Variables

    public float Health
    {
        get { return _health; }
        set { _health = Mathf.Clamp (0, _maxHealth, value); }
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    public float HealthRegenerateSpeed
    {
        get { return _healthRegenerateSpeed; }
    }

    public float Stamina
    {
        get { return _stamina; }
        set { _stamina = Mathf.Clamp (0, _maxStamina, value); }
    }

    public float MaxStamina
    {
        get { return _maxStamina; }
    }

    public float StaminaRegenerateSpeed
    {
        get { return _staminaRegenerateSpeed; }
    }

    #endregion

    #region Private Variables

    [SerializeField]
    private float _health;

    [SerializeField]
    private float _maxHealth;

    [SerializeField]
    private float _healthRegenerateSpeed;

    [SerializeField]
    private float _stamina;

    [SerializeField]
    private float _maxStamina;

    [SerializeField]
    private float _staminaRegenerateSpeed;

    #endregion
}