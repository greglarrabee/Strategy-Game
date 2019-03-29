// Sword unit class
using UnityEngine;

public class Sword : Unit
{
    public Sword()
    {
        moveRange = 2;
        atkRangeMax = 1;
        atkRangeMin = 1;
        maxHealth = 10;
        health = maxHealth;
    }
}
