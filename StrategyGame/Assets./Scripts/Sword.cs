﻿// Sword unit class
using UnityEngine;

public class Sword : Unit
{
    public Sword()
    {
        moveRange = 2;
        atkRange = 1;
        maxHealth = 50;
        health = maxHealth;
        moved = false;
    }
}