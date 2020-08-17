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

    // the scaling factor of the map
    public static Vector2 worldScale { get { return new Vector2(1.5f, 0.866f); } }

    public static int numPlayers;
    public static int radius;

    public static void setNumPlayers(int numPlayers_, int radius_)
    {
        numPlayers = numPlayers_;
        radius =  radius_;
    }

    // The ball centered in (xCenter,yCenter) with specified ballRadius. Includes tiles that could eventually be out of map
    public static Vector2Int[] GetBall(int xCenter, int yCenter, int ballRadius = 1) {
        Vector2Int[] ball = new Vector2Int[1+3*ballRadius*(ballRadius+1)];
        int nextIndex = 0;
        int dCenter = yCenter - xCenter;

        for (int x = xCenter - ballRadius; x <= xCenter + ballRadius; ++x)
            for (int y = yCenter - ballRadius; y <= yCenter + ballRadius; ++y)
                {
                    if (Math.Abs((y - x) - dCenter) <= ballRadius)
                    {
                        ball[nextIndex] = new Vector2Int(x, y);
                        ++nextIndex;
                    }
                }
        
        // just a checker
        if (nextIndex != 1+3*ballRadius*(ballRadius+1)) 
            Debug.Log("GetBall with ballRadius = " + ballRadius + " has " + nextIndex +  " entries");
        return ball;
    }

    public static Vector2Int[] GetBall(Vector2Int coord, int ballRadius = 1) {
        return GetBall(coord.x, coord.y, ballRadius);
    }

    // Tells whether the point (x, y) is on the Hexagonal map of required ballRadius
    public static bool IsOnMap(int x, int y)
    {
        return x >= 0 && y >= 0 && x <= 2*radius && y <= 2*radius && x - y <= radius && y - x <= radius;
    }
    public static bool IsOnMap(Vector2Int coord)
    {
        return IsOnMap(coord.x, coord.y);
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

    // Convert from Map coordinates to World coordinates
    public static  Vector3 ToWorldCoord(int x, int y)
    {
        return new Vector3(worldScale.x*(y-x), -0.025f, worldScale.y*(x+y-2*radius));
    }
    public static  Vector3 ToWorldCoord(Vector2Int coord)
    {
        return ToWorldCoord(coord.x, coord.y);
    }

    // Convert from Map coordinates to World coordinates
    public static Vector2Int ToMapCoord(Vector3 worldCoord)
    {
        int x = (int)((double)(-worldCoord.x*worldScale.y+worldCoord.z*worldScale.x)/(2*worldScale.x*worldScale.y)+radius+0.5);
        int y = (int)((double)(+worldCoord.x*worldScale.y+worldCoord.z*worldScale.x)/(2*worldScale.x*worldScale.y)+radius+0.5);
        return new Vector2Int(x, y);
    }
}
