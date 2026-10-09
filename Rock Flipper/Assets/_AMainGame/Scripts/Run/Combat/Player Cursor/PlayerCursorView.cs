using UnityEngine;

namespace Agame.Run.Combat
{
    public class PlayerCursorView : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject noRadiusView;
        [SerializeField]
        private GameObject radiusView;

        private bool hasPositionOverride;
        private Vector2 positionOverride;
        private Vector3 noRadiusViewLocalPosition;
        private Vector3 radiusViewLocalPosition;

        /// <summary>
        /// Shows the cursor at <paramref name="position"/> instead of under the real pointer, until <see cref="ClearPositionOverride"/>
        /// </summary>
        public void SetPositionOverride(Vector2 position)
        {
            if (!hasPositionOverride)
            {
                hasPositionOverride = true;
                noRadiusViewLocalPosition = noRadiusView.transform.localPosition;
                radiusViewLocalPosition = radiusView.transform.localPosition;
            }

            ///
            positionOverride = position;
            ApplyPositionOverride();
        }

        public void ClearPositionOverride()
        {
            if (!hasPositionOverride)
                return;

            ///
            hasPositionOverride = false;
            noRadiusView.transform.localPosition = noRadiusViewLocalPosition;
            radiusView.transform.localPosition = radiusViewLocalPosition;
        }

        private void ApplyPositionOverride()
        {
            SetWorldPosition(noRadiusView.transform, positionOverride);
            SetWorldPosition(radiusView.transform, positionOverride);
        }

        private static void SetWorldPosition(Transform target, Vector2 position)
        {
            target.position = new Vector3(position.x, position.y, target.position.z);
        }

        protected void OnEnable()
        {
            UpdateView();
        }

        protected void Update()
        {
            UpdateView();
        }

        protected void LateUpdate()
        {
            // the cursor object follows the real pointer during Update, which drags the views along with it
            if (hasPositionOverride)
            {
                ApplyPositionOverride();
            }
        }

        protected void UpdateView()
        {
            if (BuildStats.enabledMouseRadius)
            {
                noRadiusView.SetActive(false);
                radiusView.SetActive(true);

                ///
                radiusView.transform.localScale = Vector3.one * BuildStats.mouseRadius * 2;
            }
            else
            {
                noRadiusView.SetActive(true);
                radiusView.SetActive(false);
            }
        }
    }
}