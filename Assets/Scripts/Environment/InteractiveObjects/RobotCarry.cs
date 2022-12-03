using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class RobotCarry : InteractiveObject
{
    private bool _carried = false;

    public delegate void OnObjectCarry(bool carried);

    public event OnObjectCarry ObjectCarryEvent;

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        base.Interact(robotType, robot, controller);
        _carried = !_carried;
        if (_carried)
        {
            transform.SetParent(robot.transform);
        }
        else transform.parent = null;
        ObjectCarryEvent.Invoke(_carried);
    }
}
