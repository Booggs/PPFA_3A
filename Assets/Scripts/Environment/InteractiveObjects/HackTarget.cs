using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : MonoBehaviour
{
    private bool _hacked = false;

    public void SetHackStatus(bool status)
    {
        if (_hacked == status) return;

        _hacked = status;
        gameObject.SetActive(!_hacked);
    }
}
