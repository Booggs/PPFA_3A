using StarterAssets;

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSGD2.Utilities;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton class (needs to be added only once, but accessing from everywhere). It's purpose is to be only a proxy for other managers or unique component (player, camera related gameobjects, <see cref="PlayerStart"/>, etc) (similar to a "Service Locator" pattern).
/// </summary>
public class LevelReferences : Singleton<LevelReferences>
{

    [SerializeField]
    private RobotBaseController _player = null;

    [SerializeField]
    private Camera _mainCamera = null;

    [SerializeField]
    private UIManager _uiManager = null;


    public RobotBaseController Player => _player;
    public Camera Camera => _mainCamera;
    public UIManager UIManager => _uiManager;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // TODO AL : lazy, redo this properly
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    public void ChangeController(RobotBaseController controller)
    {
        _player = controller;
    }
}