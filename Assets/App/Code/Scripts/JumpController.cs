using UnityEngine;

namespace TinyAdventure
{
    public interface IJumpController
    {
        void HandleJump();
        void OnJumpTimerStart();
    }

    public class JumpController : IJumpController
    {
        public JumpController(PlayerController playerController)
        {
            _player = playerController;
        }

        private readonly PlayerController _player;
        private float _jumpVelocity;
        private const float ZeroF = 0f;

        public void HandleJump()
        {
            switch (_player._jumpTimer.IsRunning)
            {
                case false when _player.groundChecker.IsGrounded:
                    _jumpVelocity = ZeroF;
                    _player._jumpTimer.Stop();
                    return;
                case false:
                    _jumpVelocity += Physics.gravity.y * _player.Stats.gravityMultiplier * Time.fixedDeltaTime;
                    break;
            }

            _player.rb.velocity = new Vector3(_player.rb.velocity.x, _jumpVelocity, _player.rb.velocity.z);
        }

        public void OnJumpTimerStart()
        {
            _jumpVelocity = _player.Stats.jumpForce;
        }
    }
}