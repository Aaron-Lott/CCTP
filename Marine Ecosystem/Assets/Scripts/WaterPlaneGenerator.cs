using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterPlaneGenerator : MonoBehaviour
{
    public float size = 1f;
    [SerializeField] private int gridSize = 16;

    private MeshFilter filter;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();

        filter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        for(int x = 0; x < gridSize + 1; x++)
        {
            for(int y = 0; y < gridSize + 1; y++)
            {
                vertices.Add(GenerateVertex(x, y));
                normals.Add(Vector3.up);
                uvs.Add(GenerateUV(x, y));
            }
        }

        var triangles = new List<int>();
        var vertexCount = gridSize + 1;

        for(int i = 0; i < vertexCount * vertexCount - vertexCount; i++)
        {
            if((i + 1) % vertexCount == 0)
            {
                continue;
            }

            triangles.AddRange(new List<int>() {
                i + 1 + vertexCount, i + vertexCount, i, i, i + 1, i + vertexCount + 1
            });
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        return mesh;
    }

    private Vector3 GenerateVertex(int x, int y)
    {
        return new Vector3(-size * 0.5f + size * (x / (float)gridSize), 0, -size * 0.5f + size * (y / (float)gridSize));
    }

    private Vector2 GenerateUV(int x, int y)
    {
        return new Vector2(x / (float)gridSize, y / (float)gridSize);
    }

    public void FlipPlane(int direction)
    {
        if (direction > 1) direction = 1;
        if (direction < -1) direction = -1;

        transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);
    }
}
