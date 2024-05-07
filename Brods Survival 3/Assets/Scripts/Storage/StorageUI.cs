using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    
    [HideInInspector]public Storage storageOpened;
    public Slot slotPrefab;
    public Transform content;
    [Space]
    public bool opened;
    public Vector3 openPosition;

    [Header("Furnace UI")]
    public GameObject furnaceUI;
    public GameObject turnOnButton;
    public GameObject turnOffButton;




    private void Update()
    {
        if (opened)
            transform.localPosition = openPosition;
        else
            transform.position = new Vector3(-10000, 0, 0);

        if (storageOpened.isFurnace)
        {
            if (storageOpened.GetComponent<Furnace>().isOn)
            {
                for (int i = 0; i < storageOpened.slots.Length; i++)
                {
                    Slot slot = Instantiate(slotPrefab, content).GetComponent<Slot>();

                    if (storageOpened.slots[i].data == null)
                        slot.data = null;
                    else
                        slot.data = storageOpened.slots[i].data;

                    slot.stackSize = storageOpened.slots[i].stackSize;



                    slot.UpdateSlot();
                }
            }

            furnaceUI.SetActive(true);

            if (storageOpened.GetComponent<Furnace>().isOn)
            {
                turnOffButton.SetActive(true);
                turnOnButton.SetActive(false);
            }
            else
            {
                turnOffButton.SetActive(false);
                turnOnButton.SetActive(true);
            }

        }
        else
        {
            furnaceUI.SetActive(false);
        }
    }

    public void Open(Storage storage)
    {
        storageOpened = storage;

        for (int i = 0; i < storage.slots.Length; i++)
        {
            Slot slot = Instantiate(slotPrefab, content).GetComponent<Slot>();

            if (storage.slots[i].data == null)
                slot.data = null;
            else
                slot.data = storage.slots[i].data;

            slot.stackSize = storage.slots[i].stackSize;

           

            slot.UpdateSlot();

            

        }

        opened = true;
    }

    public void Close()
    {
        if (storageOpened == null)
            return;

        storageOpened.Close(GetComponentsInChildren<Slot>());
       

        Slot[] slotsToDestroy = GetComponentsInChildren<Slot>();

        for (int i = 0; i < slotsToDestroy.Length; i++)
        {
            Destroy(slotsToDestroy[i].gameObject);
        }

        

        opened = false;



    }

    public void TurnOffFurnace()
    {
        if (storageOpened == null)
            return;

        storageOpened.GetComponent<Furnace>().TurnOff();
    }
    
    public void TurnOnFurnace()
    {
        if (storageOpened == null)
            return;

        storageOpened.GetComponent<Furnace>().TurnOn();

    }
}
