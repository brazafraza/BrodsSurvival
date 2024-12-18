using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
      public CameraLook cam;
    public bool windowOpened;

    [HideInInspector] public InventoryManager inventory;
    [HideInInspector] public CraftingManager crafting;
    [HideInInspector] public StorageUI storage;
    [HideInInspector] public GameMenu gameMenu;
     public BuildingHandler building;
    public DeathScreen deathScreen;
    void Start()
    {
        cam = GetComponentInChildren<CameraLook>();

        inventory = GetComponentInChildren<InventoryManager>();
        crafting = GetComponentInChildren<CraftingManager>();
        storage = GetComponentInChildren<StorageUI>();
        // building = GetComponentInChildren<BuildingHandler>();
        gameMenu = FindObjectOfType<GameMenu>();
       
        
    }

    private void Update()
    {
        
        if(windowOpened)
        {
            cam.lockCursor = false;
            cam.canMove = false;
        }
        else
        {
            cam.lockCursor = true;
            cam.canMove = true;
        }

        if (gameMenu != null)
        {
            if (inventory.opened || GetComponent<PlayerStats>().isDead || gameMenu.opened)

                windowOpened = true;

            else
            {
                windowOpened = false;

            }
        }
        else
        {
            if (inventory.opened || GetComponent<PlayerStats>().isDead)

                windowOpened = true;

            else
            {
                windowOpened = false;

            }
        }

        
        
           
        
        
        
    }

}
