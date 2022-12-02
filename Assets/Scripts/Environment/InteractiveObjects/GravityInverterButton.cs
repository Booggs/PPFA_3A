using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class GravityInverterButton : InteractiveObject
{
    [SerializeField]
    private InvertGravityZone _invertGravityZone;

    public override bool InteractionValid(ERobotType robotType)
    {
        return Interactive;
    }

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        base.Interact(robotType, robot, controller);
        _invertGravityZone.ToggleZone();
    }
}
