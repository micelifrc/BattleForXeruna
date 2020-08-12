using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class keeps data about the map
 */
public class FieldManager
{
    private int NumPlayers{get; set;}
    private int Radius{get; set;}

    private TerrainObject[,] terrains;
    private Building[,] buildings;
    private Unit[,] units;

    // constructor
    public FieldManager(int numPlayers_)
    {
        NumPlayers = numPlayers_;
        Radius = NumPlayers <= 2 ? 5 : 6;
        terrains = new TerrainObject[2*Radius+1, 2*Radius+1];
        buildings = new Building[2*Radius+1, 2*Radius+1];
        units = new Unit[2*Radius+1, 2*Radius+1];

        buildCastles();
        initializeTerrains();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public bool isOnMap(int x, int y)
    {
        return x >= 0 && y >= 0 && x <= 2*Radius && y <= 2*Radius && x - y <= Radius && y - x <= Radius;
    }

    public bool isOnMap(Vector2Int coord)
    {
        return isOnMap(coord.x, coord.y);
    }

    private void buildCastles()
    {
        // build the blue castle
        Vector2Int[] castle = NumPlayers <= 2? Utils.GetBall(9,9) : Utils.GetBall(11,6);
        foreach (Vector2Int coord in castle)
            buildings[coord.x,coord.y] = new Castle(Utils.Color.Blue);

        castle = Utils.GetBall(1,1);
        foreach (Vector2Int coord in castle)
            buildings[coord.x,coord.y] = new Castle(Utils.Color.Red);

        if (NumPlayers > 2)
        {
            castle = Utils.GetBall(6,11);
            foreach (Vector2Int coord in castle)
                buildings[coord.x,coord.y] = new Castle(Utils.Color.Yellow);
        }
    }

    private void initializeTerrains()
    {
        for (int x = 0; x <= 2*Radius; ++x)
            for (int y = 0; y <= 2*Radius; ++y)
                if (isOnMap(x, y))
                    terrains[x, y] = new TerrainObject(buildings[x,y]==null? Utils.Color.Green : buildings[x,y].color);
    }
}

public class TerrainObject
{
    public Utils.Color color;
    public bool IsLighted {set; get;}

    public TerrainObject(Utils.Color color_, bool isLighted_ = false)
    {
        setColor(color_, false);
    }

    public void setColor(Utils.Color color_, bool isLighted_ = false)
    {
        color = color_;
        IsLighted = isLighted_;
    }
}