using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * Utility class, containing all the extra functions and enums
 */
public static class Utils
{
    // main Colors, to represent players, units, buildings and terrein
    public enum Color{Blue, Red, Yellow, Green};

    // The ball centered in (xCenter,yCenter) with specified radius. Includes tiles that could eventually be out of map
    public static Vector2Int[] GetBall(int xCenter, int yCenter, int radius = 1) {
        Vector2Int[] ball = new Vector2Int[1+3*radius*(radius+1)];
        int nextIndex = 0;
        int dCenter = yCenter - xCenter;

        for (int x = xCenter - radius; x <= xCenter + radius; ++x)
            for (int y = yCenter - radius; y <= yCenter + radius; ++y)
                {
                    if (Math.Abs((y - x) - dCenter) <= radius)
                    {
                        ball[nextIndex] = new Vector2Int(x, y);
                        ++nextIndex;
                    }
                }
        
        // just a checker
        if (nextIndex != 1+3*radius*(radius+1)) 
            Debug.Log("GetBall with radius = " + radius + " has " + nextIndex +  " entries");
        return ball;
    }

    public static Vector2Int[] GetBall(Vector2Int coord, int radius = 1) {
        return GetBall(coord.x, coord.y, radius);
    }

    // returns the radius of the map, depending on the number of players
    public static int GetRadius(int numPlayers)
    {
        return numPlayers <= 2 ? 5 : 6;
    }

    // Tells whether the point (x, y) is on the Hexagonal map of required radius
    public static bool IsOnMap(int x, int y, int radius)
    {
        return x >= 0 && y >= 0 && x <= 2*radius && y <= 2*radius && x - y <= radius && y - x <= radius;
    }
    public static bool IsOnMap(Vector2Int coord, int radius)
    {
        return IsOnMap(coord.x, coord.y, radius);
    }

    // returns the grid-distance between (xFrom,yFrom) and (xTo,yTo)
    public static int GridDistance(int xFrom, int yFrom, int xTo, int yTo)
    {
        int dx = xTo - xFrom, dy = yTo - yFrom;
        if ((dx >= 0 && dy <= 0) || (dx <= 0 && dy >= 0))
            return Math.Abs(dx) + Math.Abs(dy);
        else 
            return Math.Max(Math.Abs(dx), Math.Abs(dy));
    }
    public static int GridDistance(Vector2Int from, Vector2Int to)
    {
        return GridDistance(from.x, from.y, to.x, to.y);
    }
}
