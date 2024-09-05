using UnityEngine;

namespace TinyAdventure
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController Player;
        protected readonly Animator Animator;
        
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int DashHash = Animator.StringToHash("Dash");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");
        protected static readonly int MovingAttackHash = Animator.StringToHash("MovingAttack");
        
        protected const float CrossFadeDuration = 0.2f;

        protected BaseState(PlayerController player, Animator animator)
        {
            Player = player;
            Animator = animator;
        }
        
        public virtual void OnEnter()
        {
            // to implement
        }

        public virtual void Update()
        {
            // to implement
        }

        public virtual void FixedUpdate()
        {
            // to implement
        }

        public virtual void OnExit()
        {
            // to implement
            Debug.Log("BaseState.OnExit");
        }
    }
}