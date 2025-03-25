using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Target", story: "Is [Target] not NULL", category: "Conditions", id: "bd940bc1225d7fe2758eaa8f56939a77")]
public partial class CheckTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        return Target.Value != null ? true: false;
    }
}
