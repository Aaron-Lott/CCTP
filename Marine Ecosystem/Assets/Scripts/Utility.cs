using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region Bresenham 3D
    public static List<Vector3> Bresenham3D(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        List<Vector3> listOfPoints = new List<Vector3>();

        int dx = System.Math.Abs(x2 - x1);
        int dy = System.Math.Abs(y2 - y1);
        int dz = System.Math.Abs(z2 - z1);

        int xs, ys, zs;

        if (x2 > x1)
        {
            xs = 1;
        }
        else
        {
            xs = -1;
        }
        if (y2 > y1)
        {
            ys = 1;
        }
        else
        {
            ys = -1;
        }
        if (z2 > z1)
        {
            zs = 1;
        }
        else
        {
            zs = -1;
        }

        if (dx >= dy && dx >= dz)
        {
            int p1 = 2 * dy - dx;
            int p2 = 2 * dz - dx;

            while (x1 != x2)
            {
                x1 += xs;
                if (p1 >= 0)
                {
                    y1 += ys;
                    p1 -= 2 * dx;
                }
                if (p2 >= 0)
                {
                    z1 += zs;
                    p2 -= 2 * dx;
                }
                p1 += 2 * dy;
                p2 += 2 * dz;
                listOfPoints.Add(new Vector3(x1, y1, z1));
            }
        }
        else if (dy >= dx && dy >= dz)
        {
            int p1 = 2 * dx - dy;
            int p2 = 2 * dz - dy;

            while (y1 != y2)
            {
                y1 += ys;
                if (p1 >= 0)
                {
                    x1 += xs;
                    p1 -= 2 * dy;
                }
                if (p2 >= 0)
                {
                    z1 += zs;
                    p2 -= 2 * dy;
                }
                p1 += 2 * dx;
                p2 += 2 * dz;
                listOfPoints.Add(new Vector3(x1, y1, z1));
            }
        }
        else
        {
            int p1 = 2 * dy - dz;
            int p2 = 2 * dx - dz;

            while (z1 != z2)
            {
                z1 += zs;
                if (p1 >= 0)
                {
                    y1 += ys;
                    p1 -= 2 * dz;
                }
                if (p2 >= 0)
                {
                    x1 += xs;
                    p2 -= 2 * dz;
                }
                p1 += 2 * dy;
                p2 += 2 * dx;
                listOfPoints.Add(new Vector3(x1, y1, z1));
            }
        }

        return listOfPoints;
    }
    #endregion
}