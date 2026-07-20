using UnityEngine;
using UnityEngine.Rendering;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Flippable)), RequireComponent(typeof(SortingLayer))]
    public class FlippableSortingLayerSetter : MonoBehaviour
    {
        [SerializeField]
        private string staticSortingLayer;
        [SerializeField]
        private string flippingSortingLayer;

        private Flippable flippable;
        private SortingGroup sortingGroup;

        protected void Start()
        {
            flippable = GetComponent<Flippable>();
            sortingGroup = GetComponent<SortingGroup>();

            ///
            UpdateSortingLayer();

            ///
            flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
            flippable.OnFinishedFlipping += Flippable_OnFinishedFlipping;
        }

        private void Flippable_OnFinishedFlipping()
        {
            UpdateSortingLayer();
        }

        private void Flippable_OnStartedFlipping()
        {
            UpdateSortingLayer();
        }

        private void UpdateSortingLayer()
        {
            var sortingLayerName = flippable.IsFlipping ? flippingSortingLayer : staticSortingLayer;
            sortingGroup.sortingLayerID = SortingLayer.NameToID(sortingLayerName);
        }
    }

}