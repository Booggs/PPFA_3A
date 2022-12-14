using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Subsystem that handle the player health bar. It seek player <see cref="Damageable"/>, listen to its <see cref="Damageable.DamageTaken"/>  and <see cref="Damageable.HealthRestored"\> event and react accordingly.
/// </summary>
public class HealthBarHUDMenu : MonoBehaviour
{
    [SerializeField]
    private Image _healthbarForeground = null;

    [SerializeField]
    private Timer _fadingTimer;

    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    [SerializeField]
    private Image _currentOutline = null;

    [SerializeField]
    private Sprite[] _outlines = new Sprite[3];

    private Damageable _damageable = null;
    private bool _fadingOut = false;

    private void Awake()
    {
        _damageable = LevelReferences.Instance.CurrentController.GetComponent<Damageable>();
        //_canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        if (_damageable != null)
        {
            _damageable.DamageTaken -= Damageable_OnHealthChanged;
            _damageable.DamageTaken += Damageable_OnHealthChanged;
            _damageable.HealthRestored -= Damageable_OnHealthChanged;
            _damageable.HealthRestored += Damageable_OnHealthChanged;

        }
    }

    private void OnDisable()
    {
        if (_damageable != null)
        {
            _damageable.DamageTaken -= Damageable_OnHealthChanged;
            _damageable.HealthRestored -= Damageable_OnHealthChanged;
        }
    }

    private void Start()
    {
        if (_damageable != null)
        {
            UpdateHealth(_damageable.CurrentHealth, _damageable.MaxHealth);
        }
    }

    /*private void Update()
    {
        if (_fadingTimer.IsRunning)
            _fadingTimer.Update();

        if (_fadingOut)
        {
            _canvasGroup.alpha = 1 - _fadingTimer.Progress;
        }
        else _canvasGroup.alpha = _fadingTimer.Progress;
    }*/

    private void Damageable_OnHealthChanged(Damageable sender, Damageable.DamageableArgs args)
    {
        UpdateHealth(args.currentHealth, args.maxHealth);
    }

    private void UpdateHealth(float health, float maxHealth)
    {
        float perc = Mathf.Clamp01(health / maxHealth);
        if (perc >= 0.66f)
        {
            _currentOutline.sprite = _outlines[0];
        }
        else if (perc >= 0.33f && perc < 0.66f)
        {
            _currentOutline.sprite = _outlines[1];
        }
        else _currentOutline.sprite = _outlines[2];

        _healthbarForeground.fillAmount = perc;
    }
}
