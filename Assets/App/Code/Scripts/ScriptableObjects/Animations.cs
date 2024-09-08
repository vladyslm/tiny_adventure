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
        private const string MovingAnimation = "MovingAttack";

        // Hashes
        public readonly int LocomotionHash = Animator.StringToHash(LocomotionAnimation);
        public readonly int JumpHash = Animator.StringToHash(JumpAnimation);
        public readonly int DashHash = Animator.StringToHash(DashAnimation);
        public readonly int AttackHash = Animator.StringToHash(AttackAnimation);
        public readonly int MovingAttackHash = Animator.StringToHash(MovingAnimation);
        
        // Animator Parameters
        public readonly int Speed = Animator.StringToHash("Speed");
    }
}