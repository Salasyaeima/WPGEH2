using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Waypoints based on State", story: "Set [Waypoint] from [Place] with Index [IndexofItems]", category: "Action", id: "546768401b3d83c9a4a0a9902f5f6aee")]
public partial class SetWaypointsBasedOnStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Waypoint;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Place;
    [SerializeReference] public BlackboardVariable<int> IndexofItems;

    protected override Status OnUpdate()
    {
        Waypoint.Value = Place.Value[IndexofItems];
        return Status.Success;
    }
}

