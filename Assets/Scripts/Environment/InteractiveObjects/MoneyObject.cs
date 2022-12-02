using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class MoneyObject : InteractiveObject
{
    public int MoneyAmount = 100;

    public override bool InteractionValid(ERobotType robotType)
    {
        return Interactive;
    }

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        LevelReferences.Instance.MoneyManager.AddMoney(MoneyAmount);
        Destroy(gameObject);
    }
}
