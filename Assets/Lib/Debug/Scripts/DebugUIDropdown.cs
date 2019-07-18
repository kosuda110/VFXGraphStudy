using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    public class DebugUIDropdown : MonoBehaviour
    {

        [SerializeField]
        private string _label = "Label";

        [SerializeField]
        private Text _labelText = null;

        private Dropdown _dropDown;

        public System.Action<int> onValueChanged;

        private void Awake()
        {
            _dropDown = GetComponentInChildren<Dropdown>();
            _labelText.text = _label;
        }

        private void OnEnable()
        {
            _dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDisable()
        {
            _dropDown.onValueChanged.RemoveAllListeners();
        }

        private void OnDropdownValueChanged(int val)
        {
            onValueChanged.SafeInvoke(val);
        }

        public void SetLabel(string label)
        {
            _label = label;
            _labelText.text = _label;
        }

        public Dropdown.OptionData GetOptionData(int index)
        {
            return _dropDown.options[Mathf.Clamp(index, 0, _dropDown.options.Count - 1)];
        }

    }
}
