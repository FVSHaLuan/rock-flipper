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
        private Transform rockRoot;
        [SerializeField]
        private int rockCount = 30;
        [SerializeField, OneLineWithHeader]
        private List<WeightedRockPrototype> weightedRockPrototypes = new List<WeightedRockPrototype>();
        [SerializeField, ReadOnly]
        private List<GameObject> rocks = new List<GameObject>();

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
        private struct WeightedRockPrototype : IWeighted
        {
            [SerializeField]
            private GameObject rockPrototype;
            [SerializeField]
            private float weight;

            public float Weight => weight;
            public GameObject RockPrototype => rockPrototype;
        }

        public Color CameraColor => cameraColor;

        protected override void InitThisCombat()
        {
            ///
            base.InitThisCombat();

            ///
            RandomizeRocks();

            ///
            onInitThisCombat?.Invoke();
        }

        [ContextMenu("RandomizeRocks")]
        private void RandomizeRocks()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.RegisterFullObjectHierarchyUndo(rockRoot, "Randomize rocks");
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

            // Place rocks around cluster centers
            for (int i = 0; i < rockCount; i++)
            {
                // Pick a random cluster center
                Vector2 clusterCenter = clusterCenters[Random.Range(0, clusterCenters.Count)];

                // Generate a position within the cluster radius
                var rockPos = Random.insideUnitCircle * clusterRadius + clusterCenter;

                // Clamp position to bounds
                rockPos.x = Mathf.Clamp(rockPos.x, minX, maxX);
                rockPos.y = Mathf.Clamp(rockPos.y, minY, maxY);

                // Instantiate rock                
                var rock = rocks[i];
                rock.transform.localPosition = rockPos;
                rock.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }

        }

#if UNITY_EDITOR
        [ContextMenu("Editor_SpawnRocks"), EditorModeOnly]
        private void Editor_SpawnRocks()
        {
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(rockRoot, "Spawn rocks for background");

            ///
            foreach (var rock in rocks)
            {
                DestroyImmediate(rock);
            }
            rocks.Clear();

            ///
            for (int i = 0; i < rockCount; i++)
            {
                var prototype = weightedRockPrototypes.PickOne(UnityRandom.Default).RockPrototype;
                var rock = UnityEditor.PrefabUtility.InstantiatePrefab(prototype, rockRoot) as GameObject;
                rocks.Add(rock);
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