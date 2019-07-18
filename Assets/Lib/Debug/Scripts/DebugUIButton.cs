using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    [RequireComponent(typeof(Button))]
    public class DebugUIButton : MonoBehaviour
    {

        [SerializeField]
        private string _label = "Label";

        private Text _labelText;

        private Button _button;

        public System.Action onClick;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _labelText = GetComponentInChildren<Text>();
            _labelText.text = _label;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickDebugButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void OnClickDebugButton()
        {
            onClick.SafeInvoke();
        }

    }
}
