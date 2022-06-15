using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Motion : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private float baseSpeed;

    private const string speedValue = "speed";
    private int hashSpeed = 0;

    public UnityEvent eventOnPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hashSpeed = Animator.StringToHash(speedValue);
    }

    public void Start()
    {
        baseSpeed = agent.speed;
    }

    public void Destination(Vector3 position, float speed)
    {
        if (agent.isOnNavMesh == false)
        {
            Debug.LogError("agent.is not on NavMesh");
            return;
        }

        agent.SetDestination(position);
        agent.speed = speed;
        animator.SetFloat(hashSpeed, agent.velocity.magnitude);
        if (agent.remainingDistance < agent.stoppingDistance)// && agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            eventOnPosition.Invoke();
        }

    }

    private void OnDisable()
    {
        agent.enabled = false;
    }
}
