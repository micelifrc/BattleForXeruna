using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class keeps data about the map
 */
public class FieldManager
{
    private int _numPlayers;
    private int _radius;

    private Building[,] _buildings;
    private Unit[,] _units;

    // constructor
    public FieldManager()
    {
        _numPlayers = Utils.numPlayers;
        _radius = Utils.radius;
        _buildings = new Building[2*_radius+1, 2*_radius+1];
        _units = new Unit[2*_radius+1, 2*_radius+1];

        BuildCastles();
    }

    // Tells whether the point (x, y) is on the map
    public bool IsOnMap(int x, int y)
    {
        return Utils.IsOnMap(x, y);
    }
    public bool IsOnMap(Vector2Int coord)
    {
        return Utils.IsOnMap(coord);
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
}