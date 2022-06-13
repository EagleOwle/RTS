using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMonitoring : MonoBehaviour
{
    private Unit unit;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        SquadSelect.Instance.AddReadyForChange(unit);
    }

    private void OnBecameVisible()
    {
        if (unit != null && unit.Squad != null && unit.Squad.FrendlyState == FrendlyState.friendly)
        {
            SquadSelect.Instance.AddReadyForChange(unit);
        }
    }

    private void OnBecameInvisible()
    {
        if (unit != null)
        {
            SquadSelect.Instance.RemoveReadyForChange(unit);
        }
    }

}
