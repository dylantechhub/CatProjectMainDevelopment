using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        //[HideInInspector]
        public bool canclimb = false;
        //Prevents the cat from climbing for 5s
        private float ClimbCooldown = 5f; 
        [Tooltip("Players climb raycast system.")]
        public LayerMask hitlayers;
        [Tooltip("Where the raycast is")]
        public Vector3 Raycastpoint;
        [Tooltip("")]
        public float SideRadius = 0.1f;
        [Tooltip("")]
        public float Sideoffset = 0.1f;
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;
        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;
        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        [HideInInspector]
        public float _cinemachineTargetYaw;
        [HideInInspector]
        public float _cinemachineTargetPitch;

        // player
        [HideInInspector]
        public float _speed;
        private float _animationBlend;
        [HideInInspector]
        public float _targetRotation = 0.0f;
        [HideInInspector]
        public float _rotationVelocity;
        public float _verticalVelocity;

        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        [HideInInspector]
        public int _animIDSpeed;
        [HideInInspector]
        public int _animIDGrounded;
        [HideInInspector]
        public int _animIDJump;
        [HideInInspector]
        public int _animIDFreeFall;
        [HideInInspector]
        public int _animIDMotionSpeed;

        [HideInInspector]
        public Animator _animator;
        [HideInInspector]
        public CharacterController _controller;
        //[HideInInspector]
        public StarterAssetsInputs _input;
        [HideInInspector]
        public GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }
        public bool StopClimbing;

        private void Update()
        {
            ClimbCooldown -= Time.deltaTime;
            _hasAnimator = TryGetComponent(out _animator);
            JumpAndGravity();
            GroundedCheck();
            Move();
            if (!StopClimbing)
            Climbcast();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
               // _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
               //_cinemachineTargetPitch += _input.look.y * Time.deltaTime;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Climbcast()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + Raycastpoint, transform.TransformDirection(Vector3.forward), out hit, 0.7f, hitlayers))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                canclimb = true;
                Gravity = 0; 
            }
            else
            {
                canclimb = false;
                Gravity = -15f;
                Debug.DrawRay(transform.position + Raycastpoint, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }
        }

        [HideInInspector]
        public Vector3 targetDirection;
        public Vector3 inputDirection;

        public bool sprinting;
        public bool moving;

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float speedOffset = 0.1f;
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
           /* if (canclimb)
            {
                if (currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    currentHorizontalSpeed = Climbrunspeed;
                }
                else if(currentHorizontalSpeed > targetSpeed - speedOffset)
                {
                    currentHorizontalSpeed -= Climbrunspeed;
                }

            }
            else
            {

            }*/
            
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset && !canclimb || currentHorizontalSpeed > targetSpeed + speedOffset && !canclimb)
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


            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero && !canclimb)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            if (!canclimb)
            {
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
            else if (canclimb)
            {
                if (_input.move.y > 0.1f)
                {
                    //gameObject.transform.position += new Vector3(0, 2 * Time.deltaTime, 0);
                    _controller.Move(new Vector3(0, 2 * Time.deltaTime, 0));
                }
                if (_input.move.y < -0.1f)
                {
                    //gameObject.transform.position += new Vector3(0, -2 * Time.deltaTime, 0);
                    _controller.Move(new Vector3(0, -2 * Time.deltaTime, 0));
                }
                if (_input.move.x < -0.1f)
                {
                    //gameObject.transform.Translate(-2 * Time.deltaTime, 0, 0);
                    _controller.Move(new Vector3(-2 * Time.deltaTime, 0, 0));
                }
                if (_input.move.x > 0.1f)
                {
                    //gameObject.transform.Translate(2 * Time.deltaTime, 0, 0);
                    _controller.Move(new Vector3(2 * Time.deltaTime, 0, 0));
                }
                
                if (canclimb && _input.jump)
                {
                    print(canclimb);
                    StopClimbing = true;
                    Gravity = -15f;
                    Quaternion targetRotation1 = Quaternion.Euler(0, -180, 0);
                    Quaternion targetRotation2 = Quaternion.Euler(0, -180, 0);

                    if (gameObject.transform.rotation.eulerAngles.y.ToString().Contains("-"))
                        gameObject.transform.rotation = targetRotation1;
                    else
                        gameObject.transform.rotation = targetRotation2;
                    canclimb = false;
                }
            }

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public bool jump;

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                StopClimbing = false;
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump || jump)
                {
                    if (_jumpTimeoutDelta <= 0.0f)
                    {
                        // the square root of H * -2 * G = how much velocity needed to reach desired height
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                        // update animator if using character
                        if (_hasAnimator)
                        {
                            _animator.SetBool(_animIDJump, true);
                        }
                    }
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
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                if (!canclimb)
                {
                    _input.jump = false;
                    jump = false;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            //if(_ClimbVeclocity)
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

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}