using UnityEngine;

namespace TinyAdventure
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Jump State.OnEnter");
            Animator.CrossFade(JumpHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            // Player.HandleJump();
            Player.JumpController.HandleJump();
            Player.MovementController.HandleMovement();
        }
    }
}