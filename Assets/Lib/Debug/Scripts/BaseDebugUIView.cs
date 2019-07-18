using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Kosu.UnityLibrary
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseDebugUIView : MonoBehaviour
    {

        protected List<System.IDisposable> _stream = new List<System.IDisposable>();

        private bool _isShowed;

        private CanvasGroup _rootCanvasGroup;

        private System.IDisposable _visibleStream;

        private void Awake()
        {
            AfterAwake();
            _rootCanvasGroup = GetComponent<CanvasGroup>();
            _rootCanvasGroup.alpha = 0;
            _rootCanvasGroup.interactable = false;
            _rootCanvasGroup.blocksRaycasts = false;
            _visibleStream = UniRxUtils.ObserveInputKeyDown(KeyCode.F1, () =>
            {
                if (_isShowed)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }, gameObject);
        }

        private void OnEnable()
        {
            Bind();
        }

        private void OnDisable()
        {
            UnBind();
        }

        protected virtual void AfterAwake()
        {
        }

        protected virtual void Bind()
        {
        }

        protected virtual void UnBind()
        {
            foreach (var stream in _stream)
            {
                stream.Dispose();
            }

            _stream.Clear();
        }

        private void Show()
        {
            if (DOTween.IsTweening(_rootCanvasGroup))
            {
                DOTween.Kill(_rootCanvasGroup);
            }

            gameObject.SetActive(true);
            _rootCanvasGroup.DOFade(1f, 0.1f).OnComplete(() =>
            {
                _rootCanvasGroup.interactable = true;
                _rootCanvasGroup.blocksRaycasts = true;
            });
            _isShowed = true;
        }

        private void Hide()
        {
            if (DOTween.IsTweening(_rootCanvasGroup))
            {
                DOTween.Kill(_rootCanvasGroup);
            }

            _rootCanvasGroup.interactable = false;
            _rootCanvasGroup.blocksRaycasts = false;
            _rootCanvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
            _isShowed = false;
        }

    }
}
