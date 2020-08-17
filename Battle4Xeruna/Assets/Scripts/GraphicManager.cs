using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GraphicManager : MonoBehaviour
{
    private int _numPlayers; // the number of players TODO should be private ad be set from Host
    private int _radius;
    // adjust rotate the objects to make them compatible with the camera
    private Quaternion _basicCameraRotation{get{return Quaternion.Euler(-90,0,0);}}

    // each array has 2 entries. [0] is for opaque material, [1] is for the bright material
    public Material[] GrassMaterial;
    public Material[] BlueMaterial;
    public Material[] RedMaterial;
    public Material[] YellowMaterial;
    public Material[] EarthMaterial;
    public Material[] StoneMaterial;
    
    // Each array will contain one entry per player
    public GameObject Castle;
    private Vector2Int[] _castlePositions;
    private Quaternion[] _castleRotations;
    private GameObject[] _castles;  // Will contain some Castle instances

    public GameObject TilePrefab;
    private Tile[,] _tiles;  // will contain the _tiles

    void Start()
    {
        SubscribeEvents();
        _numPlayers = FindObjectOfType<GameManager>()._numPlayers;
        if (_numPlayers != 2 && _numPlayers !=3)
        {
            Debug.Log("Invalid number of players = " + _numPlayers);
            return;
        }
        _radius =  FindObjectOfType<GameManager>()._radius;

        MoveCamera();
        CreateCastles();
        CreateTiles();
    }

    private void SubscribeEvents()
    {
        EventManager.main.OnLeftClick += TestingOnLeftClick;
    }

    // TODO delete
    private void TestingOnLeftClick(object sender, EventManager.MapCoordEventArgs args) {
        _tiles[args.mapCoord.x, args.mapCoord.y]?.switchLight();
    }

    // The camera should be moved differently for different players
    private void MoveCamera()
    {
        Camera camera = Camera.main;
        if (_numPlayers <= 2)
            camera.transform.position = new Vector3(0,16.8f,-3.9f);
        else
            camera.transform.position = new Vector3(0,19.6f,-4.8f);
        camera.transform.eulerAngles = new Vector3(80,0,0);
    }

    // Create the _castles
    private void CreateCastles()
    {
        _castlePositions = new Vector2Int[_numPlayers];
        _castleRotations = new Quaternion[_numPlayers];
        _castles = new GameObject[_numPlayers];
        switch (_numPlayers)
        {
            case 2:
                _castlePositions[0] = new Vector2Int(9,9);
                _castlePositions[1] = new Vector2Int(1,1);
                _castleRotations[0] = Quaternion.Euler(0,0,0);
                _castleRotations[1] = Quaternion.Euler(0,180,0);
                break;
            case 3:
                _castlePositions[0] = new Vector2Int(11,6);
                _castlePositions[1] = new Vector2Int(1,1);
                _castlePositions[2] = new Vector2Int(6,11);
                _castleRotations[0] = Quaternion.Euler(0,60,0);
                _castleRotations[1] = Quaternion.Euler(0,180,0);
                _castleRotations[2] = Quaternion.Euler(0,300,0);
                break;
        }
        for (int castleIdx = 0; castleIdx < _numPlayers; ++castleIdx)
            _castles[castleIdx] = Instantiate(Castle, Utils.ToWorldCoord(_castlePositions[castleIdx]), _castleRotations[castleIdx] * _basicCameraRotation);
    }

    // Create the _tiles
    private void CreateTiles()
    {
        _tiles = new Tile[1+2*_radius,1+2*_radius];

        Vector2Int[] BlueCastle = Utils.GetBall(_castlePositions[0]);
        Vector2Int[] RedCastle = Utils.GetBall(_castlePositions[1]);
        Vector2Int[] YellowCastle = _numPlayers > 2 ? Utils.GetBall(_castlePositions[2]) : null;

        for (int x = 0; x <= 2*_radius; ++x)
            for (int y = 0; y <= 2*_radius; ++y)
                if (IsOnMap(x, y))
                    CreateTile(x, y);
    }

    private void CreateTile(int x, int y)
    {
        Material[] materialsTop = GrassMaterial;
        if (Utils.GridDistance(_castlePositions[0].x, _castlePositions[0].y, x, y) <= 1)
            materialsTop = BlueMaterial;
        else if (Utils.GridDistance(_castlePositions[1].x, _castlePositions[1].y, x, y) <= 1)
            materialsTop = RedMaterial;
        else if (_numPlayers > 2 && Utils.GridDistance(_castlePositions[2].x, _castlePositions[2].y, x, y) <= 1)
            materialsTop = YellowMaterial;
        Material[] materialsBottom = materialsTop == GrassMaterial ? EarthMaterial : StoneMaterial;

        GameObject go = Instantiate(TilePrefab) as GameObject;
        _tiles[x, y] = go.GetComponent<Tile>();
        _tiles[x, y].Initialize(Utils.ToWorldCoord(x, y), Quaternion.Euler(0,90,0) * _basicCameraRotation, materialsTop, materialsBottom);
    }

    // Tells whether (x, y) is on the map (as Map coordinates)
    private bool IsOnMap(int x, int y)
    {
        return Utils.IsOnMap(x, y);
    }
    private bool IsOnMap(Vector2Int coord)
    {
        return Utils.IsOnMap(coord);
    }
}