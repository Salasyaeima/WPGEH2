using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Line of Sight Detection", story: "Check Line of Sight [Detector] then assign [Target]", category: "Action", id: "57871082d42719d2d15a16bb3839167f")]
public partial class LineOfSightDetectionAction : Action
{
    [SerializeReference] public BlackboardVariable<LineOfSight> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = Detector.Value.CheckInSight(Target.Value);
        return Target.Value != null ? Status.Success: Status.Failure;
    }
}

