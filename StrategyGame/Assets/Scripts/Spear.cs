// Spear unit class
using UnityEngine;

public class Spear : Unit
{
    public Spear()
    {
        moveRange = 2;
        atkRange = 2;
        maxHealth = 40;
        health = maxHealth;
        moved = false;
        acted = false;
    }
}
