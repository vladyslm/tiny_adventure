using UnityEngine;

namespace TinyAdventure
{
    public class AnimatorController
    {
        private readonly PlayerController _player;
        private readonly Animator _animator;

        public AnimatorController(PlayerController player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        public void UpdateAnimator()
        {
            _animator.SetFloat(_player.Animations.Speed, _player.CurrentSpeed);
        }
    }
}