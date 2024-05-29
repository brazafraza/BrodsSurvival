using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public bool tutorialActive;
   // public bool textActive;
    public bool playerLookingAt;
    public bool intTextActive = false;
    public GameObject tutorialObject;
    public QuestUI questUi;
    public InteractionHandler interactionHandler;


    private void Start()
    {
        tutorialObject.SetActive(false);
    }

    private void Update()
    {
        if (questUi.playerLookingAtTut)
        {
            playerLookingAt = true;
            //textActive = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (tutorialActive)
                {
                    tutorialObject.SetActive(false);
                   // intTextActive = false;
                    tutorialActive = false;
                }
                else
                {
                    tutorialObject.SetActive(true);

                    tutorialActive = true;
                }
            }
        }
        else
        {
            playerLookingAt = false;
           // textActive = false;
        }
    }

}

    

   

