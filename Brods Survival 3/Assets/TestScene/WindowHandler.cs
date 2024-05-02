using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    public MouseMovement cam;

    public bool windowOpened;

    public InventoryManager inventory;
    void Start()
    {
        cam = GetComponentInChildren<MouseMovement>();

        inventory = GetComponentInChildren<InventoryManager>();
        
    }

    private void Update()
    {
        
        if(windowOpened)
        {
            cam.mouseAcitve = false;
        }
        else
        {
            cam.mouseAcitve = true;
        }

        if (inventory.opened)
        
            windowOpened = true;
        
        else
        
            windowOpened = false;
        
        
        
    }

}
