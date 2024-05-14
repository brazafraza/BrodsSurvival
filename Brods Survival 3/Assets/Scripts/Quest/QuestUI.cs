using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject questMenu;
    public CameraLook cameraLook;
    public NPC npc;
    
    public TextMeshProUGUI dialogText;

    public Button option1BTN;
    public Button option2BTN;

    public bool menuOpen;
    public bool questAccepted = false;

    private void Start()
    {
        questMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (menuOpen)
            {
                CloseDialogUI();
                
            }
            else if (!menuOpen)
            {
                OpenDialogUI();
            }
        }
        if (menuOpen)
        {
            cameraLook.canMove = false;
            cameraLook.lockCursor = false;
        }
        if (!menuOpen)
        {
            cameraLook.canMove = true;
            cameraLook.lockCursor = true;
        }


        if (questAccepted)
        {
            npc.StartConversation();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            questAccepted = true;
        }


    }
    public void OpenDialogUI()
    {
        //disable mouse and cam movement
        menuOpen = true;
       
        questMenu.SetActive(true);
    }

    public void CloseDialogUI()
    {
        menuOpen = false;
        
        questMenu.SetActive(false);
    }

    private void openKey()
    {

    }
}
