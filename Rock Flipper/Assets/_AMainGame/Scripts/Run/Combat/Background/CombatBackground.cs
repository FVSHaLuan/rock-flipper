using GD;
using OneLine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat.Backgrounds
{
    public class CombatBackground : ExtendedMonoBehaviourRun
    {
        [Space]
        [SerializeField]
        private bool isNaNInDemo;

        [Space]
        [SerializeField]
        private string backgroundName;
        [SerializeField]
        private Color cameraColor = Color.white;
        [SerializeField]
        private Sprite thumbnail;

        [Space]
        [SerializeField]
        private Transform elementRoot;
        [SerializeField]
        private int elementCount = 30;
        [SerializeField, OneLineWithHeader]
        private List<WeightedElementPrototype> weightedElementPrototypes = new List<WeightedElementPrototype>();
        [SerializeField, ReadOnly]
        private List<GameObject> elements = new List<GameObject>();

        [Space]
        [SerializeField]
        private int clusterCountMin = 2;
        [SerializeField]
        private int clusterCountMax = 5;
        [SerializeField]
        private float clusterRadius = 2.5f; // Radius of each cluster

        [Space]
        [SerializeField]
        private UnityEvent onInitThisCombat;

        private List<Vector2> clusterCenters = new List<Vector2>();

        [System.Serializable]
        private struct WeightedElementPrototype : IWeighted
        {
            [SerializeField]
            private GameObject elementPrototype;
            [SerializeField]
            private float weight;

            public float Weight => weight;
            public GameObject ElementPrototype => elementPrototype;
        }

        public Color CameraColor => cameraColor;

        protected override void InitThisCombat()
        {
            ///
            base.InitThisCombat();

            ///
            RandomizeElements();

            ///
            onInitThisCombat?.Invoke();
        }

        [ContextMenu("RandomizeElements")]
        private void RandomizeElements()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.RegisterFullObjectHierarchyUndo(elementRoot, "Randomize elements");
            }
#endif

            ///
            float minX = -10;
            float maxX = 10;
            float minY = -5;
            float maxY = 5;

            ///
            if (Application.isPlaying)
            {
                var playfield = RunEntry.playfield;
                minX = playfield.MinX;
                maxX = playfield.MaxX;
                minY = playfield.MinY;
                maxY = playfield.MaxY;
            }

            // Parameters for clustering
            int clusterCount = Random.Range(clusterCountMin, clusterCountMax); // Number of clusters

            // Generate cluster centers
            clusterCenters.Clear();
            for (int i = 0; i < clusterCount; i++)
            {
                float clusterX = Random.Range(minX, maxX);
                float clusterY = Random.Range(minY, maxY);
                clusterCenters.Add(new Vector2(clusterX, clusterY));
            }

            // Place elements around cluster centers
            for (int i = 0; i < elementCount; i++)
            {
                // Pick a random cluster center
                Vector2 clusterCenter = clusterCenters[Random.Range(0, clusterCenters.Count)];

                // Generate a position within the cluster radius
                var elementPos = Random.insideUnitCircle * clusterRadius + clusterCenter;

                // Clamp position to bounds
                elementPos.x = Mathf.Clamp(elementPos.x, minX, maxX);
                elementPos.y = Mathf.Clamp(elementPos.y, minY, maxY);

                // Instantiate element                
                var element = elements[i];
                element.transform.localPosition = elementPos;
                element.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }

        }

#if UNITY_EDITOR
        [ContextMenu("Editor_SpawnElements"), EditorModeOnly]
        private void Editor_SpawnElements()
        {
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(elementRoot, "Spawn elements for background");

            ///
            foreach (var element in elements)
            {
                DestroyImmediate(element);
            }
            elements.Clear();

            ///
            for (int i = 0; i < elementCount; i++)
            {
                var prototype = weightedElementPrototypes.PickOne(UnityRandom.Default).ElementPrototype;
                var element = UnityEditor.PrefabUtility.InstantiatePrefab(prototype, elementRoot) as GameObject;
                elements.Add(element);
            }
        }

        [ContextMenu("Editor_SetAsActive"), PlayModeOnly]
        private void Editor_SetAsActive()
        {
            throw new System.NotImplementedException("Editor_SetAsActive is not implemented yet.");
            //RunData.ActiveBackgroundId = RunEntry.combatBackgroundManager.GetBackgroundId(this);
            //RunEntry.combatBackgroundManager.UpdateBackgroundForCurrentCombat();
        }
#endif
    }
}