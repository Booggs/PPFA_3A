using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float _gravityStrength = 15.0f;

    private bool _zeroGravity = false;
    private Vector3 _gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);
    private float _startingGravityStrength = 15.0f;

    public float GravityStrength => _gravityStrength;
    public Vector3 GravityDirection => _gravityDirection;
    public bool ZeroGravity => _zeroGravity;

    public struct GravityChangeArgs
    {
        public float gravityStrength;
        public Vector3 gravityDirection;

        public GravityChangeArgs(float gravityStrength, Vector3 gravityDirection)
        {
            this.gravityStrength = gravityStrength;
            this.gravityDirection = gravityDirection;
        }
    }

    public delegate void GravityChangeEvent(GravityChangeArgs args);

    public delegate void SetZeroGravityEvent(bool zeroGravity);



    public event GravityChangeEvent GravityChange = null;
    public event SetZeroGravityEvent SetZeroGravity = null;

    private void OnStart()
    {
        _startingGravityStrength = _gravityStrength;
    }

    public void ChangeGravity(GravityChangeArgs args)
    {
        GravityChange.Invoke(args);
        _gravityDirection = args.gravityDirection;
        _gravityStrength = args.gravityStrength;
    }

    public void EnableZeroGravity(bool zeroGravity)
    {
        SetZeroGravity.Invoke(zeroGravity);
        _gravityStrength = zeroGravity == true ? 0.0f : 15.0f;
    }
}
