using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    public class DebugUISlider : MonoBehaviour
    {

        private Slider _slider;

        private InputField _inputField;

        [SerializeField]
        private Text _valueText = null;

        [SerializeField]
        private string _label = "Label";

        [SerializeField]
        private Text _labelText = null;

        [SerializeField]
        private float _min = 0, _max = 1, _defaultValue = 0;

        public System.Action<float> onValueChanged;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _inputField = GetComponentInChildren<InputField>();
            _labelText.text = _label;
            _slider.minValue = _min;
            _slider.maxValue = _max;
            _slider.value = _defaultValue;
        }

        private void OnEnable()
        {
            _inputField.onEndEdit.AddListener(OnEndInputFieldEdit);
            _slider.onValueChanged.AddListener(OnChangeSliderValue);
        }

        private void OnDisable()
        {
            _inputField.onEndEdit.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
        }

        private void OnEndInputFieldEdit(string text)
        {
            if (float.TryParse(text, out float res))
            {
                _slider.value = res;
            }
        }

        private void OnChangeSliderValue(float val)
        {
            _valueText.text = val.ToString("F2");
            onValueChanged.SafeInvoke(val);
        }

    }
}
