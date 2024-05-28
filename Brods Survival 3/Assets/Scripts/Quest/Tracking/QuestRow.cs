using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRow : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questGiver;

    public Button trackingButton;

    public bool isActive;
    public bool isTracking;

    public Quest thisQuest;

    //add reward vars here

    private void Start()
    {
        trackingButton.onClick.AddListener(() =>
        {

            if (isActive)
            {
                if (isTracking)
                {
                    isTracking = false;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Not Tracking";
                    QuestManager.Instance.UnTrackQuest(thisQuest);
                }
                else
                {
                    isTracking = true;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Tracking";
                    QuestManager.Instance.TrackQuest(thisQuest);
                } 
            }

        });
    }

}