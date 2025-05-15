using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;

    public TMP_Text nameText;

    public TMP_Text damage1Text;

    public TMP_Text damage2Text;

    
    public Image[] typeImages;
    //private Color[] typeColours = { Color.red, Color.black };

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        //Update text components
        nameText.text = cardData.cardName;
        if (cardData.cardName.Equals("King")){
            damage1Text.text = "K";
            damage2Text.text = "K";
        }
        else if (cardData.cardName.Equals("Queen"))
        {
            damage1Text.text = "Q";
            damage2Text.text = "Q";
        }
        else if (cardData.cardName.Equals("Jack"))
        {
            damage1Text.text = "J";
            damage2Text.text = "J";
        }
        else if (cardData.cardName.Equals("Ace"))
        {
            damage1Text.text = "A";
            damage2Text.text = "A";
        }
        else
        {
            damage1Text.text = cardData.damageMax.ToString();
            damage2Text.text = cardData.damageMax.ToString();
        }        
        
        //Update image components
        if (cardData.cardType.ToString().Equals("Hearts"))
        {
            typeImages[3].gameObject.SetActive(true);
            damage1Text.color = Color.red;
            damage2Text.color = Color.red;
        }
        else if(cardData.cardType.ToString().Equals("Diamonds"))
        {
            typeImages[2].gameObject.SetActive(true);
            damage1Text.color = Color.red;
            damage2Text.color = Color.red;
        }
        else if (cardData.cardType.ToString().Equals("Spades"))
        {
            typeImages[1].gameObject.SetActive(true);
            damage1Text.color = Color.black;
            damage2Text.color = Color.black;
        }
        else
        {
            typeImages[0].gameObject.SetActive(true);
            damage1Text.color = Color.black;
            damage2Text.color = Color.black;
        }
    }
}
