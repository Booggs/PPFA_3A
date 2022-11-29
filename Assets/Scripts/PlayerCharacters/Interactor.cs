using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GSGD2;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float _interactRange = 2.0f;
    [SerializeField] private GameObject _cameraRoot = null;

    private InteractibleObject _interactiveObject;

    private void Update()
    {
        if (Physics.Linecast(_cameraRoot.transform.position, _cameraRoot.transform.position + _cameraRoot.transform.forward * _interactRange, out var hitInfo))
        {
            _interactiveObject = hitInfo.transform.GetComponentInParent<InteractibleObject>();
            if (_interactiveObject != null)
            {
                LevelReferences.Instance.UIManager.PlayerHUD.InteractPrompt.enabled = true;
            }
        }
        else
        {
            LevelReferences.Instance.UIManager.PlayerHUD.InteractPrompt.enabled = false;
        }
    }
}
