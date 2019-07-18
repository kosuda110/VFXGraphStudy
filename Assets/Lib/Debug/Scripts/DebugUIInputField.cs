using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    public class DebugUIInputField : MonoBehaviour
    {

        [SerializeField]
        private string _label = "Label";

        [SerializeField]
        private Text _labelText = null;

        private InputField _inputField;

        public System.Action<string> onEndEdit;

        private void Awake()
        {
            _labelText.text = _label;
            _inputField = GetComponentInChildren<InputField>();
        }

        private void OnEnable()
        {
            _inputField.onEndEdit.AddListener(OnEndInputFieldEdit);
        }

        private void OnDisable()
        {
            _inputField.onEndEdit.RemoveAllListeners();
        }

        private void OnEndInputFieldEdit(string text)
        {
            onEndEdit.SafeInvoke(text);
        }

        public void SetLabel(string label)
        {
            _label = label;
            _labelText.text = _label;
        }

    }
}
