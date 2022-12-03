using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class HackerController : RobotBaseController
{
    [Header("Hacker")]
    [SerializeField]
    private HelperDeployer _helperDeployer = null;

    private bool _hackerCarried = false;

    protected void Start()
    {
        base.Start();
        RobotCarry carryComponent = GetComponent<RobotCarry>();
        carryComponent.ObjectCarryEvent -= HackerCarried;
        carryComponent.ObjectCarryEvent += HackerCarried;
    }

    public override void SetControllerPossessed(bool possessed)
    {
        base.SetControllerPossessed(possessed);
        _helperDeployer.enabled = _controllerPossessed;
    }

    private void HackerCarried(bool carried)
    {
        _hackerCarried = carried;
    }

    protected override void JumpAndGravity()
    {
        if (_hackerCarried) return;
        base.JumpAndGravity();
    }
}