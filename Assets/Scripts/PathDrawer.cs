using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    Point lastPoint = new Point(-1, 0);
    Point mouseClick = new Point(1, 1);
    MeshFilter mesh;
    private void Start()
    {
        mesh = GetComponent<MeshFilter>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 point = new Vector2(hit.point.x - 0.1f, hit.point.z - 0.1f);
            int pointX = Mathf.CeilToInt(point.x) - 1;
            int pointY = Mathf.CeilToInt(point.y) - 1;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            if (pointX >= 0 && pointY >= 0 && pointX + 0.8f > point.x && pointY + 0.8f > point.y && GlobalData.Map[pointX, pointY] != 1)
            {
                if (lastPoint.X != pointX || lastPoint.Y != pointY)
                {
                    lastPoint = new Point(pointX, pointY);
                    int[,] newMap = new int[GlobalData.Map.GetLength(0), GlobalData.Map.GetLength(1)];
                    newMap = (int[,])GlobalData.Map.Clone();
                    newMap[mouseClick.X, mouseClick.Y] = 2;
                    newMap[pointX, pointY] = 3;
                    var cells = PathGenerater.GetCellsFromMap(newMap);
                    var path = PathGenerater.GenerateWay(cells);
                    mesh.mesh = MeshGeneration.GenerateLine(path);
                    Debug.Log(mouseClick);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    mouseClick = new Point(pointX, pointY);
                }
            }
            else
            {
                lastPoint.X = -1;
            }
        }
        else
        {
            lastPoint.X = -1;
        }

        if (lastPoint.X == -1)
        {
            mesh.mesh = new Mesh();
            lastPoint.X = -2;
        }
    }
}
