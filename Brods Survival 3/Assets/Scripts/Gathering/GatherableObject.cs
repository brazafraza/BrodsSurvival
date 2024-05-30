using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public enum DeathType { Destroy, EnablePhysics}
    public DeathType deathType;

    private AudioSource audioS;

    //audio clips
    public AudioClip treeBreak;
    public AudioClip stoneBreak;
    public AudioClip metalBreak;
    public AudioClip animalBreak;

    public GatherDataSO[] gatherDatas;
    public int hits;
    public ItemSO[] prefferedTools;
    public float toolMultiplier = 2;

    public int breakCount;

    public NPC npc;

    bool hasDied;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        npc = FindAnyObjectByType<NPC>();

        if (npc == null)
        {
            Debug.Log("No npc found bear");
        }
       
    }

    private void Update()
    {
        //if (npc == null)
        //{
        //    npc = FindAnyObjectByType<NPC>();

        //}

        if (hits <= 0 && !hasDied)
        {
           

            if (deathType == DeathType.Destroy)
            {
               // npc = FindAnyObjectByType<NPC>();
                if (CompareTag("Tree") && npc.firstTimeInteraction == false)
                {
                    npc.shouldRecordBreak = true;
                }

                if (CompareTag("Tree") && npc.shouldRecordBreak)
                {
                    breakCount++;
                    npc.ReceiveBreakCount(breakCount);

                }
                if (CompareTag("Stone"))
                    audioS.PlayOneShot(stoneBreak);
                if (CompareTag("Metal"))
                    audioS.PlayOneShot(stoneBreak);
                if (CompareTag("Enemy") || CompareTag("Passive"))
                    audioS.PlayOneShot(animalBreak);
                Destroy(gameObject);

            }
            else if (deathType == DeathType.EnablePhysics)
            {

               //npc = FindAnyObjectByType<NPC>();
                if (GetComponent<Rigidbody>() != null)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;

                    GetComponent<Rigidbody>().AddTorque(Vector3.right * 20);
                    if (CompareTag("Tree") && npc.firstTimeInteraction == false)
                    {
                        npc.shouldRecordBreak = true;
                    }

                    if (CompareTag("Tree") && npc.shouldRecordBreak)
                    {
                        breakCount++;
                        npc.ReceiveBreakCount(breakCount);

                    }
                    if (CompareTag("Tree"))
                        audioS.PlayOneShot(treeBreak);


                    Destroy(gameObject, 10f);
                }
                else 
                {
                  //  npc = FindAnyObjectByType<NPC>();
                    if (CompareTag("Tree") && npc.firstTimeInteraction == false)
                    {
                        npc.shouldRecordBreak = true;
                    }

                    if (CompareTag("Tree") && npc.shouldRecordBreak)
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

       // bool usingRightTool = false;

       //CHECK FOR TOOL
       if (prefferedTools.Length > 0)
       {
            for (int i = 0; i < prefferedTools.Length; i++)
            {
                if (prefferedTools[i] == toolUsed)
                {
                //   usingRightTool = true;
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
