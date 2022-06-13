using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitHealth health;

    private Unit capitan;
    public Unit Capitan
    {
        set
        {
            capitan = value;
            if (capitan == this)
            {
                squadPattern = GameObject.Instantiate(Resources.Load("Prefabs/SquadPattern", typeof(SquadPattern))) as SquadPattern;
                squadPattern.transform.SetParent(transform);
                squadPattern.transform.localPosition = Vector3.zero;
                squadPattern.transform.localEulerAngles = Vector3.zero;

                UISceneIndicatorManager.Instance.AddTarget(gameObject);

                StopCoroutine(CheckDestination());
            }

        }
    }

    private Squad squad;
    public Squad Squad => squad;

    private SquadPattern squadPattern;

    [SerializeField] private Outline outline;
    [SerializeField] private Motion motion;

    private void Awake()
    {
        motion = GetComponentInChildren<Motion>();
        outline = GetComponentInChildren<Outline>();
    }

    private void Start()
    {
        outline.enabled = false;
        StartCoroutine(CheckDestination());
    }

    private IEnumerator CheckDestination()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (capitan != this && capitan != null)
            {
                int index = squad.GetUnitIndex(this);
                if (index >= 0)
                {
                    Vector3 position = capitan.squadPattern.point[index].position;
                    motion.Destination(position, 6);
                }
            }
        }
    }

    public void Initialise(Squad squad)
    {
        this.squad = squad;
        this.squad.actionSelect += Select;

        if(squad.FrendlyState == FrendlyState.friendly)
        {
            SquadSelect.Instance.AddReadyForChange(this);
        }
    }

    private void Select(bool value)
    {
        outline.enabled = value;
    }

    public void Destination(Vector3 position)
    {
        if (capitan == this || capitan == null)
        {
            motion.Destination(position);

        }
    }

}
