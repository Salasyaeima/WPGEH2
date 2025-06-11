using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Random Patrol Waypoints", story: "Set [Waypoint] by Random [Waypoints]", category: "Action", id: "e808c00dc555cc9414a88bf7b47622da")]
public partial class RandomPatrolWaypointsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Waypoint;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    protected override Status OnUpdate()
    {
        int randomNum = UnityEngine.Random.Range(0, Waypoints.Value.Count);
        Waypoint.Value = Waypoints.Value[randomNum];
        return Status.Success;
    }
}

