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

    [Header("Drop Config")]
    public Transform dropPos;
    public Pickup dropBag;


    [Header("Furnace Config")]
    [HideInInspector] public bool isFurnace;

    private void OnDestroy()
    {
        Slot[] slots = GetComponentsInChildren<Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            DropItem(slots[i].data, slots[i].stackSize);
        }
    }

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

    public void AddItem(ItemSO data_, int stack_)
    {
        StorageSlot[] slots = GetComponentsInChildren<StorageSlot>();

        if (data_.isStackable)
        {
            StorageSlot stackableSlot = null;

            // TRY FINDING STACKABLE SLOT
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty)
                {
                    if (slots[i].data == data_ && slots[i].stackSize < data_.maxStack)
                    {
                        stackableSlot = slots[i];
                        break;
                    }

                }
            }

            if (stackableSlot != null)
            {

                // IF IT CANNOT FIT THE PICKED UP AMOUNT
                if (stackableSlot.stackSize + stack_ > data_.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + stack_) - data_.maxStack;



                    // ADD IT TO THE STACKABLE SLOT
                    stackableSlot.AddItemToSlot(data_, data_.maxStack);

                    // TRY FIND A NEW EMPTY STACK
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (slots[i].IsEmpty)
                        {
                            slots[i].AddItemToSlot(data_, amountLeft);
                            

                            break;
                        }
                    }




                }
                // IF IT CAN FIT THE PICKED UP AMOUNT
                else
                {
                    stackableSlot.AddStackAmount(stack_);


                }

               
            }
            else
            {
                StorageSlot emptySlot = null;


                // FIND EMPTY SLOT
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].IsEmpty)
                    {
                        emptySlot = slots[i];
                        break;
                    }
                }

                // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(data_, stack_);
                   


                }
                else
                {
                    DropItem(data_, stack_);
                }
            }

        }
        else
        {
            StorageSlot emptySlot = null;


            // FIND EMPTY SLOT
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty)
                {
                    emptySlot = slots[i];
                    break;
                }
            }

            // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(data_, stack_);
               


            }
            else
            {
                DropItem(data_, stack_);
            }
        }

    }

    public void DropItem(ItemSO data_, int stack_)
    {
        Pickup drop = Instantiate(dropBag.gameObject, dropPos).GetComponent<Pickup>();

        drop.transform.SetParent(null);

        drop.data = data_;
        drop.stackSize = stack_;
    }
}
