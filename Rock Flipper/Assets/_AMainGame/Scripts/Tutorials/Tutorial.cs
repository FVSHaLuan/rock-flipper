using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BT.Tutorials
{
    public class Tutorial : ScriptableObjectWithInit
    {
        [SerializeField]
        private string tutorialId;

        public string TutorialId => tutorialId;

        public bool HasPassed => Entry.Instance.GameSetting.HasTutorialPassed(TutorialId);

        public void SetAsPassed()
        {
            Entry.Instance.GameSetting.SetTutorialAsPassed(TutorialId);
            Entry.Instance.gameSettingObject.SaveData();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(tutorialId), "tutorialId missing!");
        }

        [ContextMenu("Editor_LogHasPassed"), PlayModeOnly]
        private void Editor_LogHasPassed()
        {
            Debug.Log(HasPassed);
        }
#endif
    }

}