using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GSGD2;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float _interactRange = 2.0f;
    [SerializeField] private GameObject _cameraRoot = null;

    private InteractiveObject _interactiveObject;
    private StarterAssetsInputs _input;
    private RobotBaseController _controller;
    private ERobotType _robotType = ERobotType.Tank;
    private Text _interactPrompt = null;

    private void Awake()
    {
        _input = LevelReferences.Instance.Input;
        _controller = GetComponent<RobotBaseController>();
        _robotType = _controller.RobotType;
        _interactPrompt = LevelReferences.Instance.UIManager.PlayerHUD.InteractPrompt;
    }

    private void OnEnable()
    {
        _input.Interact -= Interact;
        _input.Interact += Interact;
    }

    private void OnDisable()
    {
        _input.Interact -= Interact;
    }

    private void Update()
    {
        if (Physics.Linecast(_cameraRoot.transform.position, _cameraRoot.transform.position + _cameraRoot.transform.forward * _interactRange, out var hitInfo))
        {
            _interactiveObject = hitInfo.transform.GetComponentInParent<InteractiveObject>();
            Debug.Log("Interactive object valid : " + _interactiveObject != null +  " /n" + "Robot type valid : " + (_controller.RobotType == _interactiveObject.RobotNeeded));
            if (_interactiveObject != null && _interactiveObject.Interactive && _controller.RobotType == _interactiveObject.RobotNeeded)
            {
                _interactPrompt.enabled = true;
                _interactPrompt.text = _interactiveObject.InteractPrompt;
            }
            else _interactPrompt.enabled = false;
        }
        else _interactPrompt.enabled = false;
    }

    private void Interact()
    {
        if (_interactiveObject == null || _interactiveObject.Interactive == false || _interactiveObject.RobotNeeded != _robotType) return;
        _interactiveObject.Interact(_robotType, this.gameObject, _controller);
    }
}
