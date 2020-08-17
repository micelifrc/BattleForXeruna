using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// singleton class to transmit events
public class EventManager : MonoBehaviour
{
    public static EventManager main;

    // add the position of the mouse on the Map (in Map coordinates)
    public class MapCoordEventArgs : EventArgs
    {
        public Vector2Int mapCoord;
    }
    public event EventHandler<MapCoordEventArgs> OnLeftClick;

    private void Awake()
    {
        main = this;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckLeftMouseButtonDown();
    }
    
    private void  CheckLeftMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))  // if we left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // a projective ray from the point where we clicked
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))  // if we hit something
                if (hit.transform)  // check if the hit exists
                    OnLeftClick?.Invoke(this, new MapCoordEventArgs {mapCoord = Utils.ToMapCoord(hit.point)});
        }
    }
}
