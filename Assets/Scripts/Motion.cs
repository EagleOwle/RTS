using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof( CharacterController))]
public class Motion : MonoBehaviour
{
    private NavMeshAgent agent;
    private float baseSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        baseSpeed = agent.speed;
    }

    public void Destination(Vector3 position)
    {
        agent.SetDestination(position);
        agent.speed = baseSpeed;
    }

    public void Destination(Vector3 position, float speed)
    {
        agent.SetDestination(position);
        agent.speed = speed;
    }
}
