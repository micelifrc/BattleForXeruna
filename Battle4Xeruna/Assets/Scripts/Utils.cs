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

    // scaling factor for the World
    public static Vector2 WorldScale { get { return new Vector2(1.5f, 0.866f); } }

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

    public static Vector3 toWorldCoord(int x, int y)
    {
        return new Vector3(WorldScale.x*(y-x), WorldScale.y*(x+y), 0.0f);
    }

    public static Vector3 toWorldCoord(Vector2Int coord)
    {
        return toWorldCoord(coord.x, coord.y);
    }
}
