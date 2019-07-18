using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kosu.UnityLibrary
{
    public class PooledObjectManager : Singleton<PooledObjectManager>
    {

        private Dictionary<string, List<PooledMonobehaviour>> _pools = new Dictionary<string, List<PooledMonobehaviour>>();

        public override bool IsDontDestroyOnLoad => true;

        protected override void AfterAwake()
        {
            base.AfterAwake();
        }

        protected override void AfterDestroy()
        {
            base.AfterDestroy();
        }

        public void Initialize<T>(string key, T prefab, Transform parent = null) where T : PooledMonobehaviour
        {
            if (_pools.ContainsKey(key))
            {
                return;
            }

            InitPool(key, prefab, parent);
            return;
        }

        public T Get<T>(string key, T prefab) where T : PooledMonobehaviour
        {
            if (!_pools.ContainsKey(key))
            {
                // 初期化
                InitPool(key, prefab);
            }

            // 使えるオブジェクト返却
            var objects = _pools[key];

            if (objects.Count(_ => !_.IsActive) == 0)
            {
                if (objects.Count() == prefab.InitializeSize)
                {
                    // activeじゃないオブジェクトが無い場合は新規追加
                    Debug.LogWarning($"It is necessary to increase InitializeSize : prefab = {prefab.name} InitializeSize = {prefab.InitializeSize}");
                }

                var obj = CreateObject(prefab);
                obj.name = $"{prefab.name}_{objects.Count + 1}";
                objects.Add(obj);
                return obj;
            }

            // activeじゃないオブジェクトを取得
            return objects.FirstOrDefault(_ => !_.IsActive) as T;
        }

        private bool InitPool<T>(string key, T prefab, Transform parent = null) where T : PooledMonobehaviour
        {
            var list = new List<PooledMonobehaviour>();

            for (int i = 0; i < prefab.InitializeSize; i++)
            {
                //var obj = Instantiate(prefab) as PooledMonobehaviour;
                var obj = CreateObject(prefab);
                obj.name = $"{prefab.name}_{i + 1}";

                if (parent != null)
                {
                    obj.transform.SetParent(parent, false);
                }

                list.Add(obj);
            }

            _pools.Add(key, list);
            return true;
        }

        private T CreateObject<T>(T prefab) where T : PooledMonobehaviour
        {
            var obj = Instantiate(prefab) as PooledMonobehaviour;
            obj.SetActive(false);
            return obj as T;
        }

        public void RemoveFromPool<T>(string key, T obj) where T : PooledMonobehaviour
        {
            if (_pools.SafeTryGetValue(key, out List<PooledMonobehaviour> list))
            {
                if (!list.Contains(obj))
                {
                    return;
                }

                list.Remove(obj);
            }
        }

    }
}
