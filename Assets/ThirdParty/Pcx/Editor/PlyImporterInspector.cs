// Pcx - Point cloud importer & renderer for Unity
// https://github.com/keijiro/Pcx

using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace Pcx
{
    // Note: Not sure why but EnumPopup doesn't work in ScriptedImporterEditor,
    // so it has been replaced with a normal Popup control.

    [CustomEditor(typeof(PlyImporter))]
    class PlyImporterInspector : ScriptedImporterEditor
    {
        SerializedProperty _containerType;

        SerializedProperty _offset;

        string[] _containerTypeNames;

        protected override bool useAssetDrawPreview { get { return false; } }

        public override void OnEnable()
        {
            base.OnEnable();

            _containerType = serializedObject.FindProperty("_containerType");
            _containerTypeNames = System.Enum.GetNames(typeof(PlyImporter.ContainerType));
            _offset = serializedObject.FindProperty("_offset");
        }

        public override void OnInspectorGUI()
        {
            _containerType.intValue = EditorGUILayout.Popup(
                "Container Type", _containerType.intValue, _containerTypeNames);

            _offset.vector3Value = EditorGUILayout.Vector3Field("offset", _offset.vector3Value);

            base.ApplyRevertGUI();
        }
    }
}
