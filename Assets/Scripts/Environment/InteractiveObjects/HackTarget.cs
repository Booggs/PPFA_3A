using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : MonoBehaviour
{
    [SerializeField] 
    private int _hacksNeeded = 1;

    private int _currentHacks = 0;
    private bool _hacked = false;

    public void SetHackStatus(bool status)
    {
        if (_hacked == status) return;

        _hacked = status;
        gameObject.SetActive(!_hacked);
    }

    public void ModifyCurrentHacks(int hacksAmount)
    {
        _currentHacks += hacksAmount;
        SetHackStatus(_currentHacks >= _hacksNeeded ? true : false);
    }
}
