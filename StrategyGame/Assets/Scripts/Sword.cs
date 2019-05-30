// Sword unit class
using UnityEngine;

public class Sword : Unit
{
    public Sword()
    {
        moveRange = 3;
        atkRange = 1;
        maxHealth = 50;
        health = maxHealth;
        moved = false;
        acted = false;
    }
}
