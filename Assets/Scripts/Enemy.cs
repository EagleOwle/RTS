using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<Squad> squads = new List<Squad>();
    private int waive = 1;

    private void Start()
    {
        CreateSquad();
    }

    private void CreateSquad()
    {
        Squad squad = new Squad();
        squad.Initialise(FrendlyState.enemy);
        squads.Add(squad);
        squad.InstantiateUnits(transform.position);
    }

    
}
