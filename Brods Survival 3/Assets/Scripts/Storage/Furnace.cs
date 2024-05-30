using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public Storage storage;
    [Space]
    [Space]
    [HideInInspector] public StorageSlot fuelSlot;
    [HideInInspector] public StorageSlot smeltingSlot;

    [Space]
    public bool isOn;
    public GameObject VFX;
    public NPC npc;

    public AudioSource audioS;
    public AudioClip furnaceOn;
    public AudioClip furnaceOff;

    public int smeltCount;

    private float currentFuelTimer;
    private float fuelTimer;

    private float smeltTimer;
    private float currentSmeltTimer;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        npc = FindObjectOfType<NPC>();

        if (GetComponent<Storage>() != null)
        {
            storage = GetComponent<Storage>();
        }
        else
        {
            Debug.LogError("FURNACE : No storage script attached");
        }
    }

    private void Update()
    {
        #region FIND SLOTS

        if (isOn)
        {

            if (fuelSlot == null)
            {
                for (int i = 0; i < storage.slots.Length; i++)
                {
                    if (storage.slots[i].data != null)
                    {
                        if (storage.slots[i].data.isFuel)
                        {
                            fuelSlot = storage.slots[i];
                            currentFuelTimer = 0;
                            fuelTimer = fuelSlot.data.processTime;
                            break;
                        }
                    }
                }
            }

            if (smeltingSlot == null)
            {
                for (int i = 0; i < storage.slots.Length; i++)
                {
                    if (storage.slots[i].data != null)
                    {
                        if (storage.slots[i].data.outcome != null)
                        {
                            smeltingSlot = storage.slots[i];
                            currentSmeltTimer = 0;
                            smeltTimer = smeltingSlot.data.processTime;
                            break;
                        }
                    }
                }
            }

            if (fuelSlot == null)
            {
                TurnOff();
               // audioS.PlayOneShot(furnaceOff);
            }
            else
            {
                if (fuelSlot.data == null)
                    TurnOff();
            }
        }


        #endregion

        #region SMELT

        if (isOn)
        {

            // FUEL
            if (currentFuelTimer < fuelTimer)
                currentFuelTimer += Time.deltaTime;
            else
            {

                currentFuelTimer = 0;

                fuelSlot.stackSize--;

               
            }

            // SMELTING
            if (currentSmeltTimer < smeltTimer)
                currentSmeltTimer += Time.deltaTime;
            else
            {
                currentSmeltTimer = 0;

                if (smeltingSlot != null)
                {
                    if (smeltingSlot.data != null)
                    {
                        storage.AddItem(smeltingSlot.data.outcome, smeltingSlot.data.outcomeAmount);
                    }

                    smeltingSlot.stackSize--;

                    if (npc.firstTimeInteraction == false)
                    {
                        npc.shouldRecordSmelt = true;
                    }

                    if (npc.shouldRecordSmelt)
                    {
                        smeltCount++;
                        npc.ReceiveSmeltCount(smeltCount);

                    }

                }

               
            }
            

        }


        #endregion
    }

    public void TurnOn()
    {
        isOn = true;
        VFX.SetActive(true);
        audioS.PlayOneShot(furnaceOn);
    }


    public void TurnOff()
    {
        audioS.PlayOneShot(furnaceOff);

        isOn = false;

        VFX.SetActive(false);

        fuelSlot = null;
        smeltingSlot = null;
    }
}
