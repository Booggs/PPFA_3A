using StarterAssets;

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSGD2.Utilities;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using System.Security.Cryptography;

/// <summary>
/// Singleton class (needs to be added only once, but accessing from everywhere). It's purpose is to be only a proxy for other managers or unique component (player, camera related gameobjects, <see cref="PlayerStart"/>, etc) (similar to a "Service Locator" pattern).
/// </summary>
public class LevelReferences : Singleton<LevelReferences>
{
    [SerializeField]
    private RobotBaseController _currentController = null;

    [SerializeField]
    private Camera _mainCamera = null;

    [SerializeField]
    private UIManager _uiManager = null;

    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private StarterAssetsInputs _input;

    [Tooltip("Plug the PlayerFollowCamera GameObject in there")]
    [SerializeField]
    private CinemachineVirtualCamera _mainVirtualCamera = null;

    [SerializeField]
    private RobotBaseController _tankController = null;

    [SerializeField]
    private HackerController _hackerController = null;

    [SerializeField]
    private DroneController _droneController = null;

    private ERobotType _currentRobotType = ERobotType.Tank;

    public RobotBaseController CurrentController => _currentController;
    public Camera Camera => _mainCamera;
    public UIManager UIManager => _uiManager;
    public PlayerInput PlayerInput => _playerInput;
    public StarterAssetsInputs Input => _input;
    public ERobotType CurrentRobotType => _currentRobotType;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // TODO AL : lazy, redo this properly
        if (Gamepad.current == null) return;
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    private new void Start()
    {
        base.Start();
        _mainVirtualCamera.Follow = _currentController.CinemachineCameraTarget.transform;
        _droneController.enabled = false;
        _hackerController.enabled = false;
    }

    public void ChangeController(ERobotType robotType)
    {
        if (_currentRobotType == robotType) return;

        _currentRobotType = robotType;
        _currentController.enabled = false;

        switch (_currentRobotType)
        {
            case ERobotType.Tank:
                _currentController = _tankController;
                break;

            case ERobotType.Hacker:
                _currentController = _hackerController;
                break;

            case ERobotType.Drone:
                _currentController = _droneController;
                break;

            default:
                break;
        }
        _uiManager.SetPlayerHud(_currentRobotType);
        _currentController.enabled = true;
        _mainVirtualCamera.Follow = _currentController.CinemachineCameraTarget.transform;
    }
}

public enum ERobotType { Tank, Hacker, Drone };