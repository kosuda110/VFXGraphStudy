using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    public class LogPanel : MonoBehaviour
    {

        [SerializeField]
        private RectTransform _logStringParent = null;

        [SerializeField]
        private int _maxQueueSize = 100;

        [SerializeField]
        private ScrollRect _scrollRect = null;

        private int _textCount = 0;

        [SerializeField]
        private Toggle _autoScrollToggle = null;

        private Queue<string> _logStrings = new Queue<string>();

        private Queue<string> _warningStrings = new Queue<string>();

        private Queue<string> _errorStrings = new Queue<string>();

        private List<Transform> _visibleLogTexts = new List<Transform>();

        private List<Transform> _visibleWarningTexts = new List<Transform>();

        private List<Transform> _visibleErrorTexts = new List<Transform>();

        [SerializeField]
        private Toggle _logToggle = null;

        [SerializeField]
        private Toggle _warningToggle = null;

        [SerializeField]
        private Toggle _errorToggle = null;

        private void Awake()
        {
            _autoScrollToggle.isOn = true;
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += LogCallbackHandler;
            _logToggle.onValueChanged.AddListener((isOn) =>
            {
                _visibleLogTexts.ForEach(_ => _.gameObject.SetActive(isOn));
            });
            _warningToggle.onValueChanged.AddListener((isOn) =>
            {
                _visibleWarningTexts.ForEach(_ => _.gameObject.SetActive(isOn));
            });
            _errorToggle.onValueChanged.AddListener((isOn) =>
            {
                _visibleErrorTexts.ForEach(_ => _.gameObject.SetActive(isOn));
            });
        }

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= LogCallbackHandler;
            _logToggle.onValueChanged.RemoveAllListeners();
            _warningToggle.onValueChanged.RemoveAllListeners();
            _errorToggle.onValueChanged.RemoveAllListeners();
        }

        private void Update()
        {
            if (_autoScrollToggle.isOn)
            {
                _scrollRect.verticalNormalizedPosition = 0;
            }

            while (_logStrings.Count > 0)
            {
                var text = CreateLogText(_logStrings.Dequeue());
                text.gameObject.SetActive(_logToggle.isOn);
                _visibleLogTexts.Add(text);
            }

            while (_warningStrings.Count > 0)
            {
                var text = CreateLogText(_warningStrings.Dequeue());
                text.gameObject.SetActive(_warningToggle.isOn);
                _visibleWarningTexts.Add(text);
            }

            while (_errorStrings.Count > 0)
            {
                var text = CreateLogText(_errorStrings.Dequeue());
                text.gameObject.SetActive(_errorToggle.isOn);
                _visibleErrorTexts.Add(text);
            }

            _textCount = _logStringParent.childCount;

            while (_textCount > _maxQueueSize)
            {
                var child = _logStringParent.GetChild(0);
                DestroyImmediate(child.gameObject);
                _textCount = _logStringParent.childCount;

                if (_visibleLogTexts.Contains(child))
                {
                    _visibleLogTexts.Remove(child);
                }

                if (_visibleWarningTexts.Contains(child))
                {
                    _visibleWarningTexts.Remove(child);
                }

                if (_visibleErrorTexts.Contains(child))
                {
                    _visibleErrorTexts.Remove(child);
                }
            }
        }

        private void LogCallbackHandler(string logString, string stackTrace, LogType type)
        {
            string log = "";

            switch (type)
            {
                case LogType.Log:
                    log = "<color=\"#00FF11\">";
                    break;

                case LogType.Warning:
                    log = "<color=\"#FFF000\">";
                    break;

                case LogType.Error:
                case LogType.Exception:
                    log = "<color=\"#FF0500\">";
                    break;
            }

            log += string.Format("[{0}][{1}] {2}</color>",
                                 type.ToString(),
                                 System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                 logString);

            switch (type)
            {
                case LogType.Log:
                    _logStrings.Enqueue(log);
                    break;

                case LogType.Warning:
                    _warningStrings.Enqueue(log);
                    break;

                case LogType.Error:
                case LogType.Exception:
                    _errorStrings.Enqueue(log);
                    break;
            }
        }

        private Transform CreateLogText(string log)
        {
            var text = Instantiate(Resources.Load<Text>("LogText"));
            text.text = log;
            text.transform.SetParent(_logStringParent, false);
            return text.transform;
        }

    }
}
