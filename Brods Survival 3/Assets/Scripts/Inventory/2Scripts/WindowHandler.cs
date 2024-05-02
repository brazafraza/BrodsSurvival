using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
      public CameraLook cam;
   // public MouseMovement cam;

    public bool windowOpened;

    public InventoryManager inventory;
    void Start()
    {
        cam = GetComponentInChildren<CameraLook>();

        inventory = GetComponentInChildren<InventoryManager>();
        
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

        if (inventory.opened)
        
            windowOpened = true;
        
        else
        
            windowOpened = false;
        
        
        
    }

}
