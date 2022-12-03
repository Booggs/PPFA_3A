using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StarterAssets;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CustomPlayerGravity : MonoBehaviour
{
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float _gravityStrength = -15.0f;

    private bool _customGravityActive = true;
    private bool _zeroGravity = false;
    private bool _invertGravity = false;
    private float _startingGravityStrength = -15.0f;
    private CharacterController _controller = null;

    public float GravityStrength => _gravityStrength;
    public bool ZeroGravity => _zeroGravity;
    public bool InvertGravity => _invertGravity;

    public bool CustomGravityActive
    {
        get
        {
            return _customGravityActive;
        }
        set
        {
            _customGravityActive = value;
        }
    }

    #region Deprecated Gravity Direction Change

    //public struct GravityChangeArgs
    //{
    //    public float gravityStrength;
    //    public Vector3 gravityDirection;

    //    public GravityChangeArgs(float gravityStrength, Vector3 gravityDirection)
    //    {
    //        this.gravityStrength = gravityStrength;
    //        this.gravityDirection = gravityDirection;
    //    }
    //}

    //public delegate void GravityChangeEvent(GravityChangeArgs args);
    //public event GravityChangeEvent GravityChange = null;
    //public void ChangeGravity(GravityChangeArgs args)
    //{
    //    GravityChange.Invoke(args);
    //    _gravityDirection = args.gravityDirection;
    //    _gravityStrength = args.gravityStrength;
    //}

    #endregion

    public delegate void SetZeroGravityEvent(bool zeroGravity);
    public delegate void SetInvertGravityEvent(CustomPlayerGravity customGravity, bool invertGravity);


    public event SetZeroGravityEvent SetZeroGravity = null;
    public event SetInvertGravityEvent SetInvertGravity = null;

    private void Start()
    {
        _startingGravityStrength = _gravityStrength;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

    }
    
    public void EnableZeroGravity(bool zeroGravity)
    {
        if (_customGravityActive == false) return;
        _zeroGravity = zeroGravity;
        _gravityStrength = _zeroGravity == true ? 0.0f : 15.0f;
        SetZeroGravity.Invoke(zeroGravity);
    }

    public void EnableInvertGravity(bool invertGravity)
    {
        if (invertGravity == _invertGravity || _customGravityActive == false) return;
        _invertGravity = invertGravity;

        _gravityStrength = _invertGravity == true ? _startingGravityStrength * -1.0f : _startingGravityStrength * 1.0f;
        transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 180);
        SetInvertGravity.Invoke(this, _invertGravity);
    }
}