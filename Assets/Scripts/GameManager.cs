using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickableObject
{
    public string objectType;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Queue<string> turnQueue = new Queue<string>();
    public List<PickableObject> pickableObjects = new List<PickableObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeTurnQueue();
    }

    void InitializeTurnQueue()
    {
        foreach (var obj in pickableObjects)
        {
            turnQueue.Enqueue(obj.objectType);
        }
    }

    public string GetCurrentTurn()
    {
        return turnQueue.Peek();
    }

    public void SwitchTurn()
    {
        if (turnQueue.Count > 0)
        {
            string currentTurn = turnQueue.Dequeue();
            turnQueue.Enqueue(currentTurn);
            Debug.Log("Giliran sekarang: " + GetCurrentTurn());
        }
    }

}
