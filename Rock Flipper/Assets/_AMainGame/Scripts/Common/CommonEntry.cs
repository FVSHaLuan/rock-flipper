using BT.F2P;
using BT.UI;
using BT.UI.ToolTips;
using UnityEngine;

namespace BT
{
    public class CommonEntry : MonoBehaviourWithInit
    {
        private static CommonEntry commonInstance;

        public static CommonEntry CommonInstance
        {
            get => commonInstance;
            protected set
            {
                ///
                value?.OnSetAsInstance();

                ///
                commonInstance = value;

                ///
                Entry.Instance.GeneralDialog = commonInstance.generalDialog;
                Entry.Instance.SettingPopup = commonInstance.SettingPopup;
            }
        }

        [Header("Common Entry")]
        [SerializeField]
        private Transform pooledObjectsRoot;
        [SerializeField]
        private GeneralDialog generalDialog;
        public FeedbackPopup feedbackPopup;
        [SerializeField]
        private UIScreen settingPopup;
        public UIScreen mainUIScreen;
        public TightScreenDetector tightScreenDetector;
        public UsePremiumFeaturePopup usePremiumFeaturePopup;
        public ToolTipManager toolTipManager;

        private EntryGeneralPool generalPool;

        public EntryGeneralPool GeneralPool
        {
            get
            {
                ///
                if (generalPool == null)
                {
                    generalPool = new EntryGeneralPool(pooledObjectsRoot);
                }

                ///
                return generalPool;
            }
        }

        public UIScreen SettingPopup { get => settingPopup; }

        protected virtual void OnSetAsInstance() { }
    }

}