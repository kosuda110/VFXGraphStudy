using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Kosu.UnityLibrary
{
    public class SaveRenderTextureWindow : EditorWindow
    {

        private static int _width;

        private static int _height;

        private static string _fileName = "test.png";

        private static string _outputPath = "C:\\Users\\kosuda_tatsuya\\Desktop\\";

        [MenuItem("Tools/SaveRenderTexture")]
        private static void Open()
        {
            GetWindow<SaveRenderTextureWindow>();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _width = EditorGUILayout.IntField("width : ", _width);
                _height = EditorGUILayout.IntField("height : ", _height);
            }
            EditorGUILayout.EndHorizontal();
            _fileName = EditorGUILayout.TextField("FileName : ", _fileName);
            _outputPath = EditorGUILayout.TextField("OutPutPath : ", _outputPath);

            if (GUILayout.Button("SAVE"))
            {
                Save();
            }

            if (GUILayout.Button("METoA"))
            {
                SaveForMEToA();
            }
        }

        private static void Save()
        {
            var cams = GetPerspectiveCameras();
            Texture2D sc = new Texture2D(_width, _height, TextureFormat.RGB24, false);

            // TODO : viewportrectの反映
            for (int i = 0; i < cams.Length; i++)
            {
                Texture2D tmp = new Texture2D(_width / cams.Length, _height, TextureFormat.RGB24, false);
                var rt = new RenderTexture(tmp.width, tmp.height, 24);
                rt.antiAliasing = 8;
                cams[i].targetTexture = rt;
                cams[i].Render();
                RenderTexture.active = rt;
                tmp.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                cams[i].targetTexture = null;
                RenderTexture.active = null;
                GameObject.DestroyImmediate(rt);
                sc.SetPixels(tmp.width * i, 0, tmp.width, tmp.height, tmp.GetPixels());
            }

            var bytes = sc.EncodeToPNG();
            System.IO.File.WriteAllBytes(_outputPath + _fileName + ".png", bytes);
        }

        private static void SaveForMEToA()
        {
            Texture2D sc = new Texture2D(3840 * 2, 2160, TextureFormat.RGBA32, false);
            {
                var pCam = GameObject.FindObjectsOfType<Camera>().Where(_ => _.name == "Display2 L").ToArray();
                var oCam = GameObject.Find("UI Camera 1").GetComponent<Camera>();
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                var tmp = new Texture2D(1920, 2160, TextureFormat.RGBA32, false);
                var rt = new RenderTexture(1920, 2160, 24);
                rt.antiAliasing = 1;
                pCam[1].targetTexture = rt;
                pCam[0].targetTexture = rt;
                oCam.targetTexture = rt;
                pCam[1].Render();
                oCam.Render();
                pCam[0].Render();
                RenderTexture.active = rt;
                tmp.ReadPixels(new Rect(0, 0, 1920, 2160), 0, 0);
                pCam[1].targetTexture = null;
                pCam[0].targetTexture = null;
                oCam.targetTexture = null;
                RenderTexture.active = null;
                DestroyImmediate(rt);
                sc.SetPixels(0, 0, 1920, tmp.height, tmp.GetPixels());
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
            }
            {
                var pCam = GameObject.FindObjectsOfType<Camera>().Where(_ => _.name == "Display2 R").ToArray();
                var oCam = GameObject.Find("UI Camera 5").GetComponent<Camera>();
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                var tmp = new Texture2D(1920, 2160, TextureFormat.RGBA32, false);
                var rt = new RenderTexture(1920, 2160, 24);
                rt.antiAliasing = 1;
                pCam[0].targetTexture = rt;
                pCam[1].targetTexture = rt;
                oCam.targetTexture = rt;
                pCam[1].Render();
                oCam.Render();
                pCam[0].Render();
                RenderTexture.active = rt;
                tmp.ReadPixels(new Rect(0, 0, 1920, 2160), 0, 0);
                pCam[0].targetTexture = null;
                pCam[1].targetTexture = null;
                oCam.targetTexture = null;
                RenderTexture.active = null;
                DestroyImmediate(rt);
                sc.SetPixels(1920, 0, 1920, tmp.height, tmp.GetPixels());
                pCam[0].rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
                pCam[1].rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
                oCam.rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
            }
            {
                var pCam = GameObject.FindObjectsOfType<Camera>().Where(_ => _.name == "Display3 L").ToArray();
                var oCam = GameObject.Find("UI Camera 2").GetComponent<Camera>();
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                var tmp = new Texture2D(1920, 2160, TextureFormat.RGBA32, false);
                var rt = new RenderTexture(1920, 2160, 24);
                rt.antiAliasing = 1;
                pCam[0].targetTexture = rt;
                pCam[1].targetTexture = rt;
                oCam.targetTexture = rt;
                pCam[1].Render();
                oCam.Render();
                pCam[0].Render();
                RenderTexture.active = rt;
                tmp.ReadPixels(new Rect(0, 0, 1920, 2160), 0, 0);
                pCam[0].targetTexture = null;
                pCam[1].targetTexture = null;
                oCam.targetTexture = null;
                RenderTexture.active = null;
                DestroyImmediate(rt);
                sc.SetPixels(3840, 0, 1920, tmp.height, tmp.GetPixels());
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
            }
            {
                var pCam = GameObject.FindObjectsOfType<Camera>().Where(_ => _.name == "Display3 R").ToArray();
                var oCam = GameObject.Find("UI Camera 6").GetComponent<Camera>();
                pCam[0].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                pCam[1].rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                oCam.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                var tmp = new Texture2D(1920, 2160, TextureFormat.RGBA32, false);
                var rt = new RenderTexture(1920, 2160, 24);
                rt.antiAliasing = 1;
                pCam[0].targetTexture = rt;
                pCam[1].targetTexture = rt;
                oCam.targetTexture = rt;
                pCam[1].Render();
                oCam.Render();
                pCam[0].Render();
                RenderTexture.active = rt;
                tmp.ReadPixels(new Rect(0, 0, 1920, 2160), 0, 0);
                pCam[0].targetTexture = null;
                pCam[1].targetTexture = null;
                oCam.targetTexture = null;
                RenderTexture.active = null;
                DestroyImmediate(rt);
                sc.SetPixels(1920 + 3840, 0, 1920, tmp.height, tmp.GetPixels());
                pCam[0].rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
                pCam[1].rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
                oCam.rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
            }
            var bytes = sc.EncodeToPNG();
            System.IO.File.WriteAllBytes(_outputPath + _fileName + ".png", bytes);
        }

        private static Camera[] GetPerspectiveCameras()
        {
            return GameObject.FindObjectsOfType<Camera>().Where(x => x.name == "ScreenShotCamera").ToArray();
        }

    }
}
