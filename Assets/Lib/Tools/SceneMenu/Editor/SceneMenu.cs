using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// Hierarchyにフォーカスしている状態でS+altを押すとSceneショートカットを開くScript
    /// </summary>
    [InitializeOnLoad]
    public static class SceneMenu
    {
        private static readonly string SEARCH_PATH = "Assets";

        static SceneMenu()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        //HierarchyのOnGUI
        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var mousePosition = Event.current.mousePosition;

            if ((Event.current.keyCode == KeyCode.S && Event.current.alt) &&
                (Event.current.command == false) &&
                (Selection.objects.Length == 0) &&
                (mousePosition.y > 0))
            {
                var width = 100;
                var height = 100;
                var position = new Rect(mousePosition.x, mousePosition.y - height, width, height);
                //表示用リスト
                List<GUIContent> contents = new List<GUIContent>();
                //SceneNameとpathのDictionary
                IDictionary<string, string> sceneDic = new Dictionary<string, string>();
                contents.Add(new GUIContent("==== Scenes In Build ===="));

                foreach (var s in EditorBuildSettings.scenes)
                {
                    string sceneName = Path.GetFileName(s.path).Replace(".unity", "");
                    sceneDic.Add(sceneName, s.path);
                    contents.Add(new GUIContent(sceneName));
                }

                contents.Add(new GUIContent(""));
                contents.Add(new GUIContent("==== Others ===="));
                List<string> guids = new List<string>();
                var settings = AssetDatabase.FindAssets("t:SceneMenuParameter", new string[] { "Assets" });

                if (settings.Length != 0)
                {
                    foreach (var setting in settings)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(setting);
                        var obj = AssetDatabase.LoadAssetAtPath<SceneMenuParameter>(path);
                        guids.SafeAddRange(AssetDatabase.FindAssets("t:Scene", obj.searchPath));
                    }
                }
                else
                {
                    guids.SafeAddRange(AssetDatabase.FindAssets("t:Scene", new string[] { SEARCH_PATH }));
                }

                var pathList = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).Distinct();
                var tmpOtherSceneNames = new List<string>();

                foreach (var path in pathList)
                {
                    string sceneName = Path.GetFileName(path).Replace(".unity", "");

                    if (sceneDic.ContainsKey(sceneName) == true)
                    {
                        continue;
                    }

                    sceneDic.Add(sceneName, path);
                    tmpOtherSceneNames.Add(sceneName);
                }

                tmpOtherSceneNames.Sort();

                foreach (var sceneName in tmpOtherSceneNames)
                {
                    contents.Add(new GUIContent(sceneName));
                }

                // コンテキストメニュー表示
                EditorUtility.DisplayCustomMenu(position, contents.ToArray(), -1, Callback, sceneDic);
            }
        }

        //Menuタップ時のコールバック
        private static void Callback(object userData, string[] options, int selected)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false)
            {
                return;
            }

            string path = null;
            var dic = userData as Dictionary<string, string>;
            dic.TryGetValue(options[selected], out path);

            if (!string.IsNullOrEmpty(path))
            {
                EditorSceneManager.OpenScene(path);
            }
        }
    }
}
