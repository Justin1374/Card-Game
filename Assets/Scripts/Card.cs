using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject 
{
    public string cardName;
    public CardType cardType;
    public int damageMin;
    public int damageMax;
    public DamageType damageType;

    public enum CardType
    {
        Hearts, Diamonds, Clubs, Spades
    }

    public enum DamageType
    {
        Hearts, Diamonds, Clubs, Spades
    }
    
}
