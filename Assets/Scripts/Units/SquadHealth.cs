using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SquadHealth
{
    public SquadHealth(int squadeSize)
    {
        units = new UnitHealth[squadeSize];
        maxHealth = squadeSize * 100;
    }

    public EventChangeHealth eventOnChangeHealth = new EventChangeHealth();
    public int maxHealth;
    public int currentHealth;
    private UnitHealth[] units;

    public void AddUnit(UnitHealth unit)
    {
        unit = AddNewUnit(unit);
        unit.eventOnChangeHealth.AddListener(ReCalculateHealth);
    }

    private UnitHealth AddNewUnit(UnitHealth unit)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == null)
            {
                return units[i] = unit;
            }
        }

        Debug.LogError("Array UnitHealth is full");
        return null;
    }

    public void ReCalculateHealth()
    {
        int health = 0;
        foreach (var item in units)
        {
            if (item != null)
            {
                health += item.Health;
            }
        }

        currentHealth = health;
        eventOnChangeHealth.Invoke(currentHealth);
    }

}

