using UnityEngine;

namespace Agame.Run.Combat.Tutorials
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