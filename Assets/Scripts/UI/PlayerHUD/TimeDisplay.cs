using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    private Text _timeText;
    private string _timeString;

    private void Start()
    {
        _timeText = GetComponent<Text>();
    }

    private void Update()
    {
        int minutes = Mathf.FloorToInt(Time.fixedTime / 60.0f);
        int seconds = Mathf.FloorToInt(Time.fixedTime - minutes * 60);
        _timeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        _timeText.text = _timeString;
    }
}