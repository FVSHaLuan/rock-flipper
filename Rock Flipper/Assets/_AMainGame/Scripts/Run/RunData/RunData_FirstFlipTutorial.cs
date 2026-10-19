using UnityEngine;

namespace Agame
{
    /// <summary>
    /// Whether this save has finished the first flip tutorial. See <see cref="Run.Combat.Tutorials.FirstFlipTutorial"/>.
    /// </summary>
    public partial class RunData
    {
        [SerializeField]
        private bool finishedFirstFlipTutorial;

        public bool FinishedFirstFlipTutorial => finishedFirstFlipTutorial;

        public void FinishFirstFlipTutorial()
        {
            finishedFirstFlipTutorial = true;
        }
    }

}
