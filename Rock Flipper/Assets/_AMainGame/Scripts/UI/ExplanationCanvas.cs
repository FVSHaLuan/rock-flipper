using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI
{
    public class ExplanationCanvas : ExtendedMonoBehaviour
    {
        [SerializeField]
        private RectTransform explanationWrapper;
        [SerializeField]
        private ExplanationBoxUI explanationBoxUIPrototype;

        private BalancerWithObjects hiddenBalancer = new BalancerWithObjects();
        private List<ExplanationBoxUI> explanationBoxes = new List<ExplanationBoxUI>();

        public void Show(object lockingObject, Vector2 pointerWorldPosition, params string[] texts)
        {
            ///
            hiddenBalancer.AddObject(lockingObject);

            ///
            explanationWrapper.gameObject.SetActive(true);

            ///
            ViewTexts(texts);
            SetWrapperPosition(pointerWorldPosition);
        }

        private void SetWrapperPosition(Vector2 pointerWorldPosition)
        {
            ///
            Vector2 pivot = new Vector2();

            ///            
            pivot.x = pointerWorldPosition.x < 0 ? 0 : 1;
            pivot.y = pointerWorldPosition.y < 0 ? 0 : 1;

            ///
            explanationWrapper.pivot = pivot;

            ///
            var p = explanationWrapper.position;
            p = new Vector3(pointerWorldPosition.x, pointerWorldPosition.y, p.z);
            explanationWrapper.position = p;
        }

        private void ViewTexts(params string[] texts)
        {
            while (explanationBoxes.Count < texts.Length)
            {
                var explanationBox = generalPool.TakeInstance(explanationBoxUIPrototype, this);

                ///
                explanationBox.transform.SetParent(explanationWrapper);
                explanationBox.transform.localScale = Vector3.one;

                ///
                explanationBoxes.Add(explanationBox);
            }

            ///
            while (explanationBoxes.Count > texts.Length)
            {
                ///
                explanationBoxes[explanationBoxes.Count - 1].TryReturnToPoolAndDeactivate();

                ///
                explanationBoxes.RemoveAt(explanationBoxes.Count - 1);
            }

            ///
            for (int i = 0; i < texts.Length; i++)
            {
                var explanationBox = explanationBoxes[i];

                ///
                explanationBox.Text = texts[i];
                explanationBox.gameObject.SetActive(true);
            }
        }

        public void Hide(object lockingObject)
        {
            ///
            hiddenBalancer.RemoveObject(lockingObject);

            ///
            if (!hiddenBalancer.IsBalanced)
            {
                return;
            }

            ///
            foreach (var item in explanationBoxes)
            {
                item.TryReturnToPoolAndDeactivate();
            }

            ///
            explanationBoxes.Clear();

            ///
            explanationWrapper.gameObject.SetActive(false);
        }
    }

}