using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Kosu.UnityLibrary
{
    public class LogSetting
    {
        public string beforeSaveFileName;
        public string localPath;
        public int intervalMinutes;
        public string drivePath;
        public string filePrefix;
        public static readonly string PATH = "LogSetting.json";

        public LogSetting()
        {
            localPath = "C:\\METoA\\log\\";
            beforeSaveFileName = "";
            intervalMinutes = 60;
            drivePath = "1zn-XmIhHYP1STKzHGLNM6PBfl_FY3fjn";
            filePrefix = "";
        }
    }

    public class UnityLogger : MonoBehaviour
    {

        private StreamWriter _sw;

        private string _log;

        private static readonly string NEW_LINE = System.Environment.NewLine;

        private string _fileName;

        private LogSetting _setting;

        private float _elapsedTime;

        private void OnEnable()
        {
            Application.logMessageReceived += LogCallbackHandler;
            _setting = DataUtils.LoadDataFromJson<LogSetting>(LogSetting.PATH);

            if (_setting.beforeSaveFileName != null && !string.IsNullOrEmpty(_setting.beforeSaveFileName))
            {
                UploadToGoogleDrive(_setting.beforeSaveFileName);
            }

            Init();
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= LogCallbackHandler;

            if (_sw != null)
            {
                _sw.Write(NEW_LINE);
                _sw.Close();
                _sw = null;
            }

#if !UNITY_EDITOR
            _setting.beforeSaveFileName = _fileName;
#endif
            DataUtils.SaveDataToJson(_setting, LogSetting.PATH);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime / 60f > _setting.intervalMinutes)
            {
                UploadToGoogleDrive(_fileName);
                _elapsedTime = 0;
            }
        }

        private void LogCallbackHandler(string logString, string stackTrace, LogType type)
        {
            if (_sw == null)
            {
                return;
            }

            var log = string.Format("[{0}][{1}] {2}" + NEW_LINE + "{3}" + NEW_LINE,
                                    type.ToString(),
                                    System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    logString,
                                    stackTrace);
            _sw.Write(log);
        }

        private void Init()
        {
            if (_sw != null)
            {
                return;
            }

            _fileName = _setting.filePrefix + System.DateTime.Now.ToString("yyyy.MM.dd") + "_log" + ".txt";
#if !UNITY_EDITOR

            if (!Directory.Exists(_setting.localPath))
            {
                Directory.CreateDirectory(_setting.localPath);
            }

            if (!File.Exists(_setting.localPath + _fileName))
            {
                var fs = File.Create(_setting.localPath + _fileName);
                fs.Close();
            }

            _sw = new StreamWriter(_setting.localPath + _fileName, true);
            _sw.AutoFlush = true;
#endif
        }

        private void UploadToGoogleDrive(string fileName)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return;
            }

#if !UNITY_EDITOR

            if (_sw != null)
            {
                _sw.Close();
                _sw = null;
            }

            var loadPath = _setting.localPath + fileName;
            StreamReader sr = new StreamReader(loadPath);
            var file = new UnityGoogleDrive.Data.File() { Name = fileName, Content = System.Text.Encoding.UTF8.GetBytes(sr.ReadToEnd()) };
            sr.Close();
            UnityGoogleDrive.GoogleDriveFiles.List().Send().OnDone += fileList =>
            {
                var f = fileList.Files.FirstOrDefault(_ => _.Name == fileName);

                if (f == null)
                {
                    file.Parents = new List<string>() { _setting.drivePath };
                    // nothing
                    UnityGoogleDrive.GoogleDriveFiles.Create(file).Send().OnDone += (_) =>
                    {
                        Debug.Log("upload create done");
                    };
                }
                else
                {
                    // overwrite
                    UnityGoogleDrive.GoogleDriveFiles.Update(f.Id, file).Send().OnDone += (_) =>
                    {
                        Debug.Log("upload update done");
                    };
                }
            };
            Init();
#endif
        }

    }
}
