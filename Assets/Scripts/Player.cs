using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    private List<Squad> squads = new List<Squad>();
    public List<Squad> Squads => squads;

    public int squadSize;
    [SerializeField] private Transform[] spawnPoint;

    private void Start()
    {
        //CreateSquad();
    }

    private void CreateSquad()
    {
        Squad squad = new Squad();
        squads.Add(squad);
        squad.Initialise(FrendlyState.friendly, "Paladin", spawnPoint, squadSize);
    }

    public void SquadDestroy(Squad squad)
    {
        Squads.Remove(squad);
    }

}
