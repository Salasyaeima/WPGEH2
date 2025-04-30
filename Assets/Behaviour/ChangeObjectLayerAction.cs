using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change Object Layer", story: "Change [Object] Layer to [Something]", category: "Action", id: "720a1efdd20d25c8f9ca0d526e167d5c")]
public partial class ChangeObjectLayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<string> Something;

    protected override Status OnUpdate()
    {
        if (Object.Value != null)
            Object.Value.layer = LayerMask.NameToLayer(Something);
        return Status.Success;
    }
}

