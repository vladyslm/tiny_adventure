using UnityEngine;

namespace TinyAdventure
{
    public class RunState : BaseState
    {
        public RunState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Run State.OnEnter");
            Animator.Play(Player.Animations.RunHash);
        }

        public override void FixedUpdate()
        {
            Player.MovementController.HandleMovement(600);
        }
    }
}