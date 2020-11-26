using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterPlaneNoise : MonoBehaviour
{
    [SerializeField] private float power = 3;
    [SerializeField] private float scale = 1;
    [SerializeField] private float timeScale = 1;

    private float offsetX;
    private float offsetY;

    private MeshFilter filter;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();

        GenerateNoise();
    }

    private void Update()
    {
        GenerateNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetY += Time.deltaTime * timeScale;
    }

    private void GenerateNoise()
    {
        Vector3[] vertices = filter.mesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }

        filter.mesh.vertices = vertices;
    }

    private float CalculateHeight(float x, float y)
    {
        float xCoordinate = x * scale + offsetX;
        float yCoordinate = y * scale + offsetY;
        
        return Mathf.PerlinNoise(xCoordinate, yCoordinate);
    }
}
