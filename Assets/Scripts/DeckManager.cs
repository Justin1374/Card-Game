using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public List<Card> playedCards = new List<Card>();

    private int currentIndex = 0;
    private int handStartSize = 6;
    private int currentHandSize = 0;
    private int maxHandSize = 7;
    

    private void Start()
    {
        //Load all card assets from the Resources folder
        Card[] numberCards = Resources.LoadAll<Card>("NumberCards");
        Card[] letterCards = Resources.LoadAll<Card>("LetterCards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(numberCards);
        if(GameController.deckExpansionItem == true)
        {
            allCards.AddRange(letterCards);
        }
        

        HandManager hand = FindObjectOfType<HandManager>();
        for(int i = 0; i < handStartSize; i++)
        {
            DrawCard(hand);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        currentHandSize = handManager.cardsInHand.Count;
        if (allCards.Count == 0)
        {
            allCards.AddRange(playedCards);
            playedCards.Clear();
            Debug.Log("Restocked Deck!");
            //return;
        }
        else if(currentHandSize >= maxHandSize)
        {
            Debug.Log("Hand is full!");
            return;
        }
        Debug.Log("DrawCard made this far");
        //Look at next card in card index, then add it to hand. Wrap around to begining when end of count is reached
        currentIndex = Random.Range(0, allCards.Count);
        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        //currentIndex = (currentIndex + 1) % allCards.Count;
        addToDiscardPile(currentIndex);


    }

    //Move cards from hand to discard pile
    public void addToDiscardPile(int currentIndex)
    {
        playedCards.Add(allCards[currentIndex]);
        allCards.RemoveAt(currentIndex);
    }

}
