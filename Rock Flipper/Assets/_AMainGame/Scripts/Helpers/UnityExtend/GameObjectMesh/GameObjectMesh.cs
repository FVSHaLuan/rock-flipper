using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Unfinished!")]
[RequireComponent(typeof(MeshFilter))]
public class GameObjectMesh : MonoBehaviour
{
    private MeshFilter meshFilter;

    [SerializeField]
    private bool useLocalPosition;

    [SerializeField]
    private List<GameObjectVertex> gameObjectVertices;
    [SerializeField]
    private List<GameObjectVertex> gameObjectTriangleNodes;

    private bool inited = false;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangleNodes;

    protected void Awake()
    {
        TryInit();
    }

    [ContextMenu("UpdateMesh")]
    public void UpdateMesh()
    {
        ///
        TryInit();

        ///
        var thisPos = transform.position;
        for (int i = 0; i < gameObjectVertices.Count; i++)
        {
            ///
            var gov = gameObjectVertices[i];

            // Vertices
            vertices[i] = useLocalPosition ? gov.transform.localPosition + thisPos : gov.transform.position;

            // UV
            uvs[i] = gov.UV;
        }

        // Triangles
        for (int i = 0; i < gameObjectTriangleNodes.Count; i++)
        {
            triangleNodes[i] = gameObjectTriangleNodes[i].vertexId;
        }

        ///
        meshFilter.sharedMesh.SetVertices(vertices);
        meshFilter.sharedMesh.SetUVs(0, uvs);
        meshFilter.sharedMesh.SetTriangles(triangleNodes, 0);
    }

    private void TryInit()
    {
        ///
        if (inited)
        {
            return;
        }

        ///
        if (Application.isPlaying)
        {
            inited = true;
        }

        ///
        meshFilter = GetComponent<MeshFilter>();

        ///
        if (vertices == null || vertices.Length != gameObjectVertices.Count)
        {
            vertices = new Vector3[gameObjectVertices.Count];
        }
        if (uvs == null || uvs.Length != gameObjectVertices.Count)
        {
            uvs = new Vector2[gameObjectVertices.Count];
        }
        if (triangleNodes == null || triangleNodes.Length != gameObjectTriangleNodes.Count)
        {
            triangleNodes = new int[gameObjectTriangleNodes.Count];
        }

        ///
        for (int i = 0; i < gameObjectVertices.Count; i++)
        {
            gameObjectVertices[i].vertexId = i;
        }
    }

    //    protected void Update()
    //    {
    //#if UNITY_EDITOR
    //        TryInit();

    //#endif
    //    }
}
