using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static Enemy instance;
    public static Enemy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Enemy>();
            }

            return instance;
        }
    }

    private List<Squad> squads = new List<Squad>();
    public List<Squad> Squads => squads;

    public int squadSize;
    [SerializeField] private Transform[] spawnPoint;

    private int wave = 1;

    private void Start()
    {
        StartCoroutine(SpawnWaive(wave));
    }

    private IEnumerator SpawnWaive(int waveSize)
    {
        while (waveSize > 0)
        {
            yield return new WaitForSeconds(1);
            CreateSquad(squadSize);
            waveSize--;
        }
    }

    private void CreateSquad(int squadSize)
    {
        Squad squad = new Squad();
        squads.Add(squad);
        squad.Initialise(FrendlyState.enemy, "Paladin", spawnPoint, squadSize);
    }

    public void SquadDestroy(Squad squad)
    {
        Squads.Remove(squad);

        if (Squads.Count == 0)
        {
            wave++;
            StartCoroutine(SpawnWaive(wave));
        }
    }

}
