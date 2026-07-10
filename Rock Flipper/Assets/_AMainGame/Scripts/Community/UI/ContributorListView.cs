using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Community.UI
{
    public class ContributorListView : MonoBehaviourWithInit
    {
        [SerializeField]
        private ContributorView firstContributorView;
        [SerializeField]
        private GameObject firstSeparator;

        private List<ContributorView> contributorViews = new List<ContributorView>();
        private List<GameObject> separators = new List<GameObject>();

        protected override bool Init()
        {
            ///
            contributorViews.Add(firstContributorView);
            separators.Add(firstSeparator);

            ///
            return base.Init();
        }

        public void View(List<Contributor> contributors)
        {
            ///
            TryInit();

            // Hide all current views
            foreach (var item in contributorViews)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in separators)
            {
                item.gameObject.SetActive(false);
            }

            ///
            for (int i = 0; i < contributors.Count; i++)
            {
                // Separator
                if (i > 0)
                {
                    GetSeparator(i - 1).SetActive(true);
                }

                ///
                var contributorView = GetContributorView(i);
                contributorView.gameObject.SetActive(true);
                contributorView.View(contributors[i]);
            }
        }

        private ContributorView GetContributorView(int index)
        {
            ///
            if (index < contributorViews.Count - 1)
            {
                return contributorViews[index];
            }

            ///
            var contributorView = Instantiate(firstContributorView, firstContributorView.transform.parent);
            contributorView.transform.SetAsLastSibling();
            contributorViews.Add(contributorView);

            ///
            return contributorView;
        }

        private GameObject GetSeparator(int index)
        {
            ///
            if (index < separators.Count - 1)
            {
                return separators[index];
            }

            ///
            var separator = Instantiate(firstSeparator, firstSeparator.transform.parent);
            separator.transform.SetAsLastSibling();
            separators.Add(separator);

            ///
            return separator;
        }
    }

}