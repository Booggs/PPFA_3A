using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;
using System.Timers;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]

    public class RobotBaseController : MonoBehaviour
    {
        public ERobotType RobotType = ERobotType.Drone;

        [Header("Controller")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;

        //[Tooltip("Sprint speed of the character in m/s")]
        //private float SprintSpeed = 6.0f;

        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 1000.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;


        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;

        [Header("HUD")]
        [Tooltip("The HUD Menu associated with the robot")]
        public PlayerHUDMenu RobotHud = null;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player
        protected float _verticalVelocity;
        protected bool _controllerPossessed = false;
        private float _speed;
        private float _rotationVelocity;
        private float _groundedOffset;
        private readonly float _terminalVelocity = 53.0f;
        private bool _invertGravity = false;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _previousPosition = Vector3.zero;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private float _gravityStrength = -15.0f;

        protected StarterAssetsInputs Input;

        protected CustomPlayerGravity _customPlayerGravity;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        protected PlayerInput PlayerInput;
#endif
        private Interactor _interactor = null;
        private CharacterController _controller;
        private GameObject _mainCamera;
        private Timer _invertGravityTimer = new Timer();

        private const float _threshold = 0.01f;

        public Vector3 Velocity => _velocity;

        public delegate void PauseMenuPerformedEvent();

        public event PauseMenuPerformedEvent PauseMenuPerformed = null;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return PlayerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        #region Callbacks 

        private void InvertGravityEnable(CustomPlayerGravity customGravity, bool enable)
        {
            _invertGravity = enable;
            _gravityStrength = customGravity.GravityStrength;
            _groundedOffset *= -1;
            _invertGravityTimer.Start(0.5f);
            _verticalVelocity = 0.0f;
            PlayerInput.DeactivateInput();
        }
        #endregion

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _controller = GetComponent<CharacterController>();
            _customPlayerGravity = GetComponent<CustomPlayerGravity>();
            _interactor = GetComponent<Interactor>();
        }

        protected virtual void Start()
        {
            Input = LevelReferences.Instance.Input;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            PlayerInput = LevelReferences.Instance.PlayerInput;
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            _groundedOffset = GroundedOffset;
            _gravityStrength = _customPlayerGravity.GravityStrength;
            _previousPosition = transform.position;
            _customPlayerGravity.SetInvertGravity -= InvertGravityEnable;
            _customPlayerGravity.SetInvertGravity += InvertGravityEnable;
        }

        protected virtual void Update()
        {
            CalculateVelocity();
            JumpAndGravity();
            GroundedCheck();
            Move();
            if (_invertGravityTimer.Update() && _invertGravityTimer.CurrentState == Timer.State.Finished)
            {
                PlayerInput.ActivateInput();
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input
            if (Input.look.sqrMagnitude >= _threshold && _controllerPossessed)
            {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += Input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = Input.look.x * RotationSpeed * deltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        protected void Move()
        {
            // set target speed based on move speed, sprint is disabled
            float targetSpeed = MoveSpeed; //Input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (Input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = Input.analogMovement ? Input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(Input.move.x, 0.0f, Input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (Input.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * Input.move.x + transform.forward * Input.move.y;
            }
            
            // move the player
            _controller.Move((_controllerPossessed ? inputDirection.normalized * (_speed * Time.deltaTime) : new Vector3(0, 0, 0)) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        protected virtual void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = _invertGravity == true ? 2.0f : -2.0f;
                }

                // Jump
                if (Input.jump && _jumpTimeoutDelta <= 0.0f && _controllerPossessed)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2.0f * Mathf.Abs(_gravityStrength) * -1.0f) * (_invertGravity == true ? -1.0f : 1.0f);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                // if we are not grounded, do not jump
                Input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_invertGravity)
            {
                if (_verticalVelocity > _terminalVelocity * -1.0f)
                {
                    _verticalVelocity += _gravityStrength * Time.deltaTime;
                }
            }
            else if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _gravityStrength * Time.deltaTime;
            }
        }

        public virtual void SetControllerPossessed(bool possessed)
        {
            _controllerPossessed = possessed;
            _interactor.enabled = _controllerPossessed;
        }

        protected void CalculateVelocity()
        {
            _velocity = (transform.position - _previousPosition) / Time.deltaTime;
            _previousPosition = transform.position;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = Grounded ? transparentGreen : transparentRed;
            _groundedOffset = GroundedOffset;
            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), GroundedRadius);
        }
    }
}