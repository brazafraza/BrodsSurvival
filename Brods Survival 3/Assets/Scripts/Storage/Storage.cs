using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private bool hasGeneratedSlots;
    [HideInInspector]public StorageSlot[] slots;
    public StorageSlot slotPrefab;
    public int storageSize = 12;
    [Space]
    public bool opened;

    [Header("Furnace Config")]
    [HideInInspector] public bool isFurnace;

    private void Start()
    {
        isFurnace = GetComponent<Furnace>() != null;

        if (!hasGeneratedSlots)
        {
            GenerateSlots();
        }
    }

    private void Update()
    {
       
    }
    public void GenerateSlots()
    {
        List<StorageSlot> slotList = new List<StorageSlot>();

        for (int i = 0; i < storageSize; i++)
        {
            StorageSlot slot = Instantiate(slotPrefab, transform).GetComponent<StorageSlot>();

            slotList.Add(slot);
        }

        slots = slotList.ToArray();

        hasGeneratedSlots = true;
    }

    public void Open(StorageUI UI)
    {
        UI.Open(this);

        opened = true;
    }

    public void Close(Slot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].data == null)
            {
                slots[i].data = null;
            }
            else
            {
                slots[i].data = uiSlots[i].data;
            }

            slots[i].stackSize = uiSlots[i].stackSize;
        }

        opened = false;
    }

    public void AddItem(ItemSO data_, float stack_)
    {

    }
}
