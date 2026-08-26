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
        private float minRotation;
        [SerializeField]
        private float maxRotation = 360;

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
            [SerializeField]
            private int minCount;
            [SerializeField]
            private int maxCount;

            public float Weight => weight;
            public GameObject ElementPrototype => elementPrototype;
            public int MinCount => minCount;

            /// <summary>Max instances of this prototype allowed in the background. &lt;= 0 means unlimited.</summary>
            public int MaxCount => maxCount;
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
                element.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(minRotation, maxRotation));
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
            var counts = BuildPrototypeCounts();
            for (int i = 0; i < weightedElementPrototypes.Count; i++)
            {
                var prototype = weightedElementPrototypes[i].ElementPrototype;
                for (int c = 0; c < counts[i]; c++)
                {
                    var element = UnityEditor.PrefabUtility.InstantiatePrefab(prototype, elementRoot) as GameObject;
                    elements.Add(element);
                }
            }
        }

        /// <summary>Distributes elementCount instances across weightedElementPrototypes, respecting each prototype's min/max count.</summary>
        private int[] BuildPrototypeCounts()
        {
            var counts = new int[weightedElementPrototypes.Count];
            int total = 0;
            for (int i = 0; i < weightedElementPrototypes.Count; i++)
            {
                int min = Mathf.Max(0, weightedElementPrototypes[i].MinCount);
                counts[i] = min;
                total += min;
            }

            // If mins alone exceed elementCount, trim down starting from the last prototypes with a min.
            while (total > elementCount)
            {
                int trimIndex = -1;
                for (int i = counts.Length - 1; i >= 0; i--)
                {
                    if (counts[i] > 0)
                    {
                        trimIndex = i;
                        break;
                    }
                }
                if (trimIndex < 0)
                {
                    break;
                }
                counts[trimIndex]--;
                total--;
            }

            // Fill remaining slots via weighted pick, skipping prototypes that hit their max.
            var eligibleIndices = new List<int>();
            var eligibleProtos = new List<WeightedElementPrototype>();
            while (total < elementCount)
            {
                eligibleIndices.Clear();
                eligibleProtos.Clear();
                for (int i = 0; i < weightedElementPrototypes.Count; i++)
                {
                    var proto = weightedElementPrototypes[i];
                    if (proto.MaxCount <= 0 || counts[i] < proto.MaxCount)
                    {
                        eligibleIndices.Add(i);
                        eligibleProtos.Add(proto);
                    }
                }
                if (eligibleProtos.Count == 0)
                {
                    break;
                }

                eligibleProtos.PickOne(out int pickedLocalIndex, UnityRandom.Default);
                counts[eligibleIndices[pickedLocalIndex]]++;
                total++;
            }

            return counts;
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