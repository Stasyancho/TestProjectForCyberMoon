using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PathDrawer : MonoBehaviour
{
    public GameObject ActiveZone;
    public Text roundText;
    public int DistanceToMove;
    int remainingSteps;
    int round = 0;
    Point lastPoint = new Point(-1, 0);
    Point mouseClick;
    MeshFilter mesh;
    MeshFilter zoneMesh;
    int[,] AreaMap;
    PathGenerater.Cell[,] cells;
    private void Start()
    {
        mesh = GetComponent<MeshFilter>();
        zoneMesh = ActiveZone.GetComponent<MeshFilter>();
        remainingSteps = DistanceToMove;
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
                if (AreaMap != null)
                {
                    if ( AreaMap[pointX, pointY] != 1)
                    {
                        if ((lastPoint.X != pointX || lastPoint.Y != pointY) && (pointX != mouseClick.X || pointY != mouseClick.Y))
                        {
                            lastPoint = new Point(pointX, pointY);
                            int[,] newMap = new int[GlobalData.Map.GetLength(0), GlobalData.Map.GetLength(1)];
                            newMap = (int[,])GlobalData.Map.Clone();
                            newMap[mouseClick.X, mouseClick.Y] = 2;
                            newMap[pointX, pointY] = 3;
                            var cells = PathGenerater.GetCellsFromMap(newMap);
                            var path = PathGenerater.GenerateWay(cells);
                            mesh.mesh = MeshGeneration.GenerateLine(path);
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            SomeActionWhenClick(pointX, pointY);
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SomeActionWhenClick(pointX, pointY);
                    }
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

    void SomeActionWhenClick(int pointX, int pointY)
    {
        if (cells != null)
        {
            int x = pointX + (remainingSteps < mouseClick.X ? remainingSteps - mouseClick.X : 0);
            int y = pointY + (remainingSteps < mouseClick.Y ? remainingSteps - mouseClick.Y : 0);
            remainingSteps -= cells[x, y].cellCost / 10;
            if (remainingSteps == 0)
            {
                roundText.text = (++round).ToString();
                remainingSteps = DistanceToMove;
            }
        }
        int[,] newMap = new int[GlobalData.Map.GetLength(0), GlobalData.Map.GetLength(1)];
        newMap = (int[,])GlobalData.Map.Clone();
        AreaMap = AreaGenerater.GetEnableArea(newMap, pointX, pointY, remainingSteps, out cells);
        zoneMesh.mesh = MeshGeneration.GenerateMeshFromMap(AreaMap);
        lastPoint.X = -1;
        mouseClick = new Point(pointX, pointY);
    }

    public void IfNewGeneration()
    {
        mesh.mesh = new Mesh();
        cells = null;
        remainingSteps = DistanceToMove;
        round = 0;
        roundText.text = (round).ToString();
        lastPoint = new Point(-1, 0);
        zoneMesh.mesh = new Mesh();
        AreaMap = null;
    }
}
