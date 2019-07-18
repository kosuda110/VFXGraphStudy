using UnityEngine;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// MonoBehaviour Singletonクラスを作りたい場合はこのクラスを継承する
    ///
    /// DontDestroyOnLoadなものにしたい場合は
    /// IsDontDestroyOnLoadプロパティおoverriceしてtrueを戻すようにする
    ///
    /// Awake時に実行したい処理がある場合はAfterAwakeをoverrideして実装する
    /// OnDesotry時に実行したい処理がある場合はAfterDestoryをoverrideして実装する
    ///
    /// public class TestSingleton : Singleton<TestSingleton>
    /// {
    ///    public override bool IsDontDestroyOnLoad
    ///    {
    ///         get { return true; }
    ///    }
    ///
    ///    protected override void AfterAwake()
    ///    {
    ///       :
    ///    }
    ///
    ///    protected override void AfterDestory()
    ///    {
    ///       :
    ///    }
    /// }
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        public virtual bool IsDontDestroyOnLoad
        {
            get { return false; }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;

                if (IsDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning(string.Format("Singleton {0} is already exists. new gameObject was destroyed.", typeof(T).Name));
            }

            AfterAwake();
        }

        /// <summary>
        /// Awakeで実行したい処理を実装する
        /// </summary>
        protected virtual void AfterAwake()
        {
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            AfterDestroy();
        }

        /// <summary>
        /// OnDestroyで実行したい処理を実装する
        /// </summary>
        protected virtual void AfterDestroy()
        {
        }
    }
}
