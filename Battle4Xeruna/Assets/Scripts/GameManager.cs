using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int _numPlayers; // the number of players TODO should be private ad be set from Host
    public FieldManager fieldManager;

    // Start is called before the first frame update
    void Start()
    {
        fieldManager = new FieldManager(GetNumPlayers());
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
