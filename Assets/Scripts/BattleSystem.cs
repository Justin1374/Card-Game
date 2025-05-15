using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, WAIT}
public class BattleSystem : MonoBehaviour
{
    public GameObject BattleHudPrefab;
    public TMP_Text infoText;
    HandManager playerHand;
    DeckManager gameDeck;
    public Unit player;
    public Unit enemyTemplate;
    private Unit enemyUnit;
    private Unit playerUnit;
    private TMP_Text battleInfoText;
    GameObject playerHud;
    GameObject enemyHud;
    public BattleState state;

    void Start()
    {
        //Create Enemy
        enemyUnit = Instantiate(enemyTemplate);
        enemyUnit.maxHP = 100;
        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyUnit.unitName = "Bob";
        enemyUnit.power = Unit.Power.P4;
        enemyUnit.luck = 0;
        enemyUnit.defense = 1;

        //Create copy of player using playerdata
        playerUnit = Instantiate(player);
        playerUnit.maxHP = player.maxHP;
        playerUnit.currentHP = playerUnit.maxHP;
        playerUnit.unitName = player.unitName;
        playerUnit.power = player.power;
        playerUnit.luck = player.luck;
        playerUnit.defense = player.defense;

        //Create battle text
        battleInfoText = Instantiate(infoText);
        battleInfoText.transform.SetParent(transform);
        battleInfoText.rectTransform.anchoredPosition = Vector2.zero;

        //Locate existing hand and deck managers
        playerHand = FindObjectOfType<HandManager>(); 
        gameDeck = FindObjectOfType<DeckManager>();
        state = BattleState.START;
        StartCoroutine(SetupBattle(playerUnit, enemyUnit));
    }

    IEnumerator SetupBattle(Unit playerData, Unit enemyData)
    {
        //Create playerHud
        playerHud = Instantiate(BattleHudPrefab, new Vector3(6f, -3.5f ,0), Quaternion.identity);
        Debug.Log("Player health: " +  playerData.maxHP);
        playerHud.GetComponent<BattleHudManager>().unitData = playerData;
        playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();

        //Create enemyHud
        enemyHud = Instantiate(BattleHudPrefab, new Vector3(-6f, 3.5f, 0), Quaternion.identity);
        enemyHud.GetComponent<BattleHudManager>().unitData = enemyData;
        enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();

        //Start exchange of turn states, player goes first
        yield return new WaitForSeconds(1f);
        PlayerTurn();
    }

    IEnumerator PlayerAttack(Card.CardType type, int cardValue, string cardName)
    {
        //Wait
        yield return new WaitForSeconds(1.5f);

        //Add luck value to cardValue
        cardValue += (int)(playerUnit.luck);

        //Check if an active power is used, otherwise attack enemy for the card's value
        if (GameController.power1 == true && cardValue - (int)playerUnit.luck == 8)
        {
            enemyUnit.currentHP -= (cardValue - (int)enemyUnit.defense);
            playerUnit.currentHP += cardValue;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Crazy8Power works");
        }
        else if (GameController.power2 == true && cardName.Equals("Ace"))
        {
            //Deal 21 damage ignoring defense when an Ace is played
            enemyUnit.currentHP -= 21;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        }
        else
        {
            //Damage the enemy
            enemyUnit.currentHP -= (cardValue - (int)enemyUnit.defense);
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Enemy takes " + cardValue + " damage!");
        }

        TurnTransition();
    }

    IEnumerator PlayerAbility(Card.CardType type, int cardValue)
    {
        //Wait
        yield return new WaitForSeconds(1.5f);

        //Add luck value to cardValue
        cardValue += (int)(playerUnit.luck);

        //Check the card type and use the appropriate ability
        //Hearts ability
        if (type == Card.CardType.Hearts)
        {
            playerUnit.currentHP += cardValue;
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Player heals " + cardValue + " damage!");
        }
        //Clubs ability
        else if (type == Card.CardType.Clubs)
        {
            playerUnit.luck += ((float)cardValue) / 10;
            if (playerUnit.luck >= (playerUnit.unitLevel + 1))
            {
                playerUnit.luck = (playerUnit.unitLevel + 1);
            }
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Player's Luck is " + playerUnit.luck + "!");
        }
        //Diamonds ability
        else if (type == Card.CardType.Diamonds)
        {
            playerUnit.defense += ((float)cardValue) / 10;
            if (playerUnit.defense >= (playerUnit.unitLevel + 1))
            {
                playerUnit.defense = (playerUnit.unitLevel + 1);
            }
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Player's Defense is " + playerUnit.defense + "!");

        }
        //Spades/Default ability
        else
        {
            //Damage the enemy
            enemyUnit.currentHP -= (int)enemyUnit.defense;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Deal damage equal to enemy's defense");
        }

        TurnTransition();
    }

    void TurnTransition()
    {
        //Check if enemy is dead
        if (enemyHud.GetComponent<BattleHudManager>().unitData.currentHP <= 0)
        {
            //End Battle
            state = BattleState.WON;
            Debug.Log("Won");
            StartCoroutine(EndBattle());
        }
        else
        {
            //Display state status
            StartCoroutine(DisplayLine("Enemy's Turn", 0.04f));

            //Switch to enemy turn
            Debug.Log("Enemy Turn");
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            //Display victory message
            StartCoroutine(DisplayLine("You Won!", 0.04f));
            GameController.coins += 2 + Mathf.Abs(enemyUnit.currentHP);
            GameController.floorLevel += 1;
            Debug.Log("Player's coins: " + GameController.coins);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            //Display defeat message
            StartCoroutine(DisplayLine("You Lost.", 0.04f));
            Debug.Log("You Lost!");
            yield return new WaitForSeconds(2f);
        }
        //Load out of battle
        SceneManager.LoadSceneAsync("Home");
    }

    void PlayerTurn()
    {
        //Check if player has cards, draw 3 if not
        if(playerHand.cardsInHand.Count <= 0)
        {
            for(int i=0; i<3; i++)
            {
                gameDeck.DrawCard(playerHand);
            }
            
        }
        //Change state
        state = BattleState.PLAYERTURN;
    }

    

    IEnumerator EnemyTurn()
    {
        Debug.Log("EnemyTurn() active");
        //Wait
        yield return new WaitForSeconds(1.5f);

        //Randomly select an integer not less than 0 to be used as a card's value
        int enemyCardValue = Random.Range(2, 10) - (int)playerUnit.defense;
        if(enemyCardValue < 0)
        {
            enemyCardValue = 0;
        }

        //Damage the player
        playerUnit.currentHP -= enemyCardValue;
        playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        Debug.Log("Player takes " + enemyCardValue + " damage!");
        
        //Check if player is dead
        if (playerHud.GetComponent<BattleHudManager>().unitData.currentHP <= 0)
        {
            //End Battle
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            //Display state status
            StartCoroutine(DisplayLine("Player's Turn", 0.04f));

            //Switch to player turn
            PlayerTurn();
        }
    }

    public void OnAttackButton(GameObject card, int cardValue, int ability, string cardName) //ability == 1 if active, 0 if false
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            state = BattleState.WAIT;
            //Extract additional info of selected card
            Card.CardType cardType = card.GetComponent<CardDisplay>().cardData.cardType;
            //Remove card form hand and destroy Card
            playerHand.PlayCard(card);
            //Start attack
            if(ability == 1)
            {
                StartCoroutine(PlayerAbility(cardType, cardValue));
            }
            else
            {
                StartCoroutine(PlayerAttack(cardType, cardValue, cardName));
            }
        }
    }

    private IEnumerator DisplayLine(string text, float duration)
    {
        battleInfoText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            battleInfoText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
    }

}
