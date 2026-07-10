using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Community
{
    public class ContributorList : ScriptableObject
    {
        [SerializeField]
        private List<Contributor> contributors;

        public void GetContributors(List<Contributor> contributors)
        {
            contributors.Clear();

            ///
            if (contributors == null)
            {
                return;
            }

            ///
            for (int i = 0; i < this.contributors.Count; i++)
            {
                contributors.Add(this.contributors[i]);
            }
        }
    }
}
