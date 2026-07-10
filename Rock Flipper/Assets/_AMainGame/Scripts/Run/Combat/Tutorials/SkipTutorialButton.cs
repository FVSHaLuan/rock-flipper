using UnityEngine;

namespace BT.Run.Combat.Tutorials
{
    public class SkipTutorialButton : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject endCombatButton;

        protected void Start()
        {
            if (IsTutorial)
            {
                endCombatButton.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Skip()
        {
            throw new System.NotImplementedException();
        }
    }

}