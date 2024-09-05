using UnityEngine;

namespace TinyAdventure
{
    public class AttackState : BaseState
    {
        public AttackState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            if (Player.CurrentSpeed > 0)
            {
                Animator.CrossFade(MovingAttackHash, 0);
                return;
            }
            
            Animator.CrossFade(AttackHash, 0);
        }

        public override void FixedUpdate()
        {
            Player.HandleMovement();
        }
    }
}