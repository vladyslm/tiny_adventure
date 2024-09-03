using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}