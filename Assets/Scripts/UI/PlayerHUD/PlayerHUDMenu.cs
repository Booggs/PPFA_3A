using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu that group all the HUD subsystem (layout reminder, healthbar, etc...) and can show or hide all at once, or be just a proxy to pass subsystem to other object
/// </summary>
public class PlayerHUDMenu : AMenu
{
    [SerializeField] private Text _interactPrompt = null;
    [SerializeField] private Text _moneyDisplay = null;

    public Text InteractPrompt => _interactPrompt;

    private void Start()
    {
        LevelReferences.Instance.MoneyManager.AddMoneyEvent -= UpdateMoney;
        LevelReferences.Instance.MoneyManager.AddMoneyEvent += UpdateMoney;
        _moneyDisplay.text = 0.ToString();
    }

    public void UpdateMoney(int amount)
    {
        _moneyDisplay.text = amount.ToString();
    }
}
