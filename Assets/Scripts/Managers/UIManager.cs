using Cinemachine;
using StarterAssets;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GSGD2.Utilities;

/// <summary>
/// Manager class that handle global functionnality around UI. It is a proxy to UI subsystem and can enable or disable them.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _mainCanvas = null;

    [SerializeField]
    private PlayerHUDMenu _playerHUD = null;

    [SerializeField] 
    private PlayerHUDMenu _tankHUD = null;

    [SerializeField]
    private PlayerHUDMenu _hackerHUD = null;

    [SerializeField]
    private PlayerHUDMenu _droneHUD = null;

    private RobotBaseController _playerController = null;

    public Canvas MainCanvas => _mainCanvas;
    public PlayerHUDMenu PlayerHUD => _playerHUD;

    private void Awake()
    {
        _playerController = LevelReferences.Instance.CurrentController;
        _hackerHUD.SetActive(false);
        _droneHUD.SetActive(false);
    }
    public void SetPlayerHud(ERobotType robotType)
    {
        ShowPlayerHUD(false);
        switch (robotType)
        {
            case ERobotType.Tank:
            {
                _playerHUD = _tankHUD;
                break;
            }

            case ERobotType.Hacker:
            {
                _playerHUD = _hackerHUD;
                break;
                }

            case ERobotType.Drone:
            {
                _playerHUD = _droneHUD;
                break;
            }

            default:
                break;
        }
        ShowPlayerHUD(true);
    }

    public void ShowPlayerHUD(bool isActive)
    {
        _playerHUD.SetActive(isActive);
    }
}
