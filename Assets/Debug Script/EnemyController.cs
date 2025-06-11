using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using System.Collections.Generic;
public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private BehaviorGraph behavior;
    [SerializeField]
    private List<GameObject> waypoints;
    [SerializeField]
    private LineOfSight lineOfSight;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip footstepSfx;
    private Animator animator;
    private Vector3 dir;

    void Awake()
    {
        behavior.BlackboardReference.SetVariableValue<GameObject>("Target", player);
        behavior.BlackboardReference.SetVariableValue<List<GameObject>>("Waypoints", waypoints);
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lineOfSight = GetComponent<LineOfSight>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
        {
            animator.SetBool("isIdle", false);
            if (lineOfSight.DetectedTarget != null || animator.GetFloat("isTowardsLastPos") > 0)
            {
                animator.SetFloat("SpeedMagnitude", 1, 0.5f, Time.deltaTime);
            }
            else
            {
                animator.SetFloat("SpeedMagnitude", 0, -0.5f, Time.deltaTime);
            }

            dir = (agent.steeringTarget + new Vector3(0f, transform.position.y - agent.steeringTarget.y, 0f) - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
        }
        else if (animator.GetFloat("PatrolMagnitude") == 0 && animator.GetFloat("SpeedMagnitude") == 0 && animator.GetFloat("Temp") == 0)
        {
            animator.SetBool("isIdle", true);
        }
    }

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSfx);
    }

    void OnDrawGizmos()
    {
        if (agent.hasPath)
            Debug.DrawRay(transform.position, dir);
    }
}
