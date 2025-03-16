using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Line ofSight", story: "Check Line of Sight [Detector] then assign [Target]", category: "Conditions", id: "2d1cb5c375c92eff5a5b562dc788d88d")]
public partial class CheckLineOfSightCondition : Condition
{
    [SerializeReference] public BlackboardVariable<LineOfSight> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        return Detector.Value.CheckInSight(Target.Value) ? true : false;
    }
}
