using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kosu.UnityLibrary
{
    public class CreateUIElement
    {

        private static readonly string TEMPLATE_PATH = "DebugUITemplate";

        private static readonly string TEXT_PATH = "DebugUIText";

        private static readonly string FPS_TEXT_PATH = "StatsFPSText";

        private static readonly string MEMORY_TEXT_PATH = "StatsMemoryText";

        private static readonly string BUTTON_PATH = "DebugUIButton";

        private static readonly string SLIDER_PATH = "DebugUISlider";

        private static readonly string INPUTFIELD_PATH = "DebugUIInputField";

        private static readonly string TOGGLE_PATH = "DebugUIToggle";

        private static readonly string DROPDOWN_PATH = "DebugUIDropdown";

        [MenuItem("GameObject/DebugUI/Template", priority = 10)]
        public static void CreateTemplate(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(TEMPLATE_PATH)) as GameObject;
            PrefabUtility.UnpackPrefabInstance(g, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Canvas");
            g.transform.SetAsLastSibling();
            var eventSystem = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

            if (eventSystem == null)
            {
                var eg = new GameObject("EventSystem");
                eg.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eg.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        [MenuItem("GameObject/DebugUI/Text", priority = 10)]
        public static void CreateText(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(TEXT_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Text");
        }

        [MenuItem("GameObject/DebugUI/Button", priority = 10)]
        public static void CreateButton(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(BUTTON_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Button");
        }

        [MenuItem("GameObject/DebugUI/Slider", priority = 10)]
        public static void CreateSlider(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(SLIDER_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Slider");
        }

        [MenuItem("GameObject/DebugUI/InputField", priority = 10)]
        public static void CreateInputField(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(INPUTFIELD_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI InputField");
        }

        [MenuItem("GameObject/DebugUI/Toggle", priority = 10)]
        public static void CreateToggle(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(TOGGLE_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Toggle");
        }

        [MenuItem("GameObject/DebugUI/Dropdown", priority = 10)]
        public static void CreateDropdown(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(DROPDOWN_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "DebugUI Dropdown");
        }

        [MenuItem("GameObject/DebugUI/Stats/FPS Text", priority = 10)]
        public static void CreateFPSText(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(FPS_TEXT_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "StatsFPS Text");
        }

        [MenuItem("GameObject/DebugUI/Stats/Memory Text", priority = 10)]
        public static void CreateMemoryText(MenuCommand menuCommand)
        {
            var g = PrefabUtility.InstantiatePrefab(Resources.Load(MEMORY_TEXT_PATH)) as GameObject;
            OnCreatedObject(g, menuCommand.context as GameObject, "StatsMemory Text");
        }

        private static void OnCreatedObject(GameObject self, GameObject parent, string selfName = "DebugUIComponent")
        {
            self.name = selfName;
            GameObjectUtility.SetParentAndAlign(self, parent);
            Undo.RegisterCreatedObjectUndo(self, "Create " + self.name);
            Selection.activeObject = self;
        }

    }
}
