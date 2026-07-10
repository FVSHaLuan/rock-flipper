using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class IntegerTiledRectransformVertical : MonoBehaviour
    {
        [SerializeField]
        private RectTransform sizeRef;
        [SerializeField]
        private float tileHeight = 100;
        [SerializeField]
        private float cutOffThreshold = 0;
        [SerializeField]
        private float refAdditionalHeight;

        private RectTransform rectTransform;
        private int updateCount = 0;

        public void DoCorrectAtEndOfFrame()
        {
            updateCount = 3;
        }

        [ContextMenu("Correct")]
        public void Correct()
        {
            ///
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            ///
            var size = rectTransform.sizeDelta;

            ///
            var fullSize = sizeRef.rect.height + refAdditionalHeight;

            ///
            var integerSize = Mathf.Floor(fullSize / tileHeight) * tileHeight;

            ///
            var cutOff = fullSize - integerSize;

            ///
            size.y = cutOff < cutOffThreshold ? integerSize : fullSize;

            ///
            rectTransform.sizeDelta = size;
        }

        protected void LateUpdate()
        {
            ///
            if (updateCount <= 0)
            {
                return;
            }

            ///
            updateCount--;

            ///
            Correct();
        }
    }
}