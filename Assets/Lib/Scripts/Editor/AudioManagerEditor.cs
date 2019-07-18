using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kosu.UnityLibrary
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {

        private AudioManager _target;

        private string _clipName;

        private void Awake()
        {
            _target = (AudioManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _clipName = EditorGUILayout.TextField("clip name", _clipName);

            if (GUILayout.Button("Play SE"))
            {
                _target.PlaySE(_clipName);
            }

            if (GUILayout.Button("Play BGM"))
            {
                _target.PlayBGM(_clipName);
            }
        }

    }
}
