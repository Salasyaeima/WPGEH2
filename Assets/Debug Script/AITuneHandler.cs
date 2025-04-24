using UnityEngine;
using Unity.Behavior;

public class AITuneHandler : MonoBehaviour
{
    [SerializeField]
    private BehaviorGraph behavior;
    public float patrolDelay = 2;
    [Header("Walkspeed")]
    public float patrolSpeed = 2;
    public float chaseSpeed = 4;
    
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        behavior.BlackboardReference.SetVariableValue("Patrol Walkspeed", patrolSpeed);
        behavior.BlackboardReference.SetVariableValue("Chase Walkspeed", chaseSpeed);
        behavior.BlackboardReference.SetVariableValue("Patrol Delay", patrolDelay);
    }
}
