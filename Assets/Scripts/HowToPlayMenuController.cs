using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HowToPlayMenuController : MonoBehaviour
{
    public GameObject howToPlayMenu;
    private GameObject currentMenu;
    private SoundController soundPlayer;
    public TMP_Text pageContent;
    public TMP_Text pageTitle;
    public TMP_Text pageNumberInfoText;
    public List<Page> pages = new List<Page>();
    int currentPage = 1;

    //Page structure
    public struct Page
    {
        public string titleText;
        public string contentText;

        public Page(string title, string content)
        {
            this.titleText = title;
            this.contentText = content;
        }

    }

    //Create pages and add information to them
    Page coinsInfo = new Page("Coins", "Coins are used to buy items and powers from the shop. After each won battle you gain 5 coins plus additonal coins depending on how much extra damage is dealt on the final blow (If the enemy's health is -2 after the final blow you get 2 extra coins).");
    Page experienceInfo = new Page("Experience", "Experience is used to increase your level, which determines the maximum amount you can increase your stats. After each won battle you gain 0.2 xp plus additonal xp depending on how much extra damage is dealt on the final blow (If the enemy's health is -2 after the final blow you get 0.2 extra xp). You level up everytime you reach 1 total experience.");
    Page luckInfo = new Page("Luck", "The luck stat increases the value of the cards you play and can be up to 1 higher than your level. If you have luck of 1.3, the value of your cards played is increased by 1. The remaining 0.3 is your progress to your next luck boost.");
    Page defenseInfo = new Page("Defense", "The defense stat decreases the amount of damage you take and can be up to 1 higher than your level. If you have defense of 1.3, the damage you take is decreased by 1. The remaining 0.3 is your progress to your next defense boost.");
    Page powerInfo = new Page("Powers", "Powers change what the default actions of certain cards do. Only 1 power can be equiped at a time.");
    Page spadesInfo = new Page("Spades", "Special Ability: Using the ability action on a spades card will deal damage equal to the enemy's defense.");
    Page clubsInfo = new Page("Clubs", "Special Ability: Using the ability action on a clubs card will increase your luck by the card's value divided by 10. A 5 of clubs increases your luck by 0.5.");
    Page heartsInfo = new Page("Hearts", "Special Ability: Using the ability action on a hearts card will heal you for the card's value.");
    Page diamondsInfo = new Page("Diamonds", "Special Ability: Using the ability action on a diamonds card will increase your defense by the card's value divided by 10. A 5 of diamonds increases your defense by 0.5.");

    // Start is called before the first frame update
    private void Start()
    {
        //Find sound controller
        soundPlayer = FindObjectOfType<SoundController>();

        //Add the created pages to the list of pages
        pages.Add(coinsInfo);
        pages.Add(experienceInfo);
        pages.Add(luckInfo);
        pages.Add(defenseInfo);
        pages.Add(powerInfo);
        pages.Add(spadesInfo);
        pages.Add(clubsInfo);
        pages.Add(heartsInfo);
        pages.Add(diamondsInfo);
        Debug.Log(pages.Count);

        updatePage();

    }

    public void closeHowToPlayMenu()
    {
        soundPlayer.ClickSound();
        Destroy(this.gameObject);
    }

    private void updatePage()
    {
        //Display current page out of total pages
        pageNumberInfoText.text = currentPage + "/" + pages.Count;

        //Display current page
        pageTitle.text = pages[currentPage - 1].titleText;
        pageContent.text = pages[currentPage - 1].contentText;
    }

    public void nextPageButton()
    {
        if(currentPage < pages.Count)
        {
            soundPlayer.ClickSound();
            currentPage++;
            updatePage();
        }
    }

    public void prevPageButton()
    { 
        if(currentPage > 1)
        {
            soundPlayer.ClickSound();
            currentPage--;
            updatePage();
        }
    }

    
}
