using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }
    
    public class HealthComponent : MonoBehaviour
    {
        
    }
}
