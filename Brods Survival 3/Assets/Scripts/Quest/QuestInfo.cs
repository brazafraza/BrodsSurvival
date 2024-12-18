//using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]

public class QuestInfo : ScriptableObject
{
    [TextArea(5, 10)]
    public List<string> initialDialog;

    [Header("Options")]
    [TextArea(5, 10)]
    public string acceptOption;
    [TextArea(5, 10)]
    public string acceptAnswer;
    [TextArea(5, 10)]
    public string declineOption;
    [TextArea(5, 10)]
    public string declineAnswer;
    [TextArea(5, 10)]
    public string comebackAfterDecline;
    [TextArea(5, 10)]
    public string comebackInProgress;
    [TextArea(5, 10)]
    public string comebackCompleted;
    [TextArea(5, 10)]
    public string finalWords;

    [Header("Rewards")]
    // public int coinReward;
    public int happinessGain;
    // public string rewardItem1;
    // public string rewardItem2;

    [Header("Requirements")]
    //add in here gun shot req
    public string shootRequirmentItem;
    public int shootRequirementAmount;
    [Space]
    public string buildRequirmentItem;
    public int buildRequirementAmount;
    [Space]
    public string resourcesBroken;
    public int timesResourcesBroken;
    [Space]
    public string itemSmelted;
    public int timesSmelted;
    [Space]
    public string craftItem;
    public int timesCrafting;
    [Space]
    public string killType;
    public int timesKilled;



    //public string Action;
    // public int timesPerformed;


}