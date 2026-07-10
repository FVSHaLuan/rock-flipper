using System.Text;
using UnityEngine;

namespace Agame.Run.Dev
{
    public class RunLogger : MonoBehaviour
    {
        private static RunLogger instance;

        private StringBuilder unflushedLog = new StringBuilder();
        private string filePath;

        protected void Awake()
        {
            instance = this;
        }

        private void _Log(string message)
        {
            ///
            Entry.Instance.playerDataSaver.SetSaveThisFrame();

            ///
            var timeString = TimeStringHelper.GetStringFromSeconds(RunEntry.Instance.RunData.PlayTimeNow);
            var logLine = $"{timeString} - {message}";
            unflushedLog.AppendLine(logLine);

            ///
            Debug.Log($"RunLogger: {logLine}");
        }

        private void _Clear()
        {
            ///
            filePath = string.IsNullOrWhiteSpace(filePath) ? GetFilePath() : filePath;

            ///
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        protected void LateUpdate()
        {
            ///
            if (unflushedLog.Length > 0)
            {
                ///
                Flush();

                ///
                unflushedLog.Clear();
            }
        }

        private void Flush()
        {
            ///
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = GetFilePath();
            }

            ///
            System.IO.File.AppendAllText(filePath, unflushedLog.ToString());
        }

        private string GetFilePath()
        {
            ///
            var slotId = Entry.Instance.runDataManager.ActiveRunDataIndex;

            ///
            return $"{Application.persistentDataPath}/RunLog_{slotId}.txt";
        }

        public static void Log(string message)
        {
#if UNITY_EDITOR
            ///
            if (instance == null && !FindInstance())
            {
                throw new System.Exception("RunLogger instance is null. Make sure a RunLogger component is present in the scene.");
            }

            ///
            instance._Log(message);
#endif
        }

        public static void Clear()
        {
#if UNITY_EDITOR
            ///
            if (instance == null && !FindInstance())
            {
                throw new System.Exception("RunLogger instance is null. Make sure a RunLogger component is present in the scene.");
            }

            ///
            instance._Clear();
#endif

        }

        private static bool FindInstance()
        {
            instance = FindFirstObjectByType<RunLogger>();
            return instance != null;
        }
    }
}