using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperDeployer : MonoBehaviour
{
    [HideInInspector]
    public bool HelperDeployed = false;

    [SerializeField] 
    private GameObject _cameraRoot = null;

    [SerializeField] 
    private GameObject _hackerHelperPrefab = null;

    private StarterAssetsInputs _input;
    private RobotBaseController _controller;
    private HackerHelper _hackerHelper = null;

    private void Awake()
    {
        _input = LevelReferences.Instance.Input;
        _controller = GetComponent<RobotBaseController>();
    }

    private void OnEnable()
    {
        _input.SpecialInteract -= DeployHelper;
        _input.SpecialInteract += DeployHelper;
    }

    private void OnDisable()
    {
        _input.SpecialInteract -= DeployHelper;
    }

    private void DeployHelper()
    {
        if (_hackerHelper != null)
        {
            _hackerHelper.Interact(ERobotType.Hacker, gameObject, _controller);
            _hackerHelper = null;
            HelperDeployed = false;
        }
        else if (Physics.Linecast(_cameraRoot.transform.position, 
                     _cameraRoot.transform.position + _cameraRoot.transform.forward * 3.0f, out var hitInfo))
        {
            GameObject hackerHelperGameObject;
            hackerHelperGameObject = Instantiate<GameObject>(_hackerHelperPrefab, hitInfo.point, Quaternion.identity);

            _hackerHelper = hackerHelperGameObject.GetComponent<HackerHelper>();
            HelperDeployed = true;
        }
    }
}