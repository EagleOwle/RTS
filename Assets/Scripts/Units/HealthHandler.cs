using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    private int _maxHealth = 100;
    private int _health = 100;

    public Action<float> ActionHealthChanged;

    public bool TakeDamage(int damage)
    {
        if (damage < 0)
        {
            throw new Exception("Attempt to deal negative damage");
        }

        _health -= damage;
        ActionHealthChanged?.Invoke((float)_health / _maxHealth);

        return _health <= 0;
    }

}
