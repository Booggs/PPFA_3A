using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public ERobotType RobotNeeded = ERobotType.Drone;

    [SerializeField] 
    private string _interactPrompt = "What the object says when you aim at it with the right robot";

    private bool _interactive = true;


    public bool Interactive => _interactive;

    public string InteractPrompt => _interactPrompt;

    public virtual void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        if (robotType != RobotNeeded) return;
    }
}
