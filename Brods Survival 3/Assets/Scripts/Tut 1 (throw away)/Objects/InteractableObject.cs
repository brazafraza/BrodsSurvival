using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;

    public string GetItemName()
    {
        return ItemName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)&&playerInRange && SelectionManager.Instance.onTarget)
        {
            //if not full
            if(!InventorySystem.Instance.CheckIfFull())
            {    
                Debug.Log("item in inv");
                InventorySystem.Instance.AddToInventory(ItemName);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inv full");
            }


       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }


    }
}
