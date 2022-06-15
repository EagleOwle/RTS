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

            health = Mathf.Clamp(health, 0, 100);

            eventOnChangeHealth.Invoke();
        }

        get
        {
            return health;
        }
    }

    public UnityEvent eventOnChangeHealth = new UnityEvent();

    public void Initialise()
    {
        Health = UnityEngine.Random.Range(10, 100);
    }

    public void TakeDamage(int damage)
    {
        Health = health - damage;
    }

    
}