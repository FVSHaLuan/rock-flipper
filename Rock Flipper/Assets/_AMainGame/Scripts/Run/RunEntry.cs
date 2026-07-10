using Agame.Run.Combat;
using Agame.Run.Stats;
using UnityEngine;
using UnityEngine.UI;
using Agame.Run.Combat.Tutorials;
using UnityEngine.InputSystem;

namespace Agame.Run
{
    public class RunEntry : CommonEntry
    {
        private const string Editor_TestBuildStatsPath = "Assets/_Exp/BuildStatsTest.asset";

        private static BuildStatsObject buildStatsFromLastInstance;

        public static RunEntry Instance
        {
            get => CommonInstance as RunEntry;
        }

        [Header("RunEntry")]
        public RunStateManager runStateManager;
        [SerializeField]
        private BuildStatsObject buildStatsDefault;
        [SerializeField]
        private BuildStatsObject buildStatsTutorial;
        public ShortHandManager shortHandManager;
        public CombatSoundEffectManager combatSoundEffectManager;

        [Header("Combat")]
        public Camera gameplayCamera;
        public CameraShake cameraShake;
        public Playfield playfield;
        public Selectable combatInvisibleButton;
        public TutorialFlags tutorialFlags;
        public UIScreen combatScreen;

        private bool isUsingTestBuildStats;
        private string baseBuildStatsName = "";
        private BuildStatsObject editor_TestBuildStats;
        private BuildStatsObject buildStats;
        private BuildStatsObject baseBuildStats;
        private RunData runData;

        public RunData RunData => runData;
        public BuildStatsObject BuildStats
        {
            get
            {
                ///
                if (buildStats == null)
                {
                    buildStats = Instantiate(BaseBuildStats);
                }

                ///
                return buildStats;
            }
        }
        public BuildStatsObject BaseBuildStats
        {
            get
            {
                ///
                TryInitBaseBuildStats();

                ///
                return baseBuildStats;
            }
        }
        public string BaseBuildStatsName
        {
            get
            {
                TryInitBaseBuildStats();
                return baseBuildStatsName;
            }
        }
        public bool RebuildStatsAfterPrestigeFlag { get; private set; }

        public bool IsBuildStatsInvalid { get; private set; }
        public bool IsUsingTestBuildStats
        {
            get
            {
                TryInitBaseBuildStats();
                return isUsingTestBuildStats;
            }
        }

        protected override bool Init()
        {
            ///
#if UNITY_EDITOR
            var testBuildStats = UnityEditor.AssetDatabase.LoadAssetAtPath<TestBuildStats>(Editor_TestBuildStatsPath);
            if (testBuildStats != null && testBuildStats.Enabled)
            {
                editor_TestBuildStats = testBuildStats.BuildStats;
            }
            else
            {
                editor_TestBuildStats = null;
            }
#endif

            ///
            InitRunData();

            ///
            return base.Init();
        }

        private void InitRunData()
        {
            runData = Entry.Instance.runDataManager.ActiveRunDataObject.Data;

            ///
            if (!runData.InitedRun)
            {

                ///
                runData.InitRun(Entry.Instance.runDataManager.ActiveRunDataIndex);

                ///
                Debug.LogWarningFormat("Inited runData slot {0}", Entry.Instance.runDataManager.ActiveRunDataIndex);
            }

            ///
            runData.StartRun();

            ///
            Entry.Instance.PlayerData.LastSlotId = runData.SlotId;
            Entry.Instance.playerDataSaver.SetSaveThisFrame();
        }

        protected override void ExtendedAwake()
        {
            ///
            if (buildStatsFromLastInstance != null)
            {
                DestroyBuildStats(buildStatsFromLastInstance);
            }

            ///
            CommonInstance = this;
            CommonEntry.CommonInstance = this;
            Entry.ActiveGameScene = GameScene.Run;

            ///
            ApplyAllToBuildStats();

            ///
            playfield.TryInit();

            ///
            base.ExtendedAwake();
        }

        protected void OnDestroy()
        {
            buildStatsFromLastInstance = buildStats;

            ///
            Entry.Instance.playerDataSaver.SaveNow();
        }

        public void MarkBuildStatsAsInvalid(bool isAfterPrestige)
        {
            IsBuildStatsInvalid = true;
            RebuildStatsAfterPrestigeFlag = true;
        }

        public void RebuildBuildStats()
        {
            IsBuildStatsInvalid = false;

            ///
            if (buildStats != null)
            {
                DestroyBuildStats(buildStats);
            }
            buildStats = null;

            ///
            ApplyAllToBuildStats();

            ///
            RebuildStatsAfterPrestigeFlag = false;
        }

        public void FinishTutorial()
        {
            runData.FinishedTutorial = true;
            Entry.Instance.visualSceneLoader.Load(GameScene.Run);
            Entry.Instance.playerDataSaver.SetSaveThisFrame();
        }

        private void DestroyBuildStats(BuildStatsObject buildStats)
        {
            Destroy(buildStats.gameObject);
        }

        private void TryInitBaseBuildStats()
        {
            if (baseBuildStats == null)
            {
                ///
                TryInit();
#if UNITY_EDITOR
                ///
                if (editor_TestBuildStats != null)
                {
                    baseBuildStats = Instantiate(editor_TestBuildStats);
                    baseBuildStatsName = editor_TestBuildStats.name;
                    isUsingTestBuildStats = true;

                    ///
                    Debug.LogWarning($"Using test BuildStats (instantiated): {editor_TestBuildStats.name}", editor_TestBuildStats);
                }
                else
#endif
                {
                    baseBuildStats = RunData.FinishedTutorial ? Instantiate(buildStatsDefault) : Instantiate(buildStatsTutorial);
                    baseBuildStatsName = baseBuildStats.name;
                    isUsingTestBuildStats = false;
                }
            }
        }

        public Vector2 GetMouseWorldPosition()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            return gameplayCamera.ScreenToWorldPoint(mousePosition);
        }

        private void ApplyAllToBuildStats()
        {
            Debug.LogError("ApplyAllToBuildStats is not implemented yet. Please implement this method to apply necessary data to the BuildStatsObject.");
        }

        protected void LateUpdate()
        {
            RunData.FrameUpdateLate();
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_EditBuildStats"), PlayModeOnly]
        private void Editor_EditBuildStats()
        {
            UnityEditor.Selection.activeObject = buildStats;
        }
#endif
    }

}