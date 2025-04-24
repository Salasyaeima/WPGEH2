using UnityEngine;
using Unity.Behavior;

public class AITuneHandler : MonoBehaviour
{
    [SerializeField]
    private BehaviorGraph behavior;
    public float patrolDelay = 2f;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Threshold")]
    public float patrolThreshold = 2f;
    public float chaseThreshold = 3f;
    
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        behavior.BlackboardReference.SetVariableValue("Patrol Walkspeed", patrolSpeed);
        behavior.BlackboardReference.SetVariableValue("Chase Walkspeed", chaseSpeed);
        behavior.BlackboardReference.SetVariableValue("Patrol Delay", patrolDelay);
        behavior.BlackboardReference.SetVariableValue("Patrol Threshold", patrolThreshold);
        behavior.BlackboardReference.SetVariableValue("Chase Threshold", chaseThreshold);
    }
}
