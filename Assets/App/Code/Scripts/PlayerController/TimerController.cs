using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class TimerController
    {
        private List<Timer> _timers;

        public readonly CountdownTimer JumpTimer;
        public readonly CountdownTimer JumpCooldownTimer;
        public readonly CountdownTimer DashTimer;
        public readonly CountdownTimer DashCooldownTimer;
        public readonly CountdownTimer AttackTimer;
        public readonly CountdownTimer AttackCooldownTimer;

        private readonly PlayerController _player;

        public TimerController(PlayerController playerController)
        {
            _player = playerController;

            JumpTimer = new CountdownTimer(_player.Stats.jumpDuration);
            JumpCooldownTimer = new CountdownTimer(_player.Stats.jumpCoolDown);
            DashTimer = new CountdownTimer(_player.Stats.dashDuration);
            DashCooldownTimer = new CountdownTimer(_player.Stats.dashCoolDown);
            AttackTimer = new CountdownTimer(_player.Stats.attackDuration);
            AttackCooldownTimer = new CountdownTimer(_player.Stats.attackCooldown);

            _timers = new List<Timer>(6)
            {
                JumpTimer,
                JumpCooldownTimer,
                DashTimer,
                DashCooldownTimer,
                AttackTimer,
                AttackCooldownTimer
            };
        }

        public void HandleTimer()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
    }
}