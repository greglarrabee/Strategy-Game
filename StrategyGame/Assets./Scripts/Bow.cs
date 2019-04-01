// Bow unit class
using UnityEngine;

public class Bow : Unit
{
    public Bow()
    {
        moveRange = 2;
        atkRange = 3;
        maxHealth = 30;
        health = maxHealth;
        moved = false;
    }
}