using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace FH.Core.HelperComponent
{
    public abstract class OutsideTargetRectTransform : MonoBehaviour
    {
        [SerializeField]
        RectTransform targetRectTransform;

        protected RectTransform TargetRectTransform
        {
            get
            {
                return targetRectTransform;
            }

            private set
            {
                targetRectTransform = value;
            }
        }

        public void Reset()
        {
            TargetRectTransform = GetComponent<RectTransform>();
        }

        public void OnValidate()
        {
            if (this.gameObject.scene != null && this.gameObject.scene.name != null)
            {
                Assert.IsTrue(TargetRectTransform != null);
            }
        }
    }

}