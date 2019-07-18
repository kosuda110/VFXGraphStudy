using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Kosu.UnityLibrary
{
    public class PoolUserTest : MonoBehaviour
    {

        [SerializeField]
        private PoolObjectTest _prefab = null;

        private void Awake()
        {
            PooledObjectManager.Instance.Initialize(_prefab.PoolKey, _prefab);
        }

        private void Start()
        {
            Observable.Interval(System.TimeSpan.FromMilliseconds(10)).Subscribe(_ => UpdateParticle()).AddTo(gameObject);
        }

        private void UpdateParticle()
        {
            var particle = _prefab.Get<PoolObjectTest>(true);
            particle.transform.SetGlobalPositionX(Random.Range(-10, 10));
            particle.transform.SetGlobalPositionY(Random.Range(-10, 10));
            particle.PlayParticle();
        }

    }
}
