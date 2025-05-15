using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject Crazy8Power;
    public GameObject BlackjackPower;
    public GameObject RoyalPackItem;
    public TMP_Text Coins;
    public TMP_Text InfoText;    

    private void Start()
    {
        //Display player's coins
        Coins.text = "Coins: " + GameController.coins.ToString();

        //Remove items from shop if they are bought or if they can no longer be bought
        if(GameController.power1 == true)
        {
            Crazy8Power.gameObject.SetActive(false);
            BlackjackPower.gameObject.SetActive(false);
        }
        if (GameController.power2 == true)
        {
            BlackjackPower.gameObject.SetActive(false);
            Crazy8Power.gameObject.SetActive(false);
        }
        if (GameController.deckExpansionItem == true)
        {
            RoyalPackItem.gameObject.SetActive(false);
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
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.power1 = true;
            Crazy8Power.SetActive(false);
            BlackjackPower.gameObject.SetActive(false);
        }
    }

    public void BuyBlackjack()
    {
        if (GameController.coins < 10)
        {
            StartCoroutine(DisplayShopInfo("Not Enough Coins"));
        }
        else
        {
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.power2 = true;
            BlackjackPower.SetActive(false);
            Crazy8Power.gameObject.SetActive(false);
        }
    }

    public void BuyRoyalPack()
    {
        if (GameController.coins < 10)
        {
            StartCoroutine(DisplayShopInfo("Not Enough Coins"));
        }
        else
        {
            GameController.coins -= 10;
            Coins.text = "Coins: " + GameController.coins.ToString();
            GameController.deckExpansionItem = true;
            RoyalPackItem.SetActive(false);
        }
    }

    public IEnumerator DisplayShopInfo(string text)
    {
        InfoText.text = text;
        yield return new WaitForSeconds(2.5f);
        InfoText.text = "";
        
    }

    public void DisplayItemInformation(GameObject obj)
    {
        InfoText.text = obj.name;
    }

    public void ResetItemInformation()
    {
        InfoText.text = "";
    }


}
