using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Area Check", story: "Check [Target] inside [Detection] Range", category: "Action", id: "52fd494a34a394fa83138a98019aae26")]
public partial class AreaCheckAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<AreaCheck> Detection;

    protected override Status OnUpdate()
    {
        // Target.Value = Detection.Value.CheckInRadius();
        // return Target.Value != null ? Status.Success : Status.Failure;
        return Status.Failure;
    }
}

