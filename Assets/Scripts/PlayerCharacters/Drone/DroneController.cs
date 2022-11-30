using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DroneController : RobotBaseController
{
    [Header("Drone")]
    [SerializeField] 
    private float _flightSpeed = 3.0f;


    private new void Start()
    {
        base.Start();
        _customPlayerGravity.CustomGravityActive = false;
    }

    private new void Update()
    {
        CalculateVelocity();
        DroneFlight();
        Move();
    }

    private void DroneFlight()
    {
        _verticalVelocity = Input.verticalMove * _flightSpeed;
    }
}