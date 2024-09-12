using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class AttackState : BaseState
    {
        public AttackState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        private int _streak;
        private bool _isFirstAttack;

        private const int MaxStreak = 5;

        public override void OnEnter()
        {
            _isFirstAttack = true;
            Debug.Log("Attack State.OnEnter");
            PlayAttackAnimation();
        }

        public override void FixedUpdate()
        {
            Player.TimerController.AttackTimer.Reset();
            // Player.TestCoroutine(Test());
            Player.MovementController.HandleMovement(50);
        }
        

        private void PlayAttackAnimation()
        {
            Player.TestCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            if (Player.StateMachine.CurrentState is not AttackState && !_isFirstAttack)
            {
                yield break;
            }
            Debug.Log($"Streak: {_streak}");
 
            switch (_streak)
            {
                case 0 when _isFirstAttack:
                    _isFirstAttack = false;
                    yield return FastAttack();
                    if (_streak == 1)
                    {
                       yield return Attack();
                    }
                    break;
                
                case >= 1 and <= 3:
                    yield return FastAttack();
                    yield return Attack();
                    break;
                case 4:
                    yield return StabbingAttack();
                    yield return Attack();
                    break;
                case MaxStreak:
                    yield return SpinAttack();
                    break;
            }

            _streak = 0;
            Player.TimerController.AttackTimer.Stop();
        }

        private IEnumerator FastAttack()
        {
            Animator.Play(Player.Animations.FastAttackHash);
            yield return new WaitForSeconds(Player.Animations.fastAttackLength);
            HandleStreak();
        }

        private IEnumerator SpinAttack()
        {
            Animator.Play(Player.Animations.SpinningAttackHash);
            yield return new WaitForSeconds(Player.Animations.spinAttackLength);
            HandleStreak();
        }

        private IEnumerator StabbingAttack()
        {
            Animator.Play(Player.Animations.StabbingAttackHash);
            yield return new WaitForSeconds(Player.Animations.stabbingAttackLength);
            HandleStreak();
        }

        private void HandleStreak()
        {
            if (Player.IsAttackChained && _streak < MaxStreak)
            {
                _streak++;
                Player.IsAttackChained = false;
            }
            else
            {
                _streak = 0;
            }
        }
    }
}