using GD;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Community.UI
{
    public class LocalizationContributorDisplayer : ExtendedMonoBehaviour
    {
        [SerializeField]
        private LocalizedString contributorListTerm;
        [SerializeField]
        private ContributorListView contributorListView;

        private List<Contributor> contributors = new List<Contributor>();

        public void UpdateView()
        {
            ///
            var contributorList = LocalizationManager.GetTranslatedObjectByTermName<ContributorList>(contributorListTerm.mTerm);

            ///
            contributorList?.GetContributors(contributors);

            ///
            if (contributorList == null
                || contributors.Count == 0)
            {
                contributorListView.gameObject.SetActive(false);
                return;
            }

            ///
            contributorListView.gameObject.SetActive(true);
            contributors.Shuffle(UnityRandom.Default);
            contributorListView.View(contributors);
        }
    }
}
