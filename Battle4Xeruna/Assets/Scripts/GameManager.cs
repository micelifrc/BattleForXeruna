using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int _numPlayers; // the number of players TODO should be private ad be set from Host
    public int _radius;  // will also be set by the players
    public FieldManager fieldManager;
    public InitiativeQueue initiativeQueue;

    // Start is called before the first frame update
    void Start()
    {
        _radius = _numPlayers <= 2 ? 5 : 6;
        Utils.setNumPlayers(_numPlayers, _radius);
        fieldManager = new FieldManager();
        initiativeQueue = new InitiativeQueue(_numPlayers, 128);
    }

    /* Update is called once per frame
    void Update()
    {
        
    }*/

    public int GetNumPlayers()
    {
        return _numPlayers;
    }
}
