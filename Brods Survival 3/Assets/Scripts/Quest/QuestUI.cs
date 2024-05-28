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
    public GameMenu gameMenu;
    public Tutorial tutorial;

    public TextMeshProUGUI dialogText;

    public Button option1BTN;
    public Button option2BTN;

    public bool menuOpen;
    public bool questAccepted = false;
    public bool playerLookingAtNPC = false;
    public bool playerLookingAtTut = false;

    private void Start()
    {
        questMenu.SetActive(false);
        gameMenu = FindAnyObjectByType<GameMenu>();
    }

    private void Update()
    {
        CheckIfLookingAtNPC();
        CheckIfLookingAtTut();

        if (playerLookingAtNPC)
        {
          //  interactionText
        }

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

        //if (gameMenu.opened)
        //{
        //    Debug.Log("enable");
        //    cameraLook.canMove = false;
        //    cameraLook.lockCursor = false;
        //}

        //else if ((menuOpen && !questManager.isQuestMenuOpen && !inventoryManager.opened))
        //{
        //    Debug.Log("QUEST UI: mouse disabled");
        //    cameraLook.canMove = false;
        //    cameraLook.lockCursor = false;
        //}

        //if ((!menuOpen && !questManager.isQuestMenuOpen && !inventoryManager.opened) || !gameMenu.opened)
        //{
        //    Debug.Log("QUEST UI: mouse enabled");
        //    cameraLook.canMove = true;
        //    cameraLook.lockCursor = true;
        //}

        if (menuOpen || questManager.isQuestMenuOpen || inventoryManager.opened || gameMenu.opened)
        {
            //enable mouse
            cameraLook.canMove = false;
            cameraLook.lockCursor = false;
        }
        
        if(!menuOpen && !questManager.isQuestMenuOpen && !inventoryManager.opened && !gameMenu.opened)
        {
            //disable mouse
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

        if (Physics.Raycast(ray, out hit, 7.5f))
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

    private void CheckIfLookingAtTut()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 7.5f))
        {
            if (hit.collider.gameObject == tutorial.gameObject)
            {
                playerLookingAtTut = true;
            }
            //else
            //{
            //    playerLookingAtTut = false;
            //}
        }
        else
        {
            playerLookingAtTut = false;
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
