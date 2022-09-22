using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class MeshGenerator
    {
        public static MeshData GenerateMesh(int width, int length, float[,] noiseMap)
        {
            MeshData meshData = new MeshData(width, length);
            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (width - 1) / 2f;
            int vertexIndex = 0;

            for(int z = 0; z < length; z++)
            {
                for(int x = 0; x < width; x++)
                {
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, 0, topLeftZ - z);
                    meshData.uv[vertexIndex] = new Vector2(x / (float)width, z / (float)length);

                    if (x < width - 1 && z < length - 1)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex+width+1, vertexIndex, vertexIndex + 1);
                    }
                    vertexIndex++;
                }
            }
            return meshData;
        }
    }

    public class MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uv;

        int triangleIndex;

        public MeshData(int meshWidth, int meshLength)
        {
            vertices = new Vector3[meshWidth * meshLength];
            triangles = new int[(meshWidth - 1) * (meshLength - 1) * 6];
            uv = new Vector2[meshWidth * meshLength];
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex+1] = b;
            triangles[triangleIndex+2] = c;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}