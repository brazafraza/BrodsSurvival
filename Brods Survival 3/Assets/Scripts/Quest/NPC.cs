using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;


    public AudioSource audioS;
    public AudioClip questC;
    public AudioClip questF;

    TextMeshProUGUI npcDialogText;

    Button optionButton1;
    TextMeshProUGUI optionButton1Text;

    Button optionButton2;
    TextMeshProUGUI optionButton2Text;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    [Header("Quest Vars")]
    public bool shouldRecordShoot;
    public bool shouldRecordBuild;
    public bool shouldRecordBreak;
    public bool shouldRecordSmelt;
    public bool shouldRecordCraft;
    public bool shouldRecordKill;

    public int questShootCount;
    public int questBuildCount;
    public int questBreakCount;
    public int questSmeltCount;
    public int questCraftCount;
    public int questKillCount;

    public int requiredBuildAmount;
    public int requiredShootAmount;
    public int requiredBreakAmount;
    public int requiredSmeltAmount;
    public int requiredCraftAmount;
    public int requiredKillAmount;

    public bool resetShootCount;
    public bool resetBuildCount;
    public bool resetBreakCount;
    public bool resetSmeltCount;
    public bool resetCraftCount;
    public bool resetKillCount;
    [Space]
    private bool firstTimeQuestMenuOpened = true;

    public bool questFaileds;

    public TextMeshProUGUI failedOrComplete;

    [Header("Scripts")]
    public Weapon weapon;
    public QuestUI questUI;
    public BuildingHandler buildingHandler;

    public Furnace furnace;
    //public break handling?

    public IslandHappiness islandHappiness;
    public DayNightCycle dayNightCycle;

    private void Start()
    {

        audioS = GetComponent<AudioSource>();
        // questUI
        npcDialogText = questUI.dialogText;

        optionButton1 = questUI.option1BTN;
        optionButton1Text = questUI.option1BTN.GetComponentInChildren<TextMeshProUGUI>();

        optionButton2 = questUI.option2BTN;
        optionButton2Text = questUI.option2BTN.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        QuestFailed();

        if (firstTimeInteraction)
        {
            questShootCount = 0;
            questBuildCount = 0;
        }

       // if(play)
    }


    public void StartConversation()
    {
        isTalkingWithPlayer = true;

        // Check if it's the first time the player opens the quest menu
        if (firstTimeQuestMenuOpened)
        {
            // Set the flag to false to indicate the quest menu has been opened at least once
            firstTimeQuestMenuOpened = false;
            return; // Exit the method without starting any quest
        }

        // Check if it's time to start the next quest due to a failed quest
        if (questFaileds && currentActiveQuest != null && currentActiveQuest.isCompleted)
        {
            if (dayNightCycle.timeOfDay > dayNightCycle.dayEndTime && dayNightCycle.timeOfDay < dayNightCycle.dayEndTime + 0.015f)
            {
                StartNextQuest();
                return;
            }
        }

        // Interacting with the NPC for the first time
        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex]; // 0 at start
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else // Interacting with the NPC after the first time
        {
            // Automatically accept the quest
            if (!currentActiveQuest.accepted && !currentActiveQuest.declined)
            {
                AcceptedQuest();
            }

            // If we return after declining the quest
            if (currentActiveQuest.declined)
            {
                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;
                SetAcceptAndDeclineOptions();
            }

            // If we return while the quest is still in progress
            if (currentActiveQuest.accepted && !currentActiveQuest.isCompleted)
            {
                if (AreQuestRequirmentsCompleted())
                {
                    ReceiveRewardAndCompleteQuest();
                    Debug.Log("Requirements Met");
                }
                else
                {
                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;
                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        questUI.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }

            if (currentActiveQuest.isCompleted)
            {
                npcDialogText.text = currentActiveQuest.info.finalWords;
                optionButton1Text.text = "[Close]";
                optionButton1.onClick.RemoveAllListeners();
                optionButton1.onClick.AddListener(() => {
                    questUI.CloseDialogUI();
                    isTalkingWithPlayer = false;
                });
            }

            // If there is another quest available
            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }
        }
    }

    private void StartNextQuest()
    {
        activeQuestIndex++;
        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            StartConversation(); // Start the conversation for the next quest
        }
        else
        {
            Debug.Log("No more quests available.");
        }
    }

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            AcceptedQuest();
        });
    }

    private bool AreQuestRequirmentsCompleted()
    {
        string shootRequiredItem = currentActiveQuest.info.shootRequirmentItem;
        int shootRequiredAmount = currentActiveQuest.info.shootRequirementAmount;
        requiredShootAmount = shootRequiredAmount;

        string buildRequiredItem = currentActiveQuest.info.buildRequirmentItem;
        int buildRequiredAmount = currentActiveQuest.info.buildRequirementAmount;
        requiredBuildAmount = buildRequiredAmount;

        string resourcesBroken = currentActiveQuest.info.resourcesBroken;
        int timesResourcesBroken = currentActiveQuest.info.timesResourcesBroken;
        requiredCraftAmount = timesResourcesBroken;

        string itemSmelted = currentActiveQuest.info.itemSmelted;
        int timesSmelted = currentActiveQuest.info.timesSmelted;
        requiredSmeltAmount = timesSmelted;

        string craftItem = currentActiveQuest.info.craftItem;
        int timesCrafting = currentActiveQuest.info.timesCrafting;
        requiredCraftAmount = timesCrafting;

        string killType = currentActiveQuest.info.killType;
        int timesKilled = currentActiveQuest.info.timesKilled;
        requiredKillAmount = timesKilled;

        if (questShootCount >= shootRequiredAmount && questBuildCount >= buildRequiredAmount && questBreakCount >= timesResourcesBroken && questSmeltCount >= timesSmelted && questCraftCount >= timesCrafting && questKillCount >= timesKilled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void QuestFailed()
    {
        if (dayNightCycle.timeOfDay > dayNightCycle.dayEndTime && dayNightCycle.timeOfDay < dayNightCycle.dayEndTime + 0.015f)
        {
            questFaileds = true;
        }
        if (dayNightCycle.timeOfDay > 2150 && dayNightCycle.timeOfDay < 2151 && questFaileds)
        {
            failedOrComplete.text = ("Quest failed, I am upset!");
            CleanseText();

            Debug.Log("QuestFailed");
            islandHappiness.happiness -= 10;
            resetShootCount = true;
            resetBreakCount = true;
            resetCraftCount = true;
            resetBuildCount = true;
            resetSmeltCount = true;
            resetKillCount = true;
            questFaileds = false;

            audioS.PlayOneShot(questF);

            if (activeQuestIndex < quests.Count - 1)
            {
                activeQuestIndex++;
                currentActiveQuest = quests[activeQuestIndex];
                StartConversation();
            }
            else
            {
                Debug.Log("No more quests available.");
            }
        }
    }

    private void StartQuestInitialDialog()
    {
        if (currentDialog < currentActiveQuest.info.initialDialog.Count)
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogDone();
            });

            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Attempted to access an invalid dialog index.");
        }
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog >= 0 && currentDialog < currentActiveQuest.info.initialDialog.Count)
        {
            if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1)
            {
                npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
                currentActiveQuest.initialDialogCompleted = true;
                SetAcceptAndDeclineOptions();
            }
            else
            {
                npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
                optionButton1Text.text = "Next";
                optionButton1.onClick.RemoveAllListeners();
                optionButton1.onClick.AddListener(() => {
                    currentDialog++;
                    CheckIfDialogDone();
                });
            }
        }
        else
        {
            currentDialog = 0;
        }
    }

    private void AcceptedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;
        npcDialogText.text = currentActiveQuest.info.acceptAnswer;
        resetShootCount = false;
        resetBreakCount = false;
        resetCraftCount = false;
        resetBuildCount = false;
        resetSmeltCount = false;
        resetKillCount = false;
        CloseDialogUI();
    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            questUI.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        
        failedOrComplete.text = ("Quest completed, you've made me happy!");
        CleanseText();

        Debug.Log("adding happ");
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

        currentActiveQuest.isCompleted = true;

        var happinessGained = currentActiveQuest.info.happinessGain;
        print("You received " + happinessGained + " island happiness");

        islandHappiness.happiness += happinessGained;
        resetShootCount = true;
        resetBreakCount = true;
        resetCraftCount = true;
        resetBuildCount = true;
        resetSmeltCount = true;
        resetKillCount = true;

        audioS.PlayOneShot(questC);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void ReceiveShootCount(int shootCount)
    {
        Debug.Log("Shoot count received: " + shootCount);
        questShootCount = shootCount;
    }

    public void ReceiveBuildCount(int buildCount)
    {
        Debug.Log("Build count received: " + buildCount);
        questBuildCount = buildCount;
    }

    public void ReceiveBreakCount(int breakCount)
    {
        Debug.Log("Break count received: " + breakCount);
        questBreakCount = breakCount;
    }

    public void ReceiveCraftCount(int craftCount)
    {
        Debug.Log("Craft count received: " + craftCount);
        questCraftCount = craftCount;
    }

    public void ReceiveSmeltCount(int smeltCount)
    {
        Debug.Log("Smelt count received: " + smeltCount);
        questSmeltCount = smeltCount;
    }

    public void ReceiveKillCount(int killCount)
    {
        Debug.Log("Kill count received: " + killCount);
        questKillCount = killCount;
    }

    public void CleanseText()
    {
       
        Invoke("CleanseText22", 5f);
    }

    public void CleanseText22()
    {
        failedOrComplete.text = ("");
    }

}
