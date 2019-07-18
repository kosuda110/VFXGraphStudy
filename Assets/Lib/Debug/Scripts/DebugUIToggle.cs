using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    [RequireComponent(typeof(Toggle))]
    public class DebugUIToggle : MonoBehaviour
    {
        [SerializeField]
        private string _label = "Label";

        [SerializeField]
        private Text _labelText = null;

        [SerializeField]
        private bool _defaultValue = false;

        private Toggle _toggle;

        public System.Action<bool> onValueChanged;

        private void Awake()
        {
            _labelText.text = _label;
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = _defaultValue;
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveAllListeners();
        }

        private void OnToggleValueChanged(bool isOn)
        {
            onValueChanged.SafeInvoke(isOn);
        }

        public void SetLabel(string label)
        {
            _label = label;
            _labelText.text = _label;
        }

    }
}
