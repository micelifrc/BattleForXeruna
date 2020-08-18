using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * An abstract class to represent the unit
 */
public abstract class Unit
{
    public bool isActive = false;  // only true during the turn of the unit
    protected int _id;
    public int Id{get{return _id;}}

    protected Vector2Int _mapPosition;
    protected Utils.PolarDirection _direction;

    protected int _movementPoints;
    public int MovementPoints{get{return _movementPoints;}}
    protected int _lifePoints;
    public int LifePoints{get{return _lifePoints;}}

    public virtual int Cost{get{return 0;}}
    public virtual int MovementPointsMax{get{return 0;}}
    public virtual int LifePointsMax{get{return 0;}}
    public virtual int RestingPoints{get{return 0;}}
    public virtual int DefencePoints{get{return 0;}}
    public virtual int CoveragePoints{get{return 0;}}
    public virtual int AttackPoints{get{return 0;}}
    public virtual int SneakPoints{get{return 0;}}
    public virtual int BuildingPoints{get{return 0;}}
    public virtual int TaxingPoints{get{return 0;}}
    public virtual bool CanAttack{get{return true;}}
    public virtual bool HasAOC{get{return true;}}

    protected GameManager _gm;

    // tell whether the Unit has already performed some action during this turn, and whether the action was an heavy one
    private bool _hasPerformedHeavyAction;

    protected void Update()
    {
        if (!isActive)
            return;  // cannot use the Unit while it is not active
        // This should allow the player to perform actions with the active unit
    }

    // What the Unit does at the start of its turn. Will be overridden by some derived Units
    public virtual void PerformInitialPhaseActions()
    {
        _hasPerformedHeavyAction = false;
    }

    // What the Unit does at the end of its turn. Will be overridden by some derived Units
    public virtual void PerformEndingPhaseActions()
    {
        isActive = false;
        _gm.initiativeQueue.Rotate(_hasPerformedHeavyAction);
    }

    public bool MoveToDirection(Utils.PolarDirection dir)
    {
        if (_movementPoints <= 0)
        {
            Debug.Log("Cannot move. Insufficient movementPoints");
            return false;
        }

        Vector2Int newMapPosition = _mapPosition + Utils.ToCoordIncrement(dir);
        if (!Utils.IsOnMap(newMapPosition))
        {
            Debug.Log("Cannot move out of the map");
            return false;
        }

        --_movementPoints;
        _direction = dir;
        _mapPosition = newMapPosition;
        return true;
    }

    public void Rotate(Utils.PolarDirection dir)
    {
        _direction = dir;
    }

    public virtual void Heal(int healingPoints)
    {
        if (healingPoints < 0)
        {
            Debug.Log("Cannot heal negative number of heal points");
            return;
        }
        _lifePoints = Math.Min(_lifePoints + healingPoints, LifePointsMax);
    }

    public virtual void Damage(int damagePoints)
    {
        if (damagePoints <= 0)
        {
            Debug.Log("Cannot take negative damage");
            return;
        }
        _lifePoints -= damagePoints;
        if (_lifePoints <= 0)
            Die();
    }

    public virtual void Die()
    {
        // TODO currently nothing happens
    }
}
