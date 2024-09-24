using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class HitVFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] vfxParticles;
        
        public void PlayHitVFX()
        {
            var effect = vfxParticles[Random.Range(0, vfxParticles.Length)];
            if (!effect.gameObject.activeSelf)
            {
                effect.gameObject.SetActive(true);
                return;
            }
            effect.Simulate(0, true, true);
            effect.Play();
        }
    }
}
