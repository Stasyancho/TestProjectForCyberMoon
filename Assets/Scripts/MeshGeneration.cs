using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public static class MeshGeneration
{
    public static Mesh GenerateMeshFromMap(int[,] map)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        Mesh mesh = new Mesh();
        int index = 0;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 0)
                {
                    vertices.Add(new Vector3(0.1f + i, 0, 0.1f + j));
                    vertices.Add(new Vector3(0.1f + i, 0, 0.9f + j));
                    vertices.Add(new Vector3(0.9f + i, 0, 0.1f + j));
                    vertices.Add(new Vector3(0.9f + i, 0, 0.9f + j));

                    triangles.Add(0 + index);
                    triangles.Add(1 + index);
                    triangles.Add(2 + index);
                    triangles.Add(2 + index);
                    triangles.Add(1 + index);
                    triangles.Add(3 + index);

                    index += 4;
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    public static Mesh GenerateLine(List<Point> points)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        Mesh mesh = new Mesh();
        int index = 0;

        foreach (Point point in points)
        {
            vertices.Add(new Vector3(0.1f + point.X, 0, 0.1f + point.Y));
            vertices.Add(new Vector3(0.1f + point.X, 0, 0.9f + point.Y));
            vertices.Add(new Vector3(0.9f + point.X, 0, 0.1f + point.Y));
            vertices.Add(new Vector3(0.9f + point.X, 0, 0.9f + point.Y));

            triangles.Add(0 + index);
            triangles.Add(1 + index);
            triangles.Add(2 + index);
            triangles.Add(2 + index);
            triangles.Add(1 + index);
            triangles.Add(3 + index);

            index += 4;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }
}
