using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public enum DeathType { Destroy, EnablePhysics}
    public DeathType deathType;

    public GatherDataSO[] gatherDatas;
    public int hits;
    public ItemSO[] prefferedTools;
    public float toolMultiplier = 2;

    public int breakCount;

    public NPC npc;

    bool hasDied;

    private void Start()
    {
       npc = FindObjectOfType<NPC>();
       
    }

    private void Update()
    {
        if (hits <= 0 && !hasDied)
        {
            if (deathType == DeathType.Destroy)
            {
                if (npc.firstTimeInteraction == false && CompareTag("Tree"))
                {
                    npc.shouldRecordBreak = true;
                }

                if (npc.shouldRecordBreak && CompareTag("Tree"))
                {
                    breakCount++;
                    npc.ReceiveBreakCount(breakCount);

                }
                Destroy(gameObject);

            }
            else if (deathType == DeathType.EnablePhysics)
            {
                if (GetComponent<Rigidbody>() != null)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;

                    GetComponent<Rigidbody>().AddTorque(Vector3.right * 20);

                    if (npc.firstTimeInteraction == false && CompareTag("Tree"))
                    {
                        npc.shouldRecordBreak = true;
                    }

                    if (npc.shouldRecordBreak && CompareTag("Tree"))
                    {
                        breakCount++;
                        npc.ReceiveBreakCount(breakCount);

                    }
                    Destroy(gameObject, 10f);
                }
                else 
                {
                    if (npc.firstTimeInteraction == false && CompareTag("Tree"))
                    {
                        npc.shouldRecordBreak = true;
                    }

                    if (npc.shouldRecordBreak && CompareTag("Tree"))
                    {
                        breakCount++;
                        npc.ReceiveBreakCount(breakCount);

                    }
                    Destroy(gameObject);
                }
                    
            }


            hasDied = true;
        }

       
            
        
    }


    public void Gather(ItemSO toolUsed, InventoryManager inventory)
    {
        if (hits <= 0)
            return;

        bool usingRightTool = false;

       //CHECK FOR TOOL
       if (prefferedTools.Length > 0)
       {
            for (int i = 0; i < prefferedTools.Length; i++)
            {
                if (prefferedTools[i] == toolUsed)
                {
                    usingRightTool = true;
                    break;
                }
            }             
       }


        //GATHER
        int selectedGatherData = Random.Range(0, gatherDatas.Length);

        inventory.AddItem(gatherDatas[selectedGatherData].item, gatherDatas[selectedGatherData].amount);

        hits--;
       
    }
    
}
