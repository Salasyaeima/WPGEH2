using UnityEngine;
using Unity.Behavior;

public class AISpeedHandler : MonoBehaviour
{
    public float patrolSpeed = 2;
    public float chaseSpeed = 4;
    [SerializeField]
    private BehaviorGraph behavior;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        behavior.BlackboardReference.SetVariableValue("Patrol Walkspeed", patrolSpeed);
        behavior.BlackboardReference.SetVariableValue("Chase Walkspeed", chaseSpeed);
    }
}
