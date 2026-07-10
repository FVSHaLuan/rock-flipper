using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Community
{
    public class Contributor : ScriptableObject
    {
        [SerializeField]
        private string contributorName;
        [SerializeField]
        private Sprite icon;

        public string ContributorName => contributorName;
        public Sprite Icon => icon;
    }

}