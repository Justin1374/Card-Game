using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, WAIT}
public class BattleSystem : MonoBehaviour
{
    public GameObject BattleHudPrefab;
    public TMP_Text infoText;
    public TMP_Text xpEarned;
    public TMP_Text coinsEarned;
    public Sprite[] xpBarSprite;
    public GameObject xpBar;
    HandManager playerHand;
    DeckManager gameDeck;
    SoundController soundPlayer;
    public Unit player;
    public Unit enemyTemplate;
    private Unit enemyUnit;
    private Unit playerUnit;
    private TMP_Text battleInfoText;
    GameObject playerHud;
    GameObject enemyHud;
    public BattleState state;
    private int battleHealth = 25 + 10*(GameController.floorLevel);
    int enemyChoiceThreshold = 7;

    void Start()
    {
        //Create Enemy
        enemyUnit = Instantiate(enemyTemplate);
        //Set Enemy health, level and name
        enemyUnit.maxHP = battleHealth;
        enemyUnit.currentHP = battleHealth;
        enemyUnit.unitLevel = GameController.floorLevel;
        if(enemyUnit.unitLevel < 0)
        {
            enemyUnit.unitLevel = 0;
        }
        else if (enemyUnit.unitLevel < 3)
        {
            enemyUnit.unitName = "Grunt";
        }
        else if(enemyUnit.unitLevel < 9)
        {
            enemyUnit.unitName = "Henchman";
        }
        else
        {
            enemyUnit.unitName = "BOSS";
        }
        //Determine Enemy modifiers based on current level
        //Determine if the enemy has a power
        if(GameController.floorLevel > 2)
        {
            enemyUnit.power = (Unit.Power)Random.Range(0, 2);
        }
        else
        {
            enemyUnit.power = Unit.Power.P3;
        }
        //Determine if the enemy has increased luck
        if(GameController.floorLevel > 6) 
        {
            enemyUnit.luck = Random.Range(0, enemyUnit.unitLevel - 6);        
        }
        else
        {
            enemyUnit.luck = 0;
        }
        //Determine if the enemy has increased defense
        if( GameController.floorLevel > 3)
        {
            enemyUnit.defense = Random.Range(0, enemyUnit.unitLevel - 3);
        }
        else 
        {
            enemyUnit.defense = 0;
        }

        //Create copy of player using playerdata
        playerUnit = Instantiate(player);
        playerUnit.maxHP = battleHealth;
        playerUnit.currentHP = battleHealth;
        playerUnit.unitName = player.unitName;
        playerUnit.unitLevel = GameController.playerLevel;
        if(GameController.power1 == true)
        {
            playerUnit.power = Unit.Power.Crazy_8;
        }
        if (GameController.power2 == true)
        {
            playerUnit.power = Unit.Power.Blackjack;
        }
        playerUnit.luck = 0;
        playerUnit.defense = 0;

        //Create battle text
        battleInfoText = Instantiate(infoText);
        battleInfoText.transform.SetParent(transform);
        battleInfoText.rectTransform.anchoredPosition = Vector2.zero;

        //Locate existing hand and deck managers, and sound player
        playerHand = FindObjectOfType<HandManager>(); 
        gameDeck = FindObjectOfType<DeckManager>();
        soundPlayer = FindObjectOfType<SoundController>();
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
            soundPlayer.DamageSound();
            enemyUnit.currentHP -= (cardValue - (int)enemyUnit.defense);
            playerUnit.currentHP += cardValue;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Crazy8Power works");
        }
        else if (GameController.power2 == true && cardName.Equals("Ace"))
        {
            //Deal 21 damage ignoring defense when an Ace is played
            soundPlayer.DamageSound();
            enemyUnit.currentHP -= 21;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        }
        else
        {
            //Damage the enemy
            soundPlayer.DamageSound();
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
            soundPlayer.HealSound();
            playerUnit.currentHP += cardValue;
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            Debug.Log("Player heals " + cardValue + " damage!");
        }
        //Clubs ability
        else if (type == Card.CardType.Clubs)
        {
            soundPlayer.BuffSound();
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
            soundPlayer.BuffSound();
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
            soundPlayer.DamageSound();
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
            StartCoroutine(DisplayLine("Enemy's Turn", 0.02f));

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
            soundPlayer.WinSound();
            StartCoroutine(DisplayLine("You Won!", 0.04f));

            //Display and add coins earned
            coinsEarned.text = "+" + (5 + Mathf.Abs(enemyUnit.currentHP)).ToString() + " Coins";
            GameController.coins += 5 + Mathf.Abs(enemyUnit.currentHP);

            //Increase current floor level
            GameController.floorLevel += 1;

            //Display and add experience earned
            xpEarned.text = "+" + (0.2f + Mathf.Abs((float)enemyUnit.currentHP/10)).ToString() + " Xp";
            GameController.xp += 0.2f + Mathf.Abs((float)enemyUnit.currentHP/10);
            xpBar.gameObject.SetActive(true);
            Debug.Log("Current xp is: " + GameController.xp);
            Debug.Log("Sprite array index is: " + (int)(GameController.xp*10));
            

            //Increase player level if xp threshold is reached
            if(GameController.xp >= 1)
            {
                GameController.playerLevel += 1;
                xpBar.GetComponent<UnityEngine.UI.Image>().sprite = xpBarSprite[(int)(GameController.xp * 10) - 10];
                yield return new WaitForSeconds(1f);
                StartCoroutine(DisplayLine("Level Up!", 0.04f));
                GameController.xp -= 1;
                Debug.Log("Xp after level up is: " + GameController.xp);
            }
            else
            {
                xpBar.GetComponent<UnityEngine.UI.Image>().sprite = xpBarSprite[(int)(GameController.xp * 10)];
                Debug.Log("Xp after match is: " + GameController.xp);
            }
            Debug.Log("Player's coins: " + GameController.coins);
            yield return new WaitForSeconds(4f);
        }
        else
        {
            //Display defeat message
            soundPlayer.DefeatSound();
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
            soundPlayer.DealCardSound();
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

        //Randomly select a positive integer to be used as a card's value
        int enemyCardValue = Random.Range(2, 11);
        enemyCardValue -= (int)playerUnit.defense;
        enemyCardValue += (int)enemyUnit.luck;
        if (enemyCardValue < 0)
        {
            enemyCardValue = 0;
        }

        //Decide enemy's action
        int action = Random.Range(0, 11);
        if(action < enemyChoiceThreshold || enemyUnit.currentHP >= enemyUnit.maxHP) //Attack
        {
            soundPlayer.DamageSound();
            playerUnit.currentHP -= enemyCardValue;
            playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        }
        else if (action == enemyChoiceThreshold) //Power
        {
            if (enemyUnit.power == 0)
            {
                soundPlayer.DamageSound();
                playerUnit.currentHP -= enemyCardValue;
                enemyUnit.currentHP += enemyCardValue;
                enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Enemy Crazy8Power works");
            }
            else if (enemyUnit.power == (Unit.Power)1)
            {
                //Deal 21 damage ignoring defense when an Ace is played
                soundPlayer.DamageSound();
                playerUnit.currentHP -= 21;
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
                Debug.Log("Enemy BlackJack Power works");
            }
            else
            {
                soundPlayer.DamageSound();
                playerUnit.currentHP -= enemyCardValue;
                playerHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
            }

        }
        else //Heal
        {
            soundPlayer.HealSound();
            enemyUnit.currentHP += enemyCardValue;
            enemyHud.GetComponent<BattleHudManager>().UpdateHudDisplay();
        }

        //Check if player is dead
        if (playerHud.GetComponent<BattleHudManager>().unitData.currentHP <= 0)
        {
            //End Battle
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            //Display state status
            StartCoroutine(DisplayLine("Player's Turn", 0.02f));

            //Switch to player turn
            yield return new WaitForSeconds(1.2f);
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
            soundPlayer.PlayCardSound();
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
