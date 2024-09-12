using UnityEngine;

namespace TinyAdventure
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Locomotion State.OnEnter");
            Animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            Player.MovementController.HandleMovement(Player.Stats.moveSpeed);
        }
    }
}