using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Perform Caught", story: "Perform [Caught] when Agent caught the Target", category: "Action", id: "e4b8adfbd38c8075c51b1c5745460d0f")]
public partial class PerformCaughtAction : Action
{
    [SerializeReference] public BlackboardVariable<CaughtHandler> Caught;

    protected override Status OnUpdate()
    {
        Caught.Value.PerformCaught();
        return Status.Success;
    }
}

