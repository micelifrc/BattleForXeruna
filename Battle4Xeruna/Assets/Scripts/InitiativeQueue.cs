using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InitiativeQueue
{
    // The units in the queue are represented through their id's.
    private Queue<int> _queue;
    private int _bencher;  // this waits to be inserted next. -1 denotes that the bench is empty

    public InitiativeQueue(int initialSize, int reserverdSpace = 0)
    {
        _queue = new Queue<int>(reserverdSpace);
        for (int id = 0; id < initialSize; ++id)
            Insert(id);
        _bencher = -1;
    }

    // returns the id of the first Unit in the Queue. Returns -1 if no Unit is in the Queue
    public int GetFirst()
    {
        if (EmptyQueue())
        {
            _queue.Enqueue(_bencher);
            ResetBencher();
        }
        return _queue.Peek();
    }

    public void Insert(int idToInsert)
    {
        _queue.Enqueue(idToInsert);
    }

    // deletes all occurrences of idToDelete from the bench and the queue
    public void Delete(int idToDelete)
    {
        _queue = new Queue<int>(_queue.Where(id => id != idToDelete));
        if (_bencher == idToDelete)
            ResetBencher();
    }
    
    // rotates the queue, moving the first element of the queue to the last
    public void Rotate(bool putToBench)
    {
        if (EmptyQueue())
        {
            Debug.Log("Cannot rotate empty queue");
        }

        InsertBencher();
        if (putToBench)
        {
            _bencher = _queue.Peek();
        } 
        else
        {
            _queue.Enqueue(_queue.Peek());
        }
        _queue.Dequeue();
    }

    private bool EmptyQueue()
    {
        return _queue.Count == 0;
    }

    private bool BenchIsEmpty()
    {
        return _bencher == -1;
    }

    private void ResetBencher()
    {
        _bencher = -1;
    }

    private void InsertBencher()
    {
        if (!BenchIsEmpty())
        {
            _queue.Enqueue(_bencher);
            ResetBencher();
        }
    }
}
