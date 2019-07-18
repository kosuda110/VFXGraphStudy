using UnityEngine;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// ImageEffect実装のベースクラス
    /// </summary>
    public class MyImageEffectBase : MonoBehaviour
    {
        [SerializeField]
        private Shader _shader = null;

        [SerializeField]
        private Material _mat;

        protected string _nowPattern;

        protected Material Mat
        {
            get
            {
                if (_mat == null)
                {
                    _mat = new Material(_shader);
                    _mat.hideFlags = HideFlags.HideAndDontSave;
                }

                return _mat;
            }
        }

        protected virtual void Start()
        {
            if (SystemInfo.supportsImageEffects == false ||
                _shader == null ||
                _shader.isSupported == false)
            {
                enabled = false;
            }
        }

        public void ChangeShader(System.Enum pattern)
        {
            if (string.IsNullOrEmpty(_nowPattern) == false)
            {
                Mat.DisableKeyword(_nowPattern);
            }

            _nowPattern = pattern.ToString();
            Mat.EnableKeyword(_nowPattern);
        }
    }
}
