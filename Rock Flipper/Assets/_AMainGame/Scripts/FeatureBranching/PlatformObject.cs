using UnityEngine;

namespace Agame.FeatureBranching
{
    public class PlatformObject : MonoBehaviour
    {
        [SerializeField]
        private PlatformBranch platformBranch;

        protected void Awake()
        {
            switch (platformBranch)
            {
                case PlatformBranch.None:
                    gameObject.SetActive(false);
                    break;
                case PlatformBranch.All:
                    break;
                default:
                    gameObject.SetActive(PlatformBranchInfo.Current == platformBranch);
                    break;
            }
        }
    }

}