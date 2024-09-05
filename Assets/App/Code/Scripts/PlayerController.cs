using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace TinyAdventure
{
    public class PlayerController : MonoBehaviour
    {
        // Actions
        public event UnityAction<bool> OnPlayerRun = delegate { };

        [Header("References")] [SerializeField]
        private InputReader input;

        [SerializeField] private Rigidbody rb;

        // [SerializeField] private CharacterController controller;
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private Animator animator;
        [SerializeField] private GroundChecker groundChecker;

        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 6f;

        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = .2f;

        [Header("Jump Setting")] [SerializeField]
        private float jumpForce = 15f;

        [SerializeField] private float jumpMaxHeight = 2f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpCoolDown = 0;
        [SerializeField] private float gravityMultiplier = 3f;

        [Header("Dash Settings")] [SerializeField]
        private float dashForce = 2f;

        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCoolDown = 0f;

        [Header("Attack Settings")] [SerializeField]
        private float attackDuration = 0.1f;

        [SerializeField] private float attackCooldown = 0;


        // State Machine
        private StateMachine _stateMachine;

        // Private Values
        private Vector3 _movement = Vector3.zero;
        private Transform _mainCamera;
        private float _currentSpeed;
        private float _velocity;
        private float _jumpVelocity;

        private float _dashVelocity = 1f;


        List<Timer> _timers;
        private CountdownTimer _jumpTimer;
        private CountdownTimer _jumpCooldownTimer;
        private CountdownTimer _dashTimer;
        private CountdownTimer _dashCooldownTimer;

        private CountdownTimer _attackTimer;
        private CountdownTimer _attackCooldownTimer;

        private bool _isOnRunWasSent;
        
        public float CurrentSpeed => _currentSpeed;


        // Constant Values
        private const float ZeroF = 0;

        private

            // Animator parameters
            static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            if (Camera.main != null) _mainCamera = Camera.main.transform;
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
            freeLookCamera.OnTargetObjectWarped(
                transform,
                transform.position - freeLookCamera.transform.position - Vector3.forward
            );

            rb.freezeRotation = true;

            // Setup Timers
            _jumpTimer = new CountdownTimer(jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(jumpCoolDown);
            _dashTimer = new CountdownTimer(dashDuration);
            _dashCooldownTimer = new CountdownTimer(dashCoolDown);
            _attackTimer = new CountdownTimer(attackDuration);
            _attackCooldownTimer = new CountdownTimer(attackDuration);

            _timers = new List<Timer>(6)
                { _jumpTimer, _jumpCooldownTimer, _dashTimer, _dashCooldownTimer, _attackTimer, _attackCooldownTimer };

            _jumpTimer.OnTimerStart += () => _jumpVelocity = jumpForce;
            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();

            _dashTimer.OnTimerStart += () => _dashVelocity = dashForce;
            _dashTimer.OnTimerStop += () =>
            {
                _dashVelocity = 1f;
                _dashCooldownTimer.Start();
            };

            // _attackTimer.OnTimerStart += () => { Debug.Log("Starting attack"); };

            // Attatk timers
            _attackTimer.OnTimerStop += () =>
            {
                _attackCooldownTimer.Start();
                // Debug.Log("Attack ended");
            };


            // State Machine
            _stateMachine = new StateMachine();

            // Declare States
            var locomotionState = new LocomotionState(this, animator);
            var attackState = new AttackState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            

            // Define Transitions
            At(locomotionState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => _dashTimer.IsRunning));
            
            Any(attackState, new FuncPredicate(() => _attackTimer.IsRunning && groundChecker.IsGrounded));
            // At(locomotionState, attackState, new FuncPredicate(() => _attackTimer.IsRunning));
            // At(locomotionState, attackState, new FuncPredicate(() => _jumpTimer.IsRunning));
            
            // At(attackState, locomotionState, new FuncPredicate(() => !_attackTimer.IsRunning && groundChecker.IsGrounded));
            // At(attackState, jumpState, new FuncPredicate(() => !_attackTimer.IsRunning && !groundChecker.IsGrounded));

            Any(locomotionState,
                new FuncPredicate(() => groundChecker.IsGrounded && !_jumpTimer.IsRunning && !_dashTimer.IsRunning && !_attackTimer.IsRunning));

            _stateMachine.SetState(locomotionState);
        }

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);


        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            input.Attack += OnAttack;
            input.Jump += OnJump;
            input.Dash += OnDash;
        }

        private void OnDisable()
        {
            input.Attack -= OnAttack;
            input.Jump -= OnJump;
            input.Dash -= OnDash;
        }

        private void OnAttack()
        {
           if(!_attackTimer.IsRunning && !_attackCooldownTimer.IsRunning)
               _attackTimer.Start();
        }

        private void OnDash(bool performed)
        {
            if (performed && !_dashCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                _dashTimer.Start();
            }
            else if (!performed && _dashTimer.IsRunning)
            {
                _dashTimer.Stop();
            }
        }

        private void OnJump(bool performed)
        {
            if (performed && !_jumpTimer.IsRunning && !_jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpTimer.Start();
            }
            else if (!performed && _jumpTimer.IsRunning)
            {
                _jumpTimer.Stop();
            }
        }


        private void Update()
        {
            _movement = new Vector3(input.Direction.x, 0, input.Direction.y);

            _stateMachine.Update();

            // HandleMovement();
            HandleTimers();
            UpdateAnimator();
            // HandleRunAction();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
            HandleRunAction();
        }

        public void HandleJump()
        {
            if (!_jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpVelocity = ZeroF;
                _jumpTimer.Stop();
                return;
            }

            if (!_jumpTimer.IsRunning)
            {
                _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            rb.velocity = new Vector3(rb.velocity.x, _jumpVelocity, rb.velocity.z);
        }

        private void HandleTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        public void HandleMovement()
        {
            // var movementDirection = new Vector3(input.Direction.x, 0, input.Direction.y);

            var adjustedDirection = Quaternion.AngleAxis(_mainCamera.eulerAngles.y, Vector3.up) * _movement;
            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);

                rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            var velocity = adjustedDirection * (moveSpeed * _dashVelocity * Time.fixedDeltaTime);
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation =
                Quaternion.RotateTowards(targetRotation, transform.rotation, rotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float value)
        {
            if (value == 0)
            {
                _currentSpeed = 0;
                return;
            }

            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }

        private void HandleRunAction()
        {
            if (_currentSpeed > 0 && groundChecker.IsGrounded)
            {
                if (_isOnRunWasSent) return;
                OnPlayerRun?.Invoke(true);
                _isOnRunWasSent = true;
            }
            else
            {
                if (!_isOnRunWasSent) return;
                OnPlayerRun?.Invoke(false);
                _isOnRunWasSent = false;
            }
        }
    }
}