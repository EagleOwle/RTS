using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    private int health;
    public int Health
    {
        set 
        {
            health = value;
            eventOnChangeHealth.Invoke();
        }

        get
        {
            return health;
        }
    }

    public UnityEvent eventOnChangeHealth = new UnityEvent();

}