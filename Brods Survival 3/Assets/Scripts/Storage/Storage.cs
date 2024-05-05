using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [HideInInspector]public StorageSlot[] slots;
    public StorageSlot slotPrefab;
    public int storageSize = 12;
    [Space]
    public bool opened;


    private void Start()
    {
        List<StorageSlot> slotList = new List<StorageSlot>();

        for (int i = 0; i < storageSize; i++)
        {
           StorageSlot slot = Instantiate(slotPrefab, transform).GetComponent<StorageSlot>();

            slotList.Add(slot);
        }

        slots = slotList.ToArray();
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
}
