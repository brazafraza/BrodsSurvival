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
     public BuildingHandler building;
    public DeathScreen deathScreen;
    void Start()
    {
        cam = GetComponentInChildren<CameraLook>();

        inventory = GetComponentInChildren<InventoryManager>();
        crafting = GetComponentInChildren<CraftingManager>();
        storage = GetComponentInChildren<StorageUI>();
       // building = GetComponentInChildren<BuildingHandler>();
       
        
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

        if (inventory.opened || GetComponent<PlayerStats>().isDead)
        
            windowOpened = true;
        
        else
        
            windowOpened = false;
        
        
        
    }

}
