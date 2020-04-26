using System;
using System.Collections;
using System.Collections.Generic;
using Races;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public string entityName;
    public float health;
    public float maxHealth;

    public float mana;
    public float maxMana;

    public Race race;
    public Sprite sprite;

    public bool HasMaxHealth => Math.Abs(health - maxHealth) < 0.1f;
    public bool HasMaxMana => Math.Abs(mana - maxMana) < 0.1f;
}
