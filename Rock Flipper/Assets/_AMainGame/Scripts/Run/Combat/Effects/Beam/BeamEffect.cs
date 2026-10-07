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
        [SerializeField]
        private LineRenderer lineRenderer;

        public void SetStartPoint(Vector3 position)
        {
            startPointView.transform.position = position;
            Stretch();
        }

        public void SetEndPoint(Vector3 position)
        {
            endPointView.transform.position = position;
            Stretch();
        }

        [ContextMenu("Stretch"), PlayModeOnly]
        private void Stretch()
        {
            Vector3 start = startPointView.transform.position;
            Vector3 end = endPointView.transform.position;

            if (!lineRenderer.useWorldSpace)
            {
                start = lineRenderer.transform.InverseTransformPoint(start);
                end = lineRenderer.transform.InverseTransformPoint(end);
            }

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
    }

}