using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public abstract class PooledMonobehaviour : MonoBehaviour
    {

        [SerializeField]
        protected int _initializeSize = 100;

        public int InitializeSize { get { return _initializeSize; } }

        [SerializeField]
        protected string _poolKey = "key";

        public string PoolKey { get { return _poolKey; } }

        public bool IsActive { get; protected set; }

        private void OnEnable()
        {
            IsActive = true;
        }

        private void OnDisable()
        {
            IsActive = false;
        }

        private void OnDestroy()
        {
            if (PooledObjectManager.Instance != null)
            {
                PooledObjectManager.Instance.RemoveFromPool(_poolKey, this);
            }
        }

        public T Get<T>(bool enable) where T : PooledMonobehaviour
        {
            var pooledObject = PooledObjectManager.Instance.Get(_poolKey, this as T);
            pooledObject.SetActive(enable);
            return pooledObject;
        }

        public T Get<T>(bool enable, Transform parent, bool worldPositionStays = true) where T : PooledMonobehaviour
        {
            var pooledObject = Get<T>(enable);
            pooledObject.transform.SafeSetParent(parent, worldPositionStays);
            return pooledObject;
        }

    }
}
