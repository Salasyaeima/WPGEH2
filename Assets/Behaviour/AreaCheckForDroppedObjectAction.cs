using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Area Check for dropped object", story: "Check [newAreaCheck] then assign Object [Object] and [Pos]", category: "Action", id: "b9f49d8f45904d33dfc714c01ac9eaa0")]
public partial class AreaCheckForDroppedObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<AreaCheck> NewAreaCheck;
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<Vector3> Pos;

    protected override Status OnUpdate()
    {
        NewAreaCheck.Value.CheckInRadius();
        if (NewAreaCheck.Value.DetectedTarget != null)
            Pos.Value = NewAreaCheck.Value.DetectedTarget.transform.position;
            Object.Value = NewAreaCheck.Value.DetectedTarget;
        return NewAreaCheck.Value.DetectedTarget != null ? Status.Success : Status.Failure;
    }
}

