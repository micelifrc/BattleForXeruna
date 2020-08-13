using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class keeps data about the map
 */
public class FieldManager
{
    private int _numPlayers{get; set;}
    private int _radius{get; set;}

    private TerrainObject[,] _terrains;
    private Building[,] _buildings;
    private Unit[,] _units;

    // constructor
    public FieldManager(int numPlayers_)
    {
        _numPlayers = numPlayers_;
        _radius = Utils.GetRadius(numPlayers_);
        _terrains = new TerrainObject[2*_radius+1, 2*_radius+1];
        _buildings = new Building[2*_radius+1, 2*_radius+1];
        _units = new Unit[2*_radius+1, 2*_radius+1];

        BuildCastles();
        InitializeTerrains();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public bool IsOnMap(int x, int y)
    {
        return Utils.IsOnMap(x, y, _radius);
    }

    public bool IsOnMap(Vector2Int coord)
    {
        return IsOnMap(coord.x, coord.y);
    }

    private void BuildCastles()
    {
        // build the blue castle
        Vector2Int[] castle = _numPlayers <= 2? Utils.GetBall(9,9) : Utils.GetBall(11,6);
        foreach (Vector2Int coord in castle)
            _buildings[coord.x,coord.y] = new Castle(Utils.Color.Blue);

        castle = Utils.GetBall(1,1);
        foreach (Vector2Int coord in castle)
            _buildings[coord.x,coord.y] = new Castle(Utils.Color.Red);

        if (_numPlayers > 2)
        {
            castle = Utils.GetBall(6,11);
            foreach (Vector2Int coord in castle)
                _buildings[coord.x,coord.y] = new Castle(Utils.Color.Yellow);
        }
    }

    private void InitializeTerrains()
    {
        for (int x = 0; x <= 2*_radius; ++x)
            for (int y = 0; y <= 2*_radius; ++y)
                if (IsOnMap(x, y))
                    _terrains[x, y] = new TerrainObject(_buildings[x,y]==null? Utils.Color.Green : _buildings[x,y].color);
    }
}

public class TerrainObject
{
    public Utils.Color color;
    public bool isLighted;

    public TerrainObject(Utils.Color color_, bool isLighted_ = false)
    {
        SetColor(color_, false);
    }

    public void SetColor(Utils.Color color_, bool isLighted_ = false)
    {
        color = color_;
        isLighted = isLighted_;
    }
}