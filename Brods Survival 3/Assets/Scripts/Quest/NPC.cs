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

    public QuestUI questUI;

    private void Start()
    {
       // questUI

        npcDialogText = questUI.dialogText;

        optionButton1 = questUI.option1BTN;
        optionButton1Text = questUI.option1BTN.GetComponentInChildren<TextMeshProUGUI>();

        optionButton2 = questUI.option2BTN;
        optionButton2Text = questUI.option2BTN.GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Update()
    {
        
    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;

    

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

            // If we return after declining the quest
            if (currentActiveQuest.declined)
            {

                questUI.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;

                SetAcceptAndDeclineOptions();
            }


            // If we return while the quest is still in progress
            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirmentsCompleted())
                {

                    SubmitRequiredItems();

                    questUI.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;

                    optionButton1Text.text = "[Take Reward]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        ReceiveRewardAndCompleteQuest();
                    });
                }
                else
                {
                    questUI.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;

                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        questUI.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                questUI.OpenDialogUI();

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

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            AcceptedQuest();
        });

        optionButton2.gameObject.SetActive(true);
        optionButton2Text.text = currentActiveQuest.info.declineOption;
        optionButton2.onClick.RemoveAllListeners();
        optionButton2.onClick.AddListener(() => {
            DeclinedQuest();
        });
    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
           // InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);

            //remove item from inv
        }


        string secondtRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (firstRequiredItem != "")
        {
            //InventorySystem.Instance.RemoveItem(secondtRequiredItem, secondRequiredAmount);

            //remove item from inv
        }

    }

    private bool AreQuestRequirmentsCompleted()
    {
        print("Checking Requirments");

        // First Item Requirment

        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        //foreach (string item in InventoryManager.inventorySlots)
        //{
        //    if (item == firstRequiredItem)
        //    {
        //        firstItemCounter++;
        //    }
        //}

        // Second Item Requirment -- If we dont have a second item, just set it to 0

        string secondRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        //foreach (string item in InventorySystem.Instance.itemList)
        //{
        //    if (item == secondRequiredItem)
        //    {
        //        secondItemCounter++;
        //    }
        //}

        //if requirments met
        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartQuestInitialDialog()
    {
        questUI.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1) // If its the last dialog 
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

            currentActiveQuest.initialDialogCompleted = true;

            SetAcceptAndDeclineOptions();
        }
        else  // If there are more dialogs
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
    private void AcceptedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            optionButton1Text.text = "[Take Reward]";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                ReceiveRewardAndCompleteQuest();
            });
            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            CloseDialogUI();
        }



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

        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

        currentActiveQuest.isCompleted = true;

        var happinessGained = currentActiveQuest.info.happinessGain;
        print("You recieved " + happinessGained + " island happiness");

        //add happiness gained to islandmeter



        //if (currentActiveQuest.info.rewardItem1 != "")
        //{
        //    InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        //}

        //if (currentActiveQuest.info.rewardItem2 != "")
        //{
        //    InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        //}

        activeQuestIndex++;

        // Start Next Quest 
        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            questUI.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
        else
        {
            questUI.CloseDialogUI();
            isTalkingWithPlayer = false;
            print("No more quests");
        }

    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        CloseDialogUI();
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
}
