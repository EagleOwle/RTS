using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Squad squad;

    private void Start()
    {
        squad = new Squad();
        squad.Initialise(FrendlyState.friendly);
        squad.InstantiateUnits(Vector3.zero);
    }
}
