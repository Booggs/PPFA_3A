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
        /* 0 -> 0 | 1
         * 1 -> 2 | 3
         * 2 -> 4 | 5
         * 3 -> 6 | 7
         * 4 -> 8 | 9
         * 5 -> 10 | 11
         */
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

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        Gizmos.color = _foundTarget ? transparentGreen : transparentRed;
        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(transform.position, _hackingRadius);
    }
}
