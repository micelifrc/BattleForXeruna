using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Todo make it a singleton
public class GameManager : MonoBehaviour
{
    public FieldManager fieldManager;

    // Start is called before the first frame update
    void Start()
    {
        fieldManager = new FieldManager(numPlayers());
    }

    /* Update is called once per frame
    void Update()
    {
        
    }*/

    // TODO can also return 3 in the future
    public int numPlayers()
    {
        return 2;
    }
}
