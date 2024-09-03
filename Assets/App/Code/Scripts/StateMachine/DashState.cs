﻿using UnityEngine;

namespace TinyAdventure
{
    public class DashState:BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Dash State.OnEnter");
            Animator.CrossFade(DashHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            Player.HandleMovement();
        }
    }
}