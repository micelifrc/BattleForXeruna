using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphicManager : MonoBehaviour
{
    private int _numPlayers; // the number of players TODO should be private ad be set from Host
    private int _radius;
    // adjust rotate the objects to make them compatible with the camera
    private Quaternion _basicCameraRotation{get{return Quaternion.Euler(-90,0,0);}}
    // scaling factor for the World
    private static Vector2 _worldScale { get { return new Vector2(1.5f, 0.866f); } }

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

    // An exagonal tile
    public GameObject Tile;
    public struct TileObject {
        public GameObject obj;
        public Material[] materialsTop, materialsBottom; // 2 entries each
        bool isLighted;

        public TileObject(GameObject TileObj, Vector3 traslation, Quaternion rotation, Material[] materialsTop_, Material[] materialsBottom_, bool isLighted_ = false)
        {
            obj = Instantiate(TileObj, traslation, rotation);
            materialsTop = materialsTop_;
            materialsBottom = materialsBottom_;
            isLighted = isLighted_;
            SetMaterials();
        }
        
        public void SetMaterials()
        {
            int lightedIdx = isLighted ? 1 : 0;
            obj.GetComponent<MeshRenderer>().materials = new Material[2] {materialsTop[lightedIdx], materialsBottom[lightedIdx]};
        }

        public void setLight(bool isLighted_)
        {
            isLighted = isLighted_;
            SetMaterials();
        }
    }
    private TileObject[,] _tiles;  // will contain the _tiles

    void Start()
    {
        _numPlayers = FindObjectOfType<GameManager>()._numPlayers;
        if (_numPlayers != 2 && _numPlayers !=3)
        {
            Debug.Log("Invalid number of players = " + _numPlayers);
            return;
        }
        _radius = Utils.GetRadius(_numPlayers);

        MoveCamera();
        CreateCastles();
        CreateTiles();
    }

    // The camera should be moved differently for different players
    private void MoveCamera()
    {
        Camera camera = Camera.main;
        if (_numPlayers <= 2)
            camera.transform.position = new Vector3(0,12,-10);
        else
            camera.transform.position = new Vector3(0,13.5f,-11.5f);
        camera.transform.eulerAngles = new Vector3(60,0,0);
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
            _castles[castleIdx] = Instantiate(Castle, ToWorldCoord(_castlePositions[castleIdx]), _castleRotations[castleIdx] * _basicCameraRotation);
    }

    // Create the _tiles
    private void CreateTiles()
    {
        _tiles = new TileObject[1+2*_radius,1+2*_radius];

        Vector2Int[] BlueCastle = Utils.GetBall(_castlePositions[0]);
        Vector2Int[] RedCastle = Utils.GetBall(_castlePositions[1]);
        Vector2Int[] YellowCastle = _numPlayers > 2 ? Utils.GetBall(_castlePositions[2]) : null;

        for (int x = 0; x <= 2*_radius; ++x)
            for (int y = 0; y <= 2*_radius; ++y)
                if (IsOnMap(x, y))
                {
                    Material[] materialsTop = GrassMaterial;
                    if (Utils.GridDistance(_castlePositions[0].x, _castlePositions[0].y, x, y) <= 1)
                        materialsTop = BlueMaterial;
                    else if (Utils.GridDistance(_castlePositions[1].x, _castlePositions[1].y, x, y) <= 1)
                        materialsTop = RedMaterial;
                    else if (_numPlayers > 2 && Utils.GridDistance(_castlePositions[2].x, _castlePositions[2].y, x, y) <= 1)
                        materialsTop = YellowMaterial;
                    Material[] materialsBottom = materialsTop == GrassMaterial ? EarthMaterial : StoneMaterial;
                    _tiles[x, y] = new TileObject(Tile, ToWorldCoord(x, y), Quaternion.Euler(0,90,0) * _basicCameraRotation, materialsTop, materialsBottom);
                }
    }

    // Convert from Map coordinates to World coordinates
    private Vector3 ToWorldCoord(int x, int y)
    {
        return new Vector3(_worldScale.x*(y-x), 0.0f, _worldScale.y*(x+y-2*_radius));
    }
    private Vector3 ToWorldCoord(Vector2Int coord)
    {
        return ToWorldCoord(coord.x, coord.y);
    }

    // Convert from Map coordinates to World coordinates
    // TODO need testing
    private Vector2Int ToMapCoord(Vector3 worldCoord)
    {
        int x = (int)((double)(-worldCoord.x*_worldScale.y+worldCoord.y*_worldScale.x+2*_radius*_worldScale.x*_worldScale.y)/(4*_worldScale.x*_worldScale.y*_worldScale.y)+0.5);
        int y = (int)((double)(+worldCoord.x*_worldScale.y+worldCoord.y*_worldScale.x+2*_radius*_worldScale.x*_worldScale.y)/(4*_worldScale.x*_worldScale.y*_worldScale.y)+0.5);
        return new Vector2Int(x, y);
    }

    // Tells whether (x, y) is on the map (as Map coordinates)
    private bool IsOnMap(int x, int y)
    {
        return Utils.IsOnMap(x, y, _radius);
    }
    private bool IsOnMap(Vector2Int coord)
    {
        return IsOnMap(coord.x, coord.y);
    }
}