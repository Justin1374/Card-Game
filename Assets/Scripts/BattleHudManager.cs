using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHudManager : MonoBehaviour
{

    public Unit unitData;

    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text luckText;
    public TMP_Text defenseText;
    public TMP_Text levelText;

    public Image[] powerImages;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHudDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateHudDisplay()
    {
        //Update text components
        nameText.text = unitData.unitName;
        healthText.text = "HP: " + unitData.currentHP.ToString() + "|" + unitData.maxHP.ToString();
        luckText.text = "Luck: " + unitData.luck.ToString();
        defenseText.text = "Def: " + unitData.defense.ToString();
        levelText.text = unitData.unitLevel.ToString();



        //Update image components
        if (unitData.power.ToString().Equals("Crazy_8"))
        {
            powerImages[0].gameObject.SetActive(true);
        }
        else if (unitData.power.ToString().Equals("Blackjack"))
        {
            powerImages[1].gameObject.SetActive(true);
        }
        else if (unitData.power.ToString().Equals("P3"))
        {
            powerImages[2].gameObject.SetActive(true);
        }
        else
        {
            powerImages[3].gameObject.SetActive(true);
        }
    }

}
