using UnityEngine;

public class SignalReceiver : MonoBehaviour
{
    [SerializeField] TargetWalk targetWalk;

    public void MoveToWaypoint(int waypointIndex = -1)
    {
        if (targetWalk != null)
        {
            targetWalk.StartMovingToWaypoint(waypointIndex);
            Debug.Log($"Signal received: Moving to waypoint {(waypointIndex >= 0 ? waypointIndex : targetWalk.GetCurrentWaypoint())}");
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
            Debug.Log("Signal received: Stopping auto move");
        }
        else
        {
            Debug.LogError("TargetWalk is not assigned in SignalReceiver!");
        }
    }
}