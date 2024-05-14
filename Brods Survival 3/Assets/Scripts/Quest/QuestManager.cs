using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
   public static QuestManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

       
    }

    public List<Quest> allActiveQuests;
    public List<Quest> allCompletedQuests;

    [Header("QuestMenu")]
    public GameObject questMenu;
    public bool isQuestMenuOpen;

    public GameObject activeQuestPrefab;
    public GameObject completedQuestPrefab;

    public GameObject questMenuContent;
    public CameraLook cameraLook;
    public QuestUI questUI;

    [Header("QuestTracker")]
    public GameObject questTrackerContent;
    public GameObject trackerRowPrefab;

    public List<Quest> allTrackedQuests;

    public void TrackQuest(Quest quest)
    {
        allTrackedQuests.Add(quest);
       
        RefreshTrackerList();
    }

    public void UnTrackQuest(Quest quest)
    {
        allTrackedQuests.Remove(quest);
        RefreshTrackerList();
    }

    public void RefreshTrackerList()
    {
        // Destroying the previous list
        foreach (Transform child in questTrackerContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest trackedQuest in allTrackedQuests)
        {
            GameObject trackerPrefab = Instantiate(trackerRowPrefab, Vector3.zero, Quaternion.identity);
            trackerPrefab.transform.SetParent(questTrackerContent.transform, false);

            TrackerRow tRow = trackerPrefab.GetComponent<TrackerRow>();

            tRow.questName.text = trackedQuest.questName;
            tRow.description.text = trackedQuest.questDescription;

            if (trackedQuest.info.secondRequirmentItem != "") // if we have 2 requirements
            {
                tRow.requirements.text = $"{trackedQuest.info.firstRequirmentItem}" /*functionforamnt(itemname)*/ +"/" + $"{trackedQuest.info.firstRequirementAmount}\n" +
               $"{trackedQuest.info.secondRequirmentItem}" /*functionforamnt(itemname)*/ +"/" + $"{trackedQuest.info.secondRequirementAmount}\n";
            }
            else // if we have only one
            {
                tRow.requirements.text = $"{trackedQuest.info.firstRequirmentItem}" /*functionforamnt(itemname)*/ +"/" + $"{trackedQuest.info.firstRequirementAmount}\n";
            }
        }
    }
    private void Start()
    {
        isQuestMenuOpen = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isQuestMenuOpen)
            {
                isQuestMenuOpen = true;
                questMenu.SetActive(true);
            }
            else if (isQuestMenuOpen)
            {
                isQuestMenuOpen = false;
                questMenu.SetActive(false);
            }        
        }

        if (isQuestMenuOpen && !questUI.menuOpen)
        {
            cameraLook.canMove = false;
            cameraLook.lockCursor = false;
            //Debug.Log("Camera should be active");
        }
        if (!isQuestMenuOpen && !questUI.menuOpen)
        {
            cameraLook.canMove = true;
            cameraLook.lockCursor = true;
           // Debug.Log("Camera should be disabled");
        }

    }

    public void AddActiveQuest(Quest quest)
    {
        allActiveQuests.Add(quest);
        TrackQuest(quest);
        RefreshQuestList();
    }

    public void MarkQuestCompleted(Quest quest)
    {
        allActiveQuests.Remove(quest);
        allCompletedQuests.Add(quest);
        UnTrackQuest(quest);

        RefreshQuestList();
    }

    public void RefreshQuestList()
    {
        foreach (Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (Quest activeQuest in allActiveQuests)
        {
            GameObject  questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.thisQuest = activeQuest;

            qRow.questName.text = activeQuest.questName;
            qRow.questGiver.text = activeQuest.questGiver;

            qRow.isActive = true;
            qRow.isTracking = true;

            
        }

        foreach (Quest completedQuest in allCompletedQuests)
        {
            GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.questName.text = completedQuest.questName;
            qRow.questGiver.text = completedQuest.questGiver;

            qRow.isActive = false;
            qRow.isTracking = false;


        }
    }

    //make function to show items in tracker
    //public int CheckItemAmount(/*string name*/)
   // {
    //    int itemCounter = 0;
    //    foreach(string item in itemlist )
    //    {
    //        if (item == name)
    //        {
    //            itemCounter++;
    //        }
    //    }
    //    return itemCounter;
   // }
   //call this in inv if needed

}
