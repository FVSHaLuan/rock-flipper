using FH.Core.Architecture.Pool;
using Agame.FeatureBranching;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run
{
    public class SkillNodeToolTip : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnifiedText descriptionText;
        [SerializeField]
        private UnifiedText extraDescriptionText;
        [SerializeField]
        private UnifiedText levelText;
        [SerializeField]
        private UnifiedText costText;

        [Space]
        [SerializeField]
        private Image costWrapperImage;
        [SerializeField]
        private CanvasGroup costCanvasGroup;
        [SerializeField]
        private float notEnoughAlpha = 0.3f;
        [SerializeField]
        private Color normalCostWrapperColor;
        [SerializeField]
        private Color notEnoughCostWrapperColor;
        [SerializeField]
        private Color maxCostWrapperColor;
        [SerializeField]
        private GameObject debugObject;
        [SerializeField]
        private UnifiedText debugText;
        [SerializeField]
        private GameObject demoNoticeObject;

        [Space]
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private Image backgroundIconImage;

        private RectTransform rectTransform;
        private SkillNode skillNode;

        protected void OnDisable()
        {
            UnlistenToSkillNode();
        }

        private void UnlistenToSkillNode()
        {
            ///
            if (skillNode == null)
            {
                return;
            }

            ///
            skillNode.OnStateChanged -= SkillNode_OnStateChanged;
        }

        public void ShowFor(SkillNode skillNode)
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            ///
            this.skillNode = skillNode;

            ///
            UpdatePosition();
            UpdateView();

            ///
            skillNode.OnStateChanged += SkillNode_OnStateChanged;

            ///
            if (!Application.isEditor
                && debugObject != null)
            {
                debugObject.SetActive(false);
            }
            else
            {
                var dt = skillNode.NodeId + "\r\n";
                dt += skillNode.CashTier + "\r\n";

                ///
                debugText.Text = dt;
            }

            ///
            gameObject.SetActive(true);
        }

        private void SkillNode_OnStateChanged()
        {
            UpdateView();
        }

        protected void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var skillPos = skillNode.transform.position;

            ///
            if (skillPos.x >= 0)
            {
                if (skillPos.y >= 0)
                {
                    rectTransform.pivot = new Vector2(1, 1);
                    transform.position = skillNode.ToolTipBottomLeftAnchor.position;
                    backgroundImage.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    rectTransform.pivot = new Vector2(1, 0);
                    transform.position = skillNode.ToolTipTopLeftAnchor.position;
                    backgroundImage.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (skillPos.y >= 0)
                {
                    rectTransform.pivot = new Vector2(0, 1);
                    transform.position = skillNode.ToolTipBottomRightAnchor.position;
                    backgroundImage.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    rectTransform.pivot = new Vector2(0, 0);
                    transform.position = skillNode.ToolTipTopRightAnchor.position;
                    backgroundImage.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        private void UpdateView()
        {
            backgroundIconImage.sprite = skillNode.Icon;
            descriptionText.Text = skillNode.Description;
            if (string.IsNullOrWhiteSpace(skillNode.ExtraDescription))
            {
                extraDescriptionText.gameObject.SetActive(false);
            }
            else
            {
                extraDescriptionText.gameObject.SetActive(true);
                extraDescriptionText.Text = skillNode.ExtraDescription;

            }
            if (skillNode.LevelCount == int.MaxValue)
            {
                levelText.Text = $"Lvl. {skillNode.Level}/\u221E";
            }
            else
            {
                levelText.Text = string.Format("Lvl. {0}/{1}", skillNode.Level, skillNode.LevelCount);
            }
            costText.Text = skillNode.IsMaxed ? "MAXED" : skillNode.GetCostString();

            ///
            if (skillNode.IsMaxed)
            {
                costWrapperImage.color = maxCostWrapperColor;
                costCanvasGroup.alpha = 1;
            }
            else
            {
                var isEnough = skillNode.IsEnoughCurrencyToLevelUp();
                costWrapperImage.color = isEnough ? normalCostWrapperColor : notEnoughCostWrapperColor;
                costCanvasGroup.alpha = isEnough ? 1 : notEnoughAlpha;
            }

            ///            
            demoNoticeObject.SetActive(skillNode.IsDemoLimited);
        }

#if UNITY_EDITOR
        public void Editor_ShowNodeGraph()
        {
            skillNode.Editor_SelectGraphNode();
        }

        public void Editor_RefundOneLevel()
        {
            skillNode.Editor_RefundOneLevel();
        }

        public void Editor_RefundAll()
        {
            skillNode.Editor_RefundAll();
        }

        public void Editor_LevelUp()
        {
            skillNode.Editor_LevelUp();
        }

        public void Editor_LevelUpMax()
        {
            skillNode.Editor_LevelUpMax();
        }
#endif
    }

}