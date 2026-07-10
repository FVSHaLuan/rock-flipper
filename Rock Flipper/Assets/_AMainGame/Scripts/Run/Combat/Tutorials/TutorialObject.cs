using UnityEngine;

namespace BT.Run.Combat.Tutorials
{
    public class TutorialObject : ExtendedMonoBehaviourRun
    {
        protected void Start()
        {
            if (IsTutorial)
            {
                ///
            }
            else
            {
                ///
                gameObject.SetActive(false);
            }
        }
    }

}