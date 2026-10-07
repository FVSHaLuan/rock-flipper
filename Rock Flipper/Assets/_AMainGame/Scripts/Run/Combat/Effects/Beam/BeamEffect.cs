using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// A visual effect that represents a beam between two points in the game world.
    /// </summary>
    public class BeamEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject startPointView;
        [SerializeField]
        private GameObject endPointView;

        public void SetStartPoint(Vector3 position)
        {
            startPointView.transform.position = position;
        }

        public void SetEndPoint(Vector3 position)
        {
            endPointView.transform.position = position;
        }

        private void Stretch()
        {

        }
    }

}