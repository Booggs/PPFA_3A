using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu that group all the HUD subsystem (layout reminder, healthbar, etc...) and can show or hide all at once, or be just a proxy to pass subsystem to other object
/// </summary>
public class PlayerHUDMenu : AMenu
{
    [SerializeField] private HealthBarHUDMenu _healthBarHUDMenu = null;
    [SerializeField] private Text _interactPrompt = null;

    public HealthBarHUDMenu HealthBarHUDMenu => _healthBarHUDMenu;
    public Text InteractPrompt => _interactPrompt;
}
