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

        //Locate existing hand and deck managers (CHANGE TO: Instantiate hand and deck mamangers)
        playerHand = FindObjectOfType<HandManager>(); //Later change this so that Battle System creates the Hand and Deck managers too.
        gameDeck = FindObjectOfType<DeckManager>(); //^
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

    IEnumerator PlayerAttack(Card.CardType type, int cardValue, int ability) //ability == 1 if active, 0 if false
    {
        //Wait 2 seconds
        yield return new WaitForSeconds(1.5f);
        cardValue += (int)(playerUnit.luck);
        if (ability == 0)
        {
            //Damage the enemy
            enemyHud.GetComponent<BattleHudManager>().unitData.currentHP -= cardValue;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Enemy takes " + cardValue + " damage!");
            //StartCoroutine(DisplayLine("Enemy takes " + cardValue + " damage!"));
        }
        else
        {
            //Check card type and play corresponding ability
            if (type == Card.CardType.Hearts)
            {
                playerHud.GetComponent<BattleHudManager>().unitData.currentHP += cardValue;
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Player heals " + cardValue + " damage!");
                //StartCoroutine(DisplayLine("Player heals " + cardValue + " damage!"));
            }
            else if (type == Card.CardType.Clubs)
            {
                playerUnit.luck += ((float)cardValue)/10;
                if(playerUnit.luck >= (playerUnit.unitLevel + 1))
                {
                    playerUnit.luck = (playerUnit.unitLevel + 1);
                }
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Player's Luck is " + playerUnit.luck + "!");
                //StartCoroutine(DisplayLine("Player's Luck increased by " + cardValue + "%!"));
            }
            else if(type == Card.CardType.Diamonds)
            {
                playerUnit.defense += ((float)cardValue)/10;
                if (playerUnit.defense >= (playerUnit.unitLevel + 1))
                {
                    playerUnit.defense = (playerUnit.unitLevel + 1);
                }
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Player's Defense is " + playerUnit.defense + "!");
                //Debug.Log("Test: " + (5-playerUnit.defense));
                //StartCoroutine(DisplayLine("Player's Defense increased by " + cardValue + "%!"));
            }
            else
            {
                //Damage the enemy
                enemyHud.GetComponent<BattleHudManager>().unitData.currentHP -= cardValue + 2;
                playerHud.GetComponent<BattleHudManager>().unitData.currentHP -= 2;
                enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Everyone takes 2 extra damage!");
                //StartCoroutine(DisplayLine("Everyone takes 2 extra damage!"));
            }
        }

        

        //Check if enemy is dead
        if(enemyHud.GetComponent<BattleHudManager>().unitData.currentHP <= 0)
        {
            //End Battle
            state = BattleState.WON;
            Debug.Log("Won");
            StartCoroutine(EndBattle());
        }
        else
        {
            //Display state status
            StartCoroutine(DisplayLine("Enemy's Turn"));

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
            StartCoroutine(DisplayLine("You Won!"));
            GameController.coins += 1;
            Debug.Log("Player's coins: " + GameController.coins);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            //Display defeat message
            StartCoroutine(DisplayLine("You Lost."));
            Debug.Log("You Lost!");
            yield return new WaitForSeconds(2f);
        }
        SceneManager.LoadSceneAsync("Home");

        //Load out of battle

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
        yield return new WaitForSeconds(1.5f);
        int enemyCardValue = Random.Range(2, 10) - (int)playerUnit.defense;
        if(enemyCardValue < 0)
        {
            enemyCardValue = 0;
        }
        //Damage the player
        playerHud.GetComponent<BattleHudManager>().unitData.currentHP -= enemyCardValue;
        playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        Debug.Log("Player takes " + enemyCardValue + " damage!");
        //StartCoroutine(DisplayLine("Player takes " + enemyCardValue + " damage!"));

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
            StartCoroutine(DisplayLine("Player's Turn"));

            //Switch to player turn
            PlayerTurn();
        }

    }

    public void OnAttackButton(GameObject card, int cardValue, int ability) //ability == 1 if active, 0 if false
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
            StartCoroutine(PlayerAttack(cardType, cardValue, ability));
        }

    }

    private IEnumerator DisplayLine(string text)
    {
        battleInfoText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            battleInfoText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

    }

}
