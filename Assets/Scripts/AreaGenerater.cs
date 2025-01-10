using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static PathGenerater;

public static class AreaGenerater
{
    static void GetPieceOfMap(int[,] map, int x, int y, int radius, out int[,] newMap, out int[,] newMiniMap, out int x1, out int y1)
    {
        x1 = x - radius >= 0 ? x - radius : 0;
        int x2 = x + radius < map.GetLength(0) ? x + radius : map.GetLength(0) - 1;

        y1 = y - radius >= 0 ? y - radius : 0;
        int y2 = y + radius < map.GetLength(1) ? y + radius : map.GetLength(1) - 1;

        newMiniMap = new int[x2 - x1 + 1, y2 - y1 + 1];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (i == x && j == y)
                {
                    map[i, j] = 2;
                }
                if (i < x1 || i > x2 || j < y1 || j > y2)
                {
                    map[i, j] = 1;
                }
                else
                {
                    newMiniMap[i - x1, j - y1] = map[i, j];
                }
            }
        }
        newMap = map;
    }

    public static int[,] GetEnableArea(int[,] map, int x, int y, int radius, out Cell[,] cells)
    {
        int[,] miniMap;
        int x1, y1;
        GetPieceOfMap(map, x, y, radius, out map, out miniMap, out x1, out y1);
        cells = PathGenerater.GetCellsFromMap(miniMap);
        cells = Optimaze(cells);
        for (int i = 0; i < miniMap.GetLength(0); i++)
        {
            for (int j = 0; j < miniMap.GetLength(1); j++)
            {
                if (cells[i, j].cellCost > radius * 10 || cells[i, j].cellCost == -1)
                    map[i + x1, j + y1] = 1;
            }
        }

        return map;
    }
    public static Cell[,] Optimaze(Cell[,] cells)
    {
        int ii = 0;
        int jj = 0;
        string a1, a2, a3, a4;
        StartPositionFromCells(cells, out ii, out jj);
        while (!cells[ii, jj].isCheckeed)
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
            int min = -1;
            for (int i = 0;i < cells.GetLength(0); i++)
            {
                for (int j = 0;j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].cellCost != -1 && !cells[i, j].isCheckeed)
                    {
                        if (min == -1 || min > cells[i, j].cellCost)
                        {
                            ii = i;
                            jj = j;
                            min = cells[i, j].cellCost;
                        }
                    }
                }
            }
        }

        return cells;
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
}
