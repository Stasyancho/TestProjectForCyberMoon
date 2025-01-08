using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    int MapSizeX = 30;
    int MapSizeY = 30;
    void Start()
    {
        //GenerateNewMap();
        int[,] map = LoadMap();
        GetComponent<MeshFilter>().mesh = GenerateMeshFromMap(map);
    }

    public void GenerateNewMap()
    {
        int[,] map = GenerateNewMap(35);
        SaveMap(map);
        GetComponent<MeshFilter>().mesh = GenerateMeshFromMap(map);
    }

    Mesh GenerateMeshFromMap(int[,] map)
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
                    int pointX = i - map.GetLength(0) / 2;
                    int pointY = j - map.GetLength(1) / 2;
                    vertices.Add(new Vector3(0.1f + pointX, 0, 0.1f + pointY));
                    vertices.Add(new Vector3(0.1f + pointX, 0, 0.9f + pointY));
                    vertices.Add(new Vector3(0.9f + pointX, 0, 0.1f + pointY));
                    vertices.Add(new Vector3(0.9f + pointX, 0, 0.9f + pointY));

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

    int[,] LoadMap()
    {
        string stringMap = PlayerPrefs.GetString("Map");
        int index = 0;
        int[,] _map = new int[MapSizeX, MapSizeY];
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                _map[i, j] = (int)stringMap[index++] - 48;
            }
        }
        return _map;
    }
    void SaveMap(int[,] map)
    {
        string stringMap = "";
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                stringMap += map[i, j].ToString();
            }
        }
        PlayerPrefs.SetString("Map", stringMap);
    }


    int[,] GenerateNewMap(int percent)
    {
        int[,] _map = new int[MapSizeX, MapSizeY];
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                float rand = UnityEngine.Random.value;
                int znach = rand > percent / 100f ? 0 : 1;
                _map[i,j] = znach;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            _map = UpdateMap(_map);
        }

        return _map;
    }
    int[,] UpdateMap(int[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = NewValue(map, i, j);
            }
        }
        return map;
    }

    int NewValue(int[,] map, int x, int y)
    {
        int mas = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for ( int  j = y - 1; j < y + 2; j++)
            {
                if (i >= 0 && i < map.GetLength(0) && j >= 0 && j < map.GetLength(1)) mas += map[i, j];
            }
        }

        if (mas > 4) return 1;
        if (mas < 4) return 0;
        return map[x, y];
    }
}
