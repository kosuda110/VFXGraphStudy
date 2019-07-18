using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class PoolObjectTest : PooledMonobehaviour
    {

        [SerializeField]
        private ParticleSystem _particle = null;

        public void PrintName()
        {
            Debug.Log(gameObject.name);
        }

        public void PlayParticle()
        {
            _particle.Play();
            IsActive = true;
            StartCoroutine(CheckParticlePlaying());
        }

        private IEnumerator CheckParticlePlaying()
        {
            yield return new WaitUntil(() => _particle.particleCount == 0 && _particle.isPlaying);
            IsActive = false;
        }

    }
}
