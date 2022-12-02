using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int _moneyAmount = 0;

    public int MoneyAmount => _moneyAmount;

    public delegate void OnAddMoney(int moneyAmount);

    public event OnAddMoney AddMoneyEvent;

    public void AddMoney(int moneyAmount)
    {
        _moneyAmount += moneyAmount;
        AddMoneyEvent.Invoke(_moneyAmount);
    }
}
