using UnityEngine;

namespace Agame.Run.Combat
{
    public class PlayerCursorView : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject noRadiusView;
        [SerializeField]
        private GameObject radiusView;

        protected void OnEnable()
        {
            UpdateView();
        }

        protected void Update()
        {
            UpdateView();
        }

        protected void UpdateView()
        {
            if (BuildStats.enabledPlayerCursorRadius)
            {
                noRadiusView.SetActive(false);
                radiusView.SetActive(true);

                ///
                radiusView.transform.localScale = Vector3.one * BuildStats.playerCursorRadius * 2;
            }
            else
            {
                noRadiusView.SetActive(true);
                radiusView.SetActive(false);
            }
        }
    }
}