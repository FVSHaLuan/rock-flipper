using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Community.UI
{
    public class ContributorView : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private UnifiedText nameText;

        public void View(Contributor contributor)
        {
            iconImage.sprite = contributor.Icon;
            nameText.Text = contributor.ContributorName;
        }
    }

}