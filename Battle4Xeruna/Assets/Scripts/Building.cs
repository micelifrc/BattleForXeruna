using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// abstract class to represent a Building. House and Castle inherit from it!
public abstract class Building
{
    public enum BuildingType {House, Castle};

    public BuildingType buildingType;
    public Utils.Color color;

    public virtual void setColor(Utils.Color color_)
    {
        color = color_;
    }
}

public class House : Building {
    public House(Utils.Color color_)
    {
        buildingType = BuildingType.House;
        setColor(color_);
    }
}

public class Castle : Building {
    public Castle(Utils.Color color_)
    {
        buildingType = BuildingType.Castle;
        setColor(color_);
    }
}