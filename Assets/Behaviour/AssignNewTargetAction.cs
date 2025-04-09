using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Assign new Target", story: "Assign [newLineOfSight] [Target] when Hiding", category: "Action", id: "9b2c58307d3e775963fb9c298990fef3")]
public partial class AssignNewTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<LineOfSight> NewLineOfSight;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        if (NewLineOfSight.Value.DetectedTarget != null)
        {
            if (Target.Value == NewLineOfSight.Value.DetectedTarget.transform.parent.gameObject)
            {
                return Status.Success;
            }
            else if (NewLineOfSight.Value.DetectedTarget.CompareTag(NewLineOfSight.Value.tagAfter))
            {
                Target.Value = NewLineOfSight.Value.DetectedTarget.transform.parent.gameObject;
                return Status.Success;
            }else
            {
                return Status.Failure;
            }
        }
        return Status.Failure;
    }
}

