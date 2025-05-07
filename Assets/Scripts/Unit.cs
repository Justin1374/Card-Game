using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public float luck;
    public float defense;
    public Power power;

    public enum Power
    {
        Crazy_8, Blackjack, P3, P4
    }




}
