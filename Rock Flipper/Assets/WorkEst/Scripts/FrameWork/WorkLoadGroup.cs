using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorkEst
{
    [CreateAssetMenu(fileName = "WorkLoadGroup", menuName = "WorkEst/WorkLoadGroup")]
    public class WorkLoadGroup : WorkLoadBase
    {
        [SerializeField]
        private List<WorkLoadBase> childWorks;

        public List<WorkLoadBase> ChildWorks => childWorks;

        protected override float GetTime(HashSet<WorkLoadBase> calculatedWorks)
        {
            ///
            float time = 0;

            ///
            foreach (var item in childWorks)
            {
                item.CalculateTime(calculatedWorks);
                time += item.Time;
            }

            ///
            return time;
        }

#if UNITY_EDITOR
        [ContextMenu("PingChildWorksFolder")]
        private void PingChildWorksFolder()
        {
            ///
            var path = GetChildWorksFolderPath(this);

            ///
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.ImportAsset(path);
            }

            ///
            var o = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(o);
        }

        [ContextMenu("FillChildWorks")]
        public void FillChildWorks()
        {
            ///
            FillChildWorks(this);

            ///
            Debug.Log("Done filling");
        }

        private void FillChildWorks(WorkLoadGroup workLoadGroup)
        {
            ///
            workLoadGroup.SetLabel();

            ///
            var childWorksFolderPath = GetChildWorksFolderPath(workLoadGroup);

            ///
            if (!Directory.Exists(childWorksFolderPath))
            {
                ///
                Debug.LogErrorFormat(workLoadGroup, "ChildWork folder not found, WorkLoadGroup: {0}", workLoadGroup.name);

                ///
                workLoadGroup.childWorks = new List<WorkLoadBase>();
                EditorUtility.SetDirty(workLoadGroup);

                ///
                return;
            }

            ///
            var childWorks = GetWorkLoadsAt(childWorksFolderPath);

            ///
            workLoadGroup.childWorks = childWorks;

            ///
            EditorUtility.SetDirty(workLoadGroup);

            ///
            foreach (var item in childWorks)
            {
                if (item is WorkLoadGroup)
                {
                    (item as WorkLoadGroup).FillChildWorks();
                }
                else
                {
                    item.SetLabel();
                }
            }
        }

        private List<WorkLoadBase> GetWorkLoadsAt(string folderPath)
        {
            ///
            var files = Directory.GetFiles(folderPath, "*.asset");

            ///
            var workLoads = new List<WorkLoadBase>();

            ///
            foreach (var item in files)
            {
                var workLoad = AssetDatabase.LoadAssetAtPath<WorkLoadBase>(item);
                workLoads.Add(workLoad);
            }

            ///
            return workLoads;
        }

        private string GetChildWorksFolderPath(WorkLoadGroup workLoadGroup)
        {
            ///
            var assetPath = AssetDatabase.GetAssetPath(workLoadGroup);
            var assetFolderPath = Path.GetDirectoryName(assetPath);
            var folderName = Path.GetFileNameWithoutExtension(assetPath);

            ///
            return Path.Combine(assetFolderPath, folderName);
        }
#endif
    }
}