using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    int MapSizeX = 30;
    int MapSizeY = 30;
    void Start()
    {
        try
        {
            int[,] map = LoadMap();
            GlobalData.Map = map;
            GetComponent<MeshFilter>().mesh = MeshGeneration.GenerateMeshFromMap(map);
        }
        catch
        {
            GenerateNewMap();
        }
    }
    public void GenerateNewMap()
    {
        int[,] map = GenerateNewMap(35);
        GlobalData.Map = map;
        SaveMap(map);
        GetComponent<MeshFilter>().mesh = MeshGeneration.GenerateMeshFromMap(map);
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
