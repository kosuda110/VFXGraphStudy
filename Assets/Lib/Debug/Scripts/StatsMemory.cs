using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

namespace Kosu.UnityLibrary
{
    [RequireComponent(typeof(Text))]
    public class StatsMemory : MonoBehaviour
    {

        private Text _memoryText;

        private float _interval = 1;

        private float _time = 0;

        private void Awake()
        {
            _memoryText = GetComponent<Text>();
        }

        private void OnEnable()
        {
            UpdateText();
        }

        private void OnDisable()
        {
            _time = 0;
        }

        private void Update()
        {
            if (_time < _interval)
            {
                _time += Time.deltaTime;
                return;
            }

            _time = 0;
            UpdateText();
        }

        private void UpdateText()
        {
            _memoryText.text = $"GPUMem     : {(float)SystemInfo.graphicsMemorySize / 1024:F2} (GB)\n" +
                               $"System Mem : {(float)SystemInfo.systemMemorySize / 1024:F2} (GB)\n\n" +
                               $"GPUAllocatedMem        : {Profiler.GetAllocatedMemoryForGraphicsDriver() / 1048576} (MB)\n" +
                               $"TotalAllocatedMem      : {Profiler.GetTotalAllocatedMemoryLong() / 1048576} (MB)\n" +
                               $"TotalReservedMem       : {Profiler.GetTotalReservedMemoryLong() / 1048576} (MB)\n" +
                               $"TotalUnusedReservedMem : {Profiler.GetTotalUnusedReservedMemoryLong() / 1048576} (MB)";
        }

    }
}
