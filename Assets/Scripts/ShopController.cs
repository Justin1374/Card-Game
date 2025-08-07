using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject Crazy8Power;
    public GameObject BlackjackPower;
    public GameObject RoyalPackItem;
    public TMP_Text Coins;
    public TMP_Text InfoText;   
    public GameObject DescriptionText;
    private SoundController soundPlayer;

    private void Start()
    {
        //Setup audio
        soundPlayer = FindObjectOfType<SoundController>();
        //Display player's coins
        Coins.text = "Coins: " + GameController.coins.ToString();

        //Enable not yet bought items and set their values
        //Crazy8 Power
        if(GameController.power1 == true)
        {
            Crazy8Power.gameObject.SetActive(false);
            BlackjackPower.gameObject.SetActive(false);
        }
        else
        {
            Crazy8Power.GetComponent<Item>().cost = 10;
            Crazy8Power.GetComponent<Item>().description = "Any 8 card played through a normal action will deal 8 damage to the enemy and heal you for 8 HP. Ability action can still be used. Cost: " + Crazy8Power.GetComponent<Item>().cost.ToString();
        }
        //Blackjack Power
        if (GameController.power2 == true)
        {
            BlackjackPower.gameObject.SetActive(false);
            Crazy8Power.gameObject.SetActive(false);
        }
        else
        {
            BlackjackPower.GetComponent<Item>().cost = 7;
            BlackjackPower.GetComponent<Item>().description = "Any Ace played through a normal action will deal 21 damage to the enemy ignoring defense. Ability action can still be used. Cost: " + BlackjackPower.GetComponent<Item>().cost.ToString();
        }
        //Royal Pack Item
        if (GameController.deckExpansionItem == true)
        {
            RoyalPackItem.gameObject.SetActive(false);
        }
        else
        {
            RoyalPackItem.GetComponent<Item>().cost = 14;
            RoyalPackItem.GetComponent<Item>().description = "Adds King, Queen, Ace, and Jack cards of each type that all have a value of 10. Cost: " + RoyalPackItem.GetComponent<Item>().cost.ToString();
        }
    }


    public void BuyCrazy8()
    {
        if (GameController.coins < 10)
        {
            StartCoroutine(DisplayShopInfo("Not Enough Coins"));
        }
        else
        {
            soundPlayer.BuySound();
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.power1 = true;
            Crazy8Power.SetActive(false);
            BlackjackPower.gameObject.SetActive(false);
        }
    }

    public void BuyBlackjack()
    {
        if (GameController.coins < 7)
        {
            StartCoroutine(DisplayShopInfo("Not Enough Coins"));
        }
        else
        {
            soundPlayer.BuySound();
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.power2 = true;
            BlackjackPower.SetActive(false);
            Crazy8Power.gameObject.SetActive(false);
        }
    }

    public void BuyRoyalPack()
    {
        if (GameController.coins < 15)
        {
            StartCoroutine(DisplayShopInfo("Not Enough Coins"));
        }
        else
        {
            soundPlayer.BuySound();
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.deckExpansionItem = true;
            RoyalPackItem.SetActive(false);
        }
    }

    //Display general messages, such as "Not Enough Coins"
    public IEnumerator DisplayShopInfo(string text)
    {
        InfoText.text = text;
        yield return new WaitForSeconds(2.5f);
        InfoText.text = "";
        
    }

    //Display an items spesific information
    public void DisplayItemInformation(GameObject obj)
    {
        InfoText.text = obj.name;
        DescriptionText.GetComponentInChildren<TMP_Text>().text = obj.GetComponent<Item>().description;
        DescriptionText.GetComponent<Image>().color = Color.black;
    }

    //Resets text to default value
    public void ResetItemInformation()
    {
        InfoText.text = "";
        DescriptionText.GetComponentInChildren<TMP_Text>().text = "";
        DescriptionText.GetComponent<Image>().color = Color.clear;
    }


}
