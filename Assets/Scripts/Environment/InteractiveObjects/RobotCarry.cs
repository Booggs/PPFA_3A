using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class RobotCarry : InteractiveObject
{
    private bool _carried = false;
    private Quaternion _startingRotation = Quaternion.identity;

    public delegate void OnObjectCarry(bool carried);

    public event OnObjectCarry ObjectCarryEvent;

    private void Start()
    {
        _startingRotation = transform.rotation;
    }

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        base.Interact(robotType, robot, controller);
        _carried = !_carried;
        if (_carried)
        {
            transform.SetParent(controller.CinemachineCameraTarget.transform);
        }
        else transform.parent = null;
        if (ObjectCarryEvent != null) ObjectCarryEvent.Invoke(_carried);
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(_startingRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _startingRotation.eulerAngles.z);
        transform.rotation = rotation;
    }
}
