using Agame.F2P;
using Agame.UI;
using Agame.UI.ToolTips;
using UnityEngine;

namespace Agame
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
        [System.Obsolete]
        public Transform tooltipTransformParent;

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