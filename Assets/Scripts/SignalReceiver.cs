using UnityEngine;

public class SignalReceiver : MonoBehaviour
{
    [SerializeField] TargetWalk targetWalk;

    public void MoveToWaypoint(int waypointIndex = -1)
    {
        if (targetWalk != null)
        {
            targetWalk.StartMovingToWaypoint(waypointIndex);
        }
        else
        {
            Debug.LogError("TargetWalk is not assigned in SignalReceiver!");
        }
    }

    public void StopAutoMove()
    {
        if (targetWalk != null)
        {
            targetWalk.StopAutoMove();
        }
        else
        {
            Debug.LogError("TargetWalk is not assigned in SignalReceiver!");
        }
    }
}
