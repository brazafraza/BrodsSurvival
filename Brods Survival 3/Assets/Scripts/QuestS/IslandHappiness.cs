using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandHappiness : MonoBehaviour
{
  
    public StatsBar happinessBar;
    public DayNightCycle dayNightCycle;

    [Header("UI")]
    public float happiness;
    public float maxHappiness = 100f;

    public bool questCompleted;

    private void Start()
    {
        happiness = maxHappiness;
       
    }

    private void Update()
    {
        if (happiness <= 0)
            happiness = 0;

        happinessBar.numberText.text = happiness.ToString("f0");
        happinessBar.bar.fillAmount = happiness / 100;

        EndOfDay();
        BarConsequences();

        

        if (Input.GetKeyDown(KeyCode.J))
            happiness = happiness - 10;
        if (Input.GetKeyDown(KeyCode.K))
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
        //deactivate old quest = false;
        //activate new quest = true;
        //tell player to do xyz
        
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
