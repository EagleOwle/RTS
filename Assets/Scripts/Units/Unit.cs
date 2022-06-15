using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UnitState {Spawn, Ready, Dead}

public class Unit : MonoBehaviour, IDamageTaker
{
    [SerializeField] protected int speed;

    protected Squad squad;
    public Squad Squad => squad;

    protected Squad enemyTarget;
    public virtual void SetTarget(Squad target)
    {
        enemyTarget = target;
    }

    private UnitState unitState;
    public UnitState UnitState => unitState;

    private UnitHealth health;
    public UnitHealth Health => health;

    private new Collider collider;
    private new Renderer renderer;
    private Outline[] outlines;
    private Motion motion;

    private Vector3 squadTargetPosition;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        motion = GetComponentInChildren<Motion>();
        outlines = GetComponentsInChildren<Outline>();
        health = GetComponentInChildren<UnitHealth>();
        renderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        motion.eventOnPosition.AddListener(OnPosition);
        Select(false);
    }

    private IEnumerator CheckDestination()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //Debug.Log("CheckDestination");

            if (squad == null)
            {
                Debug.LogError("Squad is null");
                break;
            }

            int index = squad.GetUnitIndex(this);
            Vector3 position = Vector3.zero;

            switch (unitState)
            {
                case UnitState.Spawn:
                    position = squad.spawnPoint[index].position;
                    motion.Destination(position, speed);
                    break;

                case UnitState.Ready:
                    if (squad.Capitan != null && squad.Capitan != this)
                    {
                        position = squad.squadPattern.point[index].position;
                        motion.Destination(position, speed);
                    }
                    else
                    {
                        if(squadTargetPosition != Vector3.zero)
                        {
                            motion.Destination(squadTargetPosition, speed);
                        }
                    }
                    break;

                case UnitState.Dead:
                    break;
            }
        }

        yield return null;
    }

    public void Initialise(Squad squad)
    {
        this.squad = squad;
        this.squad.eventSelect.AddListener(Select);
        this.squad.eventSetTarget.AddListener(SetTarget);

        if(this.squad.FrendlyState == FrendlyState.friendly)
        {
            SquadSelect.Instance.AddReadyForChange(this);
        }

        unitState = UnitState.Spawn;
        StartCoroutine(CheckDestination());
    }

    private void Select(bool value)
    {
        foreach (var item in outlines)
        {
            item.enabled = value;
        }
    }

    public void Destination(Vector3 position)
    {
        squadTargetPosition = position;
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);

        if (health.Health <= 0)
        {
           Dead();
        }
    }

    private void Dead()
    {
        unitState = UnitState.Dead;
        SquadSelect.Instance.RemoveReadyForChange(this);
        squad.eventSelect.RemoveListener(Select);
        squad.eventSetTarget.RemoveListener(SetTarget);
        squad.OnUnitDestroy(this);
        StopCoroutine(CheckDestination());
        motion.enabled = false;
        Select(false);
        collider.enabled = false;
        health.enabled = false;
        renderer.gameObject.SetActive(false);
    }

    public void OnPosition()
    {
        if(unitState == UnitState.Spawn)
        {
            unitState = UnitState.Ready;
        }
    }

}
