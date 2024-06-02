using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandHappiness : MonoBehaviour
{



    //add clock bar too


    public GameObject hotbar;
    public GameMenu gameMenu;

    public StatsBar happinessBar;
    public StatsBar timeBarL;
    public StatsBar timeBarR;
    public DayNightCycle dayNightCycle;

    [Header("UI")]
    public float happiness;
    public float maxHappiness = 100f;

    [Header("Placeholder")]
    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    public bool questCompleted;

    private void Start()
    {
        gameMenu = FindAnyObjectByType<GameMenu>();
        happiness = maxHappiness/2;  
    }

    private void Update()
    {

        if (happiness <= 0)
            happiness = 0;

        happinessBar.numberText.text = happiness.ToString("f0");
        happinessBar.bar.fillAmount = happiness / 100;

        if (gameMenu.opened)
            hotbar.SetActive(false);
        if (!gameMenu.opened)
            hotbar.SetActive(true);

        if (dayNightCycle.timeOfDay >= 1350)
        {
            timeBarR.numberText.text = dayNightCycle.timeOfDay.ToString("f0");
            timeBarR.bar.fillAmount = dayNightCycle.timeOfDay / 2700f;

            timeBarL.numberText.text = dayNightCycle.timeOfDay.ToString("f0");
            timeBarL.bar.fillAmount = dayNightCycle.timeOfDay / 2700f;

        }

        if (dayNightCycle.timeOfDay <= 1350)
        {
            timeBarR.numberText.text = dayNightCycle.timeOfDay.ToString("f0");
            timeBarR.bar.fillAmount = 1 - (dayNightCycle.timeOfDay / 2700f);

            timeBarL.numberText.text = dayNightCycle.timeOfDay.ToString("f0");
            timeBarL.bar.fillAmount = 1 - (dayNightCycle.timeOfDay / 2700f);

        }

        if (Input.GetKeyDown(KeyCode.J ) && Input.GetKeyDown(KeyCode.LeftShift))
            happiness = happiness - 10;
        if (Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.LeftShift))
            happiness = happiness + 10;
    }

    private void EndOfDay()
    {
        if (dayNightCycle.timeOfDay == 2150 && questCompleted)
        {
            happiness = happiness - 10;
            NewQuest();
        }
        else if(dayNightCycle.timeOfDay == 2150)
        {
            happiness = happiness + 10;
            NewQuest();
        }
    }

    private void NewQuest()
    {
        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else
        {

        }
       
        
    }

    public void StartQuestInitialDialog()
    {
        //open dialogue ui

      //  npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

    }

    private void Quest()
    {
        //if player does xyz
        //give player reward QuestComplete()
    }

    private void QuestComplete()
    {
        if(/* quest 1 completed */questCompleted)
        questCompleted = true;
    }




    private void BarConsequences()
    {
        //if hapiness is high set respawn rates higher
        //if hapiness is low set respawn rates lower
        //if hapiness is low set enemy respawn rates higher
        //if hapiness is low make enemies stronger

        
        
    }
}
