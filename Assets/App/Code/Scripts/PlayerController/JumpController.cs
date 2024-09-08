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
            switch (_player.TimerController.JumpTimer.IsRunning)
            {
                case false when _player.GroundChecker.IsGrounded:
                    _jumpVelocity = ZeroF;
                    _player.TimerController.JumpTimer.Stop();
                    return;
                case false:
                    _jumpVelocity += Physics.gravity.y * _player.Stats.gravityMultiplier * Time.fixedDeltaTime;
                    break;
            }

            _player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _jumpVelocity, _player.Rb.velocity.z);
        }

        public void OnJumpTimerStart()
        {
            _jumpVelocity = _player.Stats.jumpForce;
        }
    }
}