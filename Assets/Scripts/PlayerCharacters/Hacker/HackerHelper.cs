using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerHelper : InteractiveObject
{
    [SerializeField] 
    private float _hackingRadius = 2.0f;

    [SerializeField]
    private LineRenderer _lineRenderer;

    private bool _foundTarget = false;
    private List<HackableObject> _targetHackableObjects = new List<HackableObject>();

    // Start is called before the first frame update
    void Start()
    {
        ActivateHelper();
    }

    private void ActivateHelper()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, _hackingRadius);

        foreach (var Collider in overlappedColliders)
        {
            HackableObject hackableObject;
            if ((hackableObject = Collider.gameObject.GetComponent<HackableObject>()) != null)
            {
                _targetHackableObjects.Add(hackableObject);
                _foundTarget = true;
            }
        }

        _lineRenderer.positionCount = _targetHackableObjects.Count * 2;

        for (int i = 0; i < _targetHackableObjects.Count; i++)
        {
            HackableObject hackableObject = _targetHackableObjects[i];
            

            if (hackableObject != null && hackableObject.Hacked == false)
            {
                hackableObject.Interact(ERobotType.Hacker, gameObject, LevelReferences.Instance.CurrentController);
                hackableObject.SetRemoteHacked(true);
                _lineRenderer.SetPosition(i + i, transform.position);
                _lineRenderer.SetPosition(i + i + 1, hackableObject.transform.position);
            }
        }
    }

    public void DeactivateHelper()
    {
        foreach (var hackableObject in _targetHackableObjects)
        {
            if (hackableObject != null && hackableObject.Hacked == true)
            {
                hackableObject.SetRemoteHacked(false);
                hackableObject.Interact(ERobotType.Hacker, gameObject, LevelReferences.Instance.CurrentController);
            }
        }
    }

    private void OnDestroy()
    {
        DeactivateHelper();
    }

    public override void Interact(ERobotType robotType, GameObject robot, RobotBaseController controller)
    {
        base.Interact(robotType, robot, controller);
        Destroy(gameObject);
    }
}
