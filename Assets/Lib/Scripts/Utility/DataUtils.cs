using UnityEngine;
using System.IO;

namespace Kosu.UnityLibrary
{
    public static class DataUtils
    {
        public static void SaveDataToJson<T>(T obj, string savePath)
        {
            string dirPath = Path.GetDirectoryName(savePath);

            if (dirPath.IsNotNullOrEmpty() && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string json = JsonUtility.ToJson(obj);
            FileStream fs = File.Create(savePath);
            var utf8_encoding = new System.Text.UTF8Encoding(false);
            StreamWriter sw = new StreamWriter(fs, utf8_encoding);
            sw.WriteLine(json);
            sw.Close();
            fs.Close();
        }

        public static T LoadDataFromJson<T>(string loadPath) where T : new ()
        {
            if (!File.Exists(loadPath))
            {
                return new T();
            }

            FileStream fs = File.Open(loadPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            var json = JsonUtility.FromJson<T>(sr.ReadToEnd());
            sr.Close();
            fs.Close();
            return json;
        }

        public static string ReadFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            string result = null;

            try
            {
                using (StreamReader sr = new StreamReader(fi.OpenRead(), System.Text.Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }
    }
}
