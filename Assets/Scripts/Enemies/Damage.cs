using GSGD2.Utilities;
using UnityEngine;

/// <summary>
/// Interface that can be implemented by user of Damage to tell the receiver who has given its damage. <see cref="Damage"/>, <seealso cref="Damageable"/>, <seealso cref="DamageDealer"/>.
/// </summary>
public interface IDamageInstigator
{
    public Transform GetTransform();
}

/// <summary>
/// Encapsulation of all informations related to a Damage done to a Damageable. It contains raw damage value, the bump informations and the instigator (source) of the Damage.
/// It can be setup by the inspector or created with a new Damage() and settings values by the constructor.
/// </summary>
[System.Serializable]
public class Damage
{
    [SerializeField]
    private int _damageValue = 1;

    [SerializeField]
    private bool _triggerInvincibility = true;

    private IDamageInstigator _instigator = null;

    public int DamageValue
    {
        get
        {
            return _damageValue;
        }
        set
        {
            _damageValue = value;
        }
    }

    public IDamageInstigator Instigator => _instigator;
    public bool TriggerInvincibility => _triggerInvincibility;

    public Damage() { }

    public Damage(int damageValue, IDamageInstigator instigator)
    {
        _damageValue = damageValue;
        _instigator = instigator;
    }

    public override string ToString()
    {
        var instigatorName = _instigator != null ? _instigator.GetTransform().name : string.Empty;
        return $"[Damage : {_damageValue} from {instigatorName}";
    }

    public void SetInstigator(IDamageInstigator damageInstigator)
    {
        _instigator = damageInstigator;
    }
}
