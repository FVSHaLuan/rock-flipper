using FH.Core.Architecture.Pool;
using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.UI
{
    public class UIBackground : GeneralPoolMemberSimplified
    {
        public event EventHandler OnShowed;
        public event EventHandler OnHid;
        public event System.Action OnShowingStateChanged;

        public delegate void EventHandler(UIBackground uiBackground);

        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float maxAlpha = 0.6f;
        [SerializeField]
        private float timeToReachMaxAlpha = 0.2f;

        [Space]
        [SerializeField]
        private Image blurImage;

        [Space]
        [SerializeField]
        private Transform childRoot;

        private BalancerWithObjects hiddenBalancer = new BalancerWithObjects();

        private List<Transform> popupTransforms = new List<Transform>();

        private RectTransform rectTransform;

        public Transform ChildRoot => childRoot;
        public bool IsShowing => !hiddenBalancer.IsBalanced;

        public override bool TryInit()
        {
            ///
            if (blurImage != null)
            {
                blurImage.material = Instantiate(blurImage.material); 
            }

            ///
            return base.TryInit();
        }

        public void RemoveShowingLock(object @object, Transform popupTransform)
        {
            ///
            TryInit();

            ///
            if (!hiddenBalancer.RemoveObject(@object))
            {
                return;
            }

            ///
            if (!popupTransforms.Remove(popupTransform))
            {
                return;
            }

            ///
            if (popupTransforms.Count > 0)
            {
                UpdatePopupTransform(popupTransforms[popupTransforms.Count - 1]);
            }
        }

        public void AddShowingLock(object @object, Transform popupTransform)
        {
            ///
            TryInit();

            ///
            if (popupTransform == null)
            {
                throw new System.ArgumentNullException();
            }

            ///
            popupTransforms.Add(popupTransform);

            ///
            UpdatePopupTransform(popupTransform);

            ///
            hiddenBalancer.AddObject(@object);
        }

        private void UpdatePopupTransform(Transform popupTransform)
        {
            ///
            if (transform.parent != popupTransform.parent)
            {
                canvasGroup.transform.SetParent(popupTransform.parent);

                ///
                CorrectSize();
            }

            ///
            int currentSiblingIndex = canvasGroup.transform.GetSiblingIndex();
            int popupTransformSiblingIndex = popupTransform.GetSiblingIndex();
            if (currentSiblingIndex != (popupTransformSiblingIndex - 1))
            {
                if (currentSiblingIndex > popupTransformSiblingIndex)
                {
                    canvasGroup.transform.SetSiblingIndex(popupTransformSiblingIndex);
                }
                else
                {
                    canvasGroup.transform.SetSiblingIndex(popupTransformSiblingIndex - 1);
                }
            }
        }

        private void CorrectSize()
        {
            ///
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;

            ///
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }

        public override void HandleGettingOutOfPool()
        {
            ///
            rectTransform = GetComponent<RectTransform>();

            ///
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
            enabled = false;

            ///
            hiddenBalancer.OnBalanceChanged += HiddenBalancer_OnBalanceChanged;

            ///
            base.HandleGettingOutOfPool();
        }

        private void HiddenBalancer_OnBalanceChanged()
        {
            ///
            if (!hiddenBalancer.IsBalanced)
            {
                canvasGroup.gameObject.SetActive(true);
            }

            ///
            enabled = true;

            ///
            canvasGroup.blocksRaycasts = !hiddenBalancer.IsBalanced;

            ///
            OnShowingStateChanged?.Invoke();

            ///
            if (IsShowing)
            {
                OnShowed?.Invoke(this);
            }
        }

        protected void Update()
        {
            ///
            if (popupTransforms.Count > 0)
            {
                UpdatePopupTransform(popupTransforms[popupTransforms.Count - 1]);
            }

            ///
            var targetAlpha = hiddenBalancer.IsBalanced ? 0 : maxAlpha;
            var speed = maxAlpha / timeToReachMaxAlpha;

            ///
            var currentAlpha = canvasGroup.alpha;

            ///
            UpdateBlurIntensity();

            ///
            if (Mathf.Approximately(targetAlpha, currentAlpha))
            {
                ///
                currentAlpha = targetAlpha;

                ///
                enabled = false;

                ///
                if (targetAlpha == 0)
                {
                    ///
                    OnHid?.Invoke(this);

                    ///
                    canvasGroup.gameObject.SetActive(false);
                }
            }
            else
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, speed * Time.unscaledDeltaTime);
            }

            ///
            canvasGroup.alpha = currentAlpha;
        }

        private void UpdateBlurIntensity()
        {
            if (blurImage != null)
            {
                ///
                float intensity = Mathf.InverseLerp(0, maxAlpha, canvasGroup.alpha);

                ///
                blurImage.material.SetFloat("_Intensity", intensity);
            }
        }
    }

}