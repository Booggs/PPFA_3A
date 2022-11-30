using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DestroyableObject : InteractiveObject
{
    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        base.Interact(robotType, robot, controller);
        Destroy(gameObject);
    }
}
