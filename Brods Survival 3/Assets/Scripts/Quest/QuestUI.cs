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
    public QuestManager questManager;
    public NPC npc;
    public InventoryManager inventoryManager;

    public TextMeshProUGUI dialogText;

    public Button option1BTN;
    public Button option2BTN;

    public bool menuOpen;
    public bool questAccepted = false;
    public bool playerLookingAtNPC = false;

    private void Start()
    {
        questMenu.SetActive(false);
    }

    private void Update()
    {
        CheckIfLookingAtNPC();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!menuOpen && playerLookingAtNPC)
            {
                menuOpen = true;
                Debug.Log("Open menu");
                OpenDialogUI();

                if (npc.firstTimeInteraction)
                {
                    questAccepted = true;
                }
            }
            else if (menuOpen)
            {
                menuOpen = false;
                Debug.Log("Close menu");
                CloseDialogUI();
            }
        }
        // if raycast hits object tagged NPC, player can press E to interact. set questAccepted to true and open menu
        //if (Input.GetKeyDown(KeyCode.E) && menuOpen)
        //{
        //    menuOpen = false;
        //    Debug.Log("Close menu");
        //    CloseDialogUI();
        //}
        //if (Input.GetKeyDown(KeyCode.E) && playerLookingAtNPC && !menuOpen)
        //{
        //    menuOpen = true;
        //    Debug.Log("Open menu");
        //    OpenDialogUI();
        //    if (npc.firstTimeInteraction)
        //    {
        //        questAccepted = true;
        //    }     
        //}
        //if (menuOpen && questAccepted && playerLookingAtNPC)
        //{
        //    questAccepted = false;
        //    menuOpen = false;
        //    CloseDialogUI();
        //}
        //if (menuOpen && playerLookingAtNPC)
        //{
        //    menuOpen = true;
        //    OpenDialogUI();
        //    // CloseDialogUI();
        //}
        //else if (!menuOpen)
        //{
        //    menuOpen = true;
        //    //OpenDialogUI();
        //}

        if (menuOpen && !questManager.isQuestMenuOpen && !inventoryManager.opened)
        {
            cameraLook.canMove = false;
            cameraLook.lockCursor = false;
        }
        if (!menuOpen && !questManager.isQuestMenuOpen && !inventoryManager.opened)
        {
            cameraLook.canMove = true;
            cameraLook.lockCursor = true;
        }

        if (questAccepted)
        {
            npc.StartConversation();
        }
    }

    private void CheckIfLookingAtNPC()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == npc.gameObject)
            {
                playerLookingAtNPC = true;
            }
            else
            {
                playerLookingAtNPC = false;
            }
        }
        else
        {
            playerLookingAtNPC = false;
        }
    }

    public void OpenDialogUI()
    {
        menuOpen = true;
        questMenu.SetActive(true);
    }

    public void CloseDialogUI()
    {
        menuOpen = false;
        questMenu.SetActive(false);
    }
}
