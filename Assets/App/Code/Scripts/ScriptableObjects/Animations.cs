using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    [CreateAssetMenu(menuName = "TinyAdventure/Data/Animations")]
    public class Animations : ScriptableObject
    {
        // Animations
        private const string LocomotionAnimation = "Locomotion";
        private const string JumpAnimation = "Jump";
        private const string DashAnimation = "Dash";
        private const string AttackAnimation = "Attack";
        private const string FastAttackAnim = "Attack2";
        private const string StabbingAttackAnim = "Attack3";
        private const string SpinAttackAnim = "Attack4";
        private const string MovingAnimation = "MovingAttack";

        // Hashes
        public readonly int LocomotionHash = Animator.StringToHash(LocomotionAnimation);
        public readonly int JumpHash = Animator.StringToHash(JumpAnimation);
        public readonly int DashHash = Animator.StringToHash(DashAnimation);
        public readonly int AttackHash = Animator.StringToHash(AttackAnimation);
        public readonly int FastAttackHash = Animator.StringToHash(FastAttackAnim);
        public readonly int StabbingAttackHash = Animator.StringToHash(StabbingAttackAnim);
        public readonly int SpinningAttackHash = Animator.StringToHash(SpinAttackAnim);
        public readonly int MovingAttackHash = Animator.StringToHash(MovingAnimation);
        
        // Animator Parameters
        public readonly int Speed = Animator.StringToHash("Speed");

        [SerializeField] public float attack1Length = 0.25f;
        [SerializeField] public float fastAttackLength = 0.26f;
        [SerializeField] public float stabbingAttackLength = 0.26f;
        [SerializeField] public float spinAttackLength = 1f;
    }
}