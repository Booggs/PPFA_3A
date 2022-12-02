using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class HackerController : RobotBaseController
{
    [Header("Hacker")]
    [SerializeField]
    private HelperDeployer _helperDeployer = null;

    protected override void SetControllerPossessed(bool possessed)
    {
        base.SetControllerPossessed(possessed);
        _helperDeployer.enabled = possessed;
    }
}