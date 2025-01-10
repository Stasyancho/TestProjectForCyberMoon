using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class PathGenerater
{
    static Dictionary<int, string> costs = new Dictionary<int, string>()
    {
        [0] = "10",
        [1] = "no",
        [2] = "start",
        [3] = "end"//,
        //[1] = "start",
        //[2] = "end",
        //[3] = "no",
        //[4] = "50",
        //[5] = "30"
    };
    public static Cell[,] GetCellsFromMap(int[,] _map)
    {
        int minStepCost = Convert.ToInt32(costs[0]);
        int n = _map.GetLength(0);
        int m = _map.GetLength(1);
        Cell[,] cells = new Cell[n, m];

        int ii = 0;
        int jj = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (costs[_map[i, j]] == "end")
                {
                    ii = i;
                    jj = j;
                }
            }
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                switch (costs[_map[i, j]])
                {
                    case "start":
                        cells[i, j] = new Cell()
                        {
                            cellCost = 0,
                            distanceToEnd = (Math.Abs(i - ii) + Math.Abs(j - jj)) * minStepCost
                        };
                        break;
                    case "end":
                        cells[i, j] = new Cell()
                        {
                            stepCost = minStepCost,
                            isEnd = true
                        };
                        break;
                    case "no":
                        cells[i, j] = new Cell()
                        {
                            stepCost = -1
                        };
                        break;
                    default:
                        cells[i, j] = new Cell()
                        {
                            stepCost = Convert.ToInt32(costs[_map[i, j]]),
                            distanceToEnd = (Math.Abs(i - ii) + Math.Abs(j - jj)) * minStepCost
                        };
                        break;
                }
            }
        }

        return cells;
    }

    public static List<Point> GenerateWay(Cell[,] cells)
    {
        int ii = 0;
        int jj = 0;
        string a1, a2, a3, a4;
        StartPositionFromCells(cells, out ii, out jj);
        while (!cells[ii, jj].isEnd)
        {
            a1 = "";
            a2 = "";
            a3 = "";
            a4 = "";
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    a1 += cells[i, j].cellCost.ToString() + " | ";
                    a2 += cells[i, j].isCheckeed.ToString() + " | ";
                    a4 += cells[i, j].stepCost.ToString() + " | ";
                }
                a1 += '\n';
                a2 += '\n';
                a4 += '\n';
            }
            a3 = a1 + '\n' + a2 + '\n' + a4;
            if (cells[ii, jj].isCheckeed)
            {
                return null;
            }
            cells[ii, jj].isCheckeed = true;
            if (ii > 0)
            {
                if ((cells[ii - 1, jj].cellCost == -1 || cells[ii - 1, jj].cellCost > cells[ii, jj].cellCost + cells[ii - 1, jj].stepCost) && cells[ii - 1, jj].stepCost != -1)
                {
                    cells[ii - 1, jj].cellCost = cells[ii, jj].cellCost + cells[ii - 1, jj].stepCost;
                    cells[ii - 1, jj].prevCell = new Point(ii, jj);
                }

                if (jj > 0)
                {
                    if ((cells[ii - 1, jj - 1].cellCost == -1 || cells[ii - 1, jj - 1].cellCost > cells[ii, jj].cellCost + cells[ii - 1, jj - 1].stepCost) && cells[ii - 1, jj - 1].stepCost != -1)
                    {
                        cells[ii - 1, jj - 1].cellCost = cells[ii, jj].cellCost + cells[ii - 1, jj - 1].stepCost;
                        cells[ii - 1, jj - 1].prevCell = new Point(ii, jj);
                    }
                }

                if (jj < cells.GetLength(1) - 1)
                {
                    if ((cells[ii - 1, jj + 1].cellCost == -1 || cells[ii - 1, jj + 1].cellCost > cells[ii, jj].cellCost + cells[ii - 1, jj + 1].stepCost) && cells[ii - 1, jj + 1].stepCost != -1)
                    {
                        cells[ii - 1, jj + 1].cellCost = cells[ii, jj].cellCost + cells[ii - 1, jj + 1].stepCost;
                        cells[ii - 1, jj + 1].prevCell = new Point(ii, jj);
                    }
                }

            }
            if (ii < cells.GetLength(0) - 1)
            {
                if ((cells[ii + 1, jj].cellCost == -1 || cells[ii + 1, jj].cellCost > cells[ii, jj].cellCost + cells[ii + 1, jj].stepCost) && cells[ii + 1, jj].stepCost != -1)
                {
                    cells[ii + 1, jj].cellCost = cells[ii, jj].cellCost + cells[ii + 1, jj].stepCost;
                    cells[ii + 1, jj].prevCell = new Point(ii, jj);
                }

                if (jj > 0)
                {
                    if ((cells[ii + 1, jj - 1].cellCost == -1 || cells[ii + 1, jj - 1].cellCost > cells[ii, jj].cellCost + cells[ii + 1, jj - 1].stepCost) && cells[ii + 1, jj - 1].stepCost != -1)
                    {
                        cells[ii + 1, jj - 1].cellCost = cells[ii, jj].cellCost + cells[ii + 1, jj - 1].stepCost;
                        cells[ii + 1, jj - 1].prevCell = new Point(ii, jj);
                    }
                }

                if (jj < cells.GetLength(1) - 1)
                {
                    if ((cells[ii + 1, jj + 1].cellCost == -1 || cells[ii + 1, jj + 1].cellCost > cells[ii, jj].cellCost + cells[ii + 1, jj + 1].stepCost) && cells[ii + 1, jj + 1].stepCost != -1)
                    {
                        cells[ii + 1, jj + 1].cellCost = cells[ii, jj].cellCost + cells[ii + 1, jj + 1].stepCost;
                        cells[ii + 1, jj + 1].prevCell = new Point(ii, jj);
                    }
                }
            }
            if (jj > 0)
            {
                if ((cells[ii, jj - 1].cellCost == -1 || cells[ii, jj - 1].cellCost > cells[ii, jj].cellCost + cells[ii, jj - 1].stepCost) && cells[ii, jj - 1].stepCost != -1)
                {
                    cells[ii, jj - 1].cellCost = cells[ii, jj].cellCost + cells[ii, jj - 1].stepCost;
                    cells[ii, jj - 1].prevCell = new Point(ii, jj);
                }
            }
            if (jj < cells.GetLength(1) - 1)
            {
                if ((cells[ii, jj + 1].cellCost == -1 || cells[ii, jj + 1].cellCost > cells[ii, jj].cellCost + cells[ii, jj + 1].stepCost) && cells[ii, jj + 1].stepCost != -1)
                {
                    cells[ii, jj + 1].cellCost = cells[ii, jj].cellCost + cells[ii, jj + 1].stepCost;
                    cells[ii, jj + 1].prevCell = new Point(ii, jj);
                }
            }
            int minCost = -1;
            int minDistance = 0;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].cellCost != -1 && !cells[i, j].isCheckeed)
                    {
                        if (minCost == -1 || 
                            (
                                (minCost > (cells[i, j].cellCost + cells[i, j].distanceToEnd) || (minCost == (cells[i, j].cellCost + cells[i, j].distanceToEnd) && minDistance > cells[i, j].distanceToEnd)))
                            )
                        {
                            ii = i;
                            jj = j;
                            minCost = cells[i, j].cellCost + cells[i, j].distanceToEnd;
                            minDistance = cells[i, j].distanceToEnd;
                        }
                    }
                }
            }
        }

        List<Point> points = new List<Point>();
        while (cells[ii, jj].cellCost != 0)
        {
            Point point = cells[ii, jj].prevCell;
            points.Add(point);
            ii = point.X;
            jj = point.Y;
        }
        return points;
    }

    static void StartPositionFromCells(Cell[,] cells, out int ii, out int jj)
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (cells[i, j].cellCost == 0)
                {
                    ii = i;
                    jj = j;
                    return;
                }
            }
        }
        ii = 0;
        jj = 0;
    }

    public class Cell
    {
        internal int stepCost;
        internal bool isEnd = false;
        internal int distanceToEnd;
        internal int cellCost = -1;
        internal bool isCheckeed = false;
        internal Point prevCell;
    }
}
