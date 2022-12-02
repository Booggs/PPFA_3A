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
    }

    private void Start()
    {
        _robotType = _controller.RobotType;
        _interactPrompt = _controller.RobotHud.InteractPrompt;

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
            if (_interactiveObject == null)
            {
                _interactiveObject = hitInfo.transform.GetComponentInParent<InteractiveObject>();
            }

            if (_interactiveObject != null && _interactiveObject.InteractionValid(_controller.RobotType))
            {
                _interactiveObject.SetInteractionReady(true);
                _interactPrompt.enabled = true;
                _interactPrompt.text = _interactiveObject.InteractPrompt;
            }
            else
            {
                _interactPrompt.enabled = false;
                if (_interactiveObject != null) _interactiveObject.SetInteractionReady(false);
            }
        }
        else
        {
            _interactPrompt.enabled = false;
            if (_interactiveObject != null)
            {
                _interactiveObject.SetInteractionReady(false);
                _interactiveObject = null;
            }
        }
    }

    private void Interact()
    {
        if (_interactiveObject == null || _interactiveObject.InteractionValid(_controller.RobotType) == false) return;
        _interactiveObject.Interact(_robotType, this.gameObject, _controller);
    }
}