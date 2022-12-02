using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class HackableObject : InteractiveObject
{
    [SerializeField] 
    private HackTarget[] _targets = Array.Empty<HackTarget>();

    private bool _hacked = false;
    private bool _remoteHacked = false;

    public bool Hacked => _hacked;

    public override void SetInteractionReady(bool ready)
    {
        base.SetInteractionReady(ready);
        if (InteractReady == false && _remoteHacked == false)
        {
            foreach (var HackTarget in _targets)
            {
                HackTarget.ModifyCurrentHacks(-1);
            }
            _hacked = false;
        }
    }

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        if (_remoteHacked) return;
        base.Interact(robotType, robot, controller);
        _hacked = !_hacked;
        foreach (var HackTarget in _targets)
        {
            if (HackTarget != null) HackTarget.ModifyCurrentHacks(_hacked ? 1 : -1);
        }
    }

    public void SetRemoteHacked(bool remoteHacked)
    {
        _remoteHacked = remoteHacked;
    }
}
