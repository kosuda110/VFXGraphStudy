using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class FollowTarget : MonoBehaviour
    {

        [SerializeField]
        private Transform _followTargetTR;

        private Transform _selfTR;

        public void Setup(Transform targetTR)
        {
            _followTargetTR = targetTR;
            _selfTR = transform;
        }

        private void Update()
        {
            if (_followTargetTR == null || !_followTargetTR.gameObject.activeSelf)
            {
                return;
            }

            _selfTR.position = _followTargetTR.position;
        }

    }
}
