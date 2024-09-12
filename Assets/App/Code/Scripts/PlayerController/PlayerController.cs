using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TinyAdventure
{
    public class PlayerController : MonoBehaviour
    {
        // Actions
        public event UnityAction<bool> OnPlayerRun = delegate { };

        [Header("References")] [SerializeField]
        private InputReader input;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private ControllerStats stats;
        [SerializeField] private Animations animations;

        // Getters
        public StateMachine StateMachine => _stateMachine;
        public Rigidbody Rb => rb;
        public Vector3 Movement => _movement;
        public GroundChecker GroundChecker => groundChecker;
        public ControllerStats Stats => stats;
        public Animations Animations => animations;
        public Transform MainCameraTransform => _mainCamera;
        public bool IsRunnint => _isRunning;

        public bool IsAttackChained
        {
            get => _isAttackChained;
            set => _isAttackChained = value;
        }


        // Controllers
        public TimerController TimerController;
        public IMovementController MovementController;
        public IJumpController JumpController;
        public AnimatorController AnimatorController;

        // State Machine
        private StateMachine _stateMachine;

        // Private Values
        private Vector3 _movement = Vector3.zero;
        private Transform _mainCamera;
        private float _currentSpeed;


        // public float _velocity;
        // private float _jumpVelocity;

        // private float _dashVelocity = 1f;


        private float _lastTimeRunButtonPressed;
        private bool _isOnRunWasSent;
        private bool _isRunning = false;
        // private float _attackStreak;
        private bool _isAttackChained; 

        public float CurrentSpeed
        {
            get => _currentSpeed;
            set => _currentSpeed = value;
        }


        private void Awake()
        {
            SetupCamera();
            SetupRigidbody();

            // Setup Controllers
            SetupControllers();

            // Setup Timers
            SetupTimerEvents();

            // Setup State Machine
            SetupStateMachine();
        }

        private void OnEnable()
        {
            input.Attack += OnAttack;
            input.Jump += OnJump;
            // input.Dash += OnDash;
            input.Run += OnRun;
        }

        private void OnDisable()
        {
            input.Attack -= OnAttack;
            input.Jump -= OnJump;
            // input.Dash -= OnDash;
            input.Run -= OnRun;
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void Update()
        {
            _movement = new Vector3(input.Direction.x, 0, input.Direction.y);

            _stateMachine.Update();

            TimerController.HandleTimer();
            AnimatorController.UpdateAnimator();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
            HandleRunAction();
        }


        private void SetupControllers()
        {
            TimerController = new TimerController(this);
            MovementController = new MovementController(this);
            JumpController = new JumpController(this);
            AnimatorController = new AnimatorController(this, animator);
        }

        private void SetupStateMachine()
        {
            // State Machine
            _stateMachine = new StateMachine();

            // Declare States
            var locomotionState = new LocomotionState(this, animator);
            var attackState = new AttackState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            var runState = new RunState(this, animator);


            // Define Transitions
            At(locomotionState, jumpState, new FuncPredicate(() => TimerController.JumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => TimerController.DashTimer.IsRunning));
            At(locomotionState, runState, new FuncPredicate((() => _isRunning)));
            At(dashState, runState, new FuncPredicate(() => _isRunning && !TimerController.DashTimer.IsRunning));
            
            // At(attackState, runState, new FuncPredicate((() => _isRunning && !TimerController.AttackTimer.IsRunning)));
            At(attackState, jumpState, new FuncPredicate(() => TimerController.JumpTimer.IsRunning && !TimerController.AttackTimer.IsRunning && GroundChecker.IsGrounded));
            At(attackState, runState,  new FuncPredicate(() => !TimerController.AttackTimer.IsRunning && GroundChecker.IsGrounded && _isRunning));
            
            Any(attackState,
                new FuncPredicate(() => TimerController.AttackTimer.IsRunning && groundChecker.IsGrounded));

            Any(locomotionState,
                new FuncPredicate(() =>
                    groundChecker.IsGrounded && !TimerController.JumpTimer.IsRunning &&
                    !TimerController.DashTimer.IsRunning &&
                    !TimerController.AttackTimer.IsRunning
                    && !_isRunning));

            _stateMachine.SetState(locomotionState);
        }

        private void SetupTimerEvents()
        {
            TimerController.JumpTimer.OnTimerStart += () => JumpController.OnJumpTimerStart();
            TimerController.JumpTimer.OnTimerStop += () => TimerController.JumpCooldownTimer.Start();

            // TimerController.DashTimer.OnTimerStart += () => _dashVelocity = stats.dashForce;
            TimerController.DashTimer.OnTimerStop += () =>
            {
                // _dashVelocity = 1f;
                TimerController.DashCooldownTimer.Start();
                // _isRunning = true;
            };

            // Attack timers
            TimerController.AttackTimer.OnTimerStop += () => { TimerController.AttackCooldownTimer.Start(); };
        }


        private void SetupRigidbody()
        {
            rb.freezeRotation = true;
        }

        private void SetupCamera()
        {
            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);


        // Events
        private void OnAttack()
        {
            // if (!TimerController.AttackTimer.IsRunning && !TimerController.AttackCooldownTimer.IsRunning)
            if (!TimerController.AttackTimer.IsRunning)
                TimerController.AttackTimer.Start();
            else
            {
                _isAttackChained = true;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    var currentTime = Time.time;

                    if (currentTime - _lastTimeRunButtonPressed > 0.5f)
                    {
                       _lastTimeRunButtonPressed = Time.time; 
                       break;
                    }
                        
                    if (currentTime - _lastTimeRunButtonPressed < 0.5f)
                    {
                        Debug.Log("Double Press");
                        _isRunning = false;
                        TimerController.DashTimer.Start();
                    }
                    break;
                case InputActionPhase.Performed:
                    Debug.Log("Performed");
                    _isRunning = true;
                    break;
                case InputActionPhase.Canceled:
                    _isRunning = false;
                    break;
                    
            }
            // StartCoroutine(CheckForDoublePress());
        }

        // private void OnDash(bool performed)
        // {
        //     Debug.Log($"Dash: {performed}");
        //     // TimerController.DashTimer.Start();
        //     
        //     // if (performed && !TimerController.DashCooldownTimer.IsRunning && groundChecker.IsGrounded)
        //     // {
        //     //     TimerController.DashTimer.Start();
        //     // }
        //     // else if (!performed && TimerController.DashTimer.IsRunning)
        //     // {
        //     //     TimerController.DashTimer.Stop();
        //     // }
        // }

        private void OnJump(bool performed)
        {
            TimerController.AttackTimer.Stop();
            if (performed && !TimerController.JumpTimer.IsRunning && !TimerController.JumpCooldownTimer.IsRunning &&
                groundChecker.IsGrounded)
            {
                TimerController.JumpTimer.Start();
            }
            else if (!performed && TimerController.JumpTimer.IsRunning)
            {
                TimerController.JumpTimer.Stop();
            }
        }


        private void HandleRunAction()
        {
            // if (_currentSpeed > 0 && groundChecker.IsGrounded)
            if ((_stateMachine.CurrentState is RunState) && groundChecker.IsGrounded)
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

        public void TestCoroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}