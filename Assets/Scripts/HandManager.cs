using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab; //Assigned in inspector
    public Transform handTransform; //Root of hand positon
    BattleSystem battleSystem;
    public float fanSpread = 7.5f;
    public float cardSpacing = -100f; //Horizontal spacing of cards displayed in hand
    public float verticalSpacing = 100f; //Vertical spacing of cards displayed in hand

    public List<GameObject> cardsInHand = new List<GameObject>(); //List of card objects in hand

    public void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }


    public void AddCardToHand(Card cardData)
    {
        //Instantiate new card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        //Set the CardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    public void PlayCard(GameObject card)
    {
        cardsInHand.Remove(card);
        Destroy(card);
        //battleSystem.OnAttackButton(card.GetComponent<CardDisplay>().cardData.damageMax);
        UpdateHandVisuals();
        
    }

    private void Update()
    {
        //UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        Debug.Log("Number of Cards in deck: " + cardCount);

        if(cardCount == 1 ) 
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for(int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalize card position between -1, 1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            //Set card positon
            cardsInHand[i].transform.localPosition = new Vector3 (horizontalOffset, verticalOffset, 0f);
        }
    }

}
