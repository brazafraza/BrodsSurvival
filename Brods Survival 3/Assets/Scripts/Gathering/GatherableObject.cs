using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public enum DeathType { Destroy, EnablePhysics }
    public DeathType deathType;

    private AudioSource audioS;
    private AudioSource audioSPlayer;

    //audio clips
    public AudioClip treeBreak;
    public AudioClip stoneBreak;
    public AudioClip metalBreak;
    public AudioClip animalBreak;
    public AudioClip treeHit;
    public AudioClip stoneHit;
    public AudioClip metalHit;
    public AudioClip animalHit;

    public GatherDataSO[] gatherDatas;
    public int hits;
    public ItemSO[] prefferedTools;
    public float toolMultiplier = 2;

    public int breakCount;

    public NPC npc;

    bool hasDied;
    bool isDead = false;

    public GameObject playerAS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        npc = FindAnyObjectByType<NPC>();

        playerAS = GameObject.Find("Player");
        audioSPlayer = playerAS.GetComponent<AudioSource>();

        if (npc == null)
        {
            Debug.Log("No npc found bear");
        }

        if (CompareTag("Enemy") || CompareTag("Passive"))
        {
            DisableNonAudioFunctions();
        }
    }

    private void Update()
    {
        if (isDead || !(CompareTag("Enemy") || CompareTag("Passive")))
        {
            if (hits <= 0 && !hasDied)
            {
                HandleObjectDestruction();
                hasDied = true;
            }
        }
    }

    private void HandleObjectDestruction()
    {
        if (CompareTag("Stone"))
        {
            audioSPlayer.PlayOneShot(stoneBreak);
        }
        if (CompareTag("Metal"))
        {
            audioSPlayer.PlayOneShot(metalBreak);
        }
        if (CompareTag("Enemy") || CompareTag("Passive"))
        {
            audioSPlayer.PlayOneShot(animalBreak);
        }
        if (CompareTag("Tree"))
        {
            audioSPlayer.PlayOneShot(treeBreak);
        }

        if (deathType == DeathType.Destroy)
        {
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
        else if (deathType == DeathType.EnablePhysics)
        {
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
                    audioSPlayer.PlayOneShot(treeBreak);

                Destroy(gameObject, 10f);
            }
            else
            {
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
    }

    public void Gather(ItemSO toolUsed, InventoryManager inventory)
    {
        if (hits <= 0 || (!isDead && (CompareTag("Enemy") || CompareTag("Passive"))))
        {
            PlayHitSound();
            return;
        }

        if (prefferedTools.Length > 0)
        {
            for (int i = 0; i < prefferedTools.Length; i++)
            {
                if (prefferedTools[i] == toolUsed)
                {
                    break;
                }
            }
        }

        int selectedGatherData = Random.Range(0, gatherDatas.Length);

        inventory.AddItem(gatherDatas[selectedGatherData].item, gatherDatas[selectedGatherData].amount);

        PlayHitSound();

        hits--;
    }

    private void PlayHitSound()
    {
        if (CompareTag("Tree"))
        {
            audioS.PlayOneShot(treeHit);
        }
        if (CompareTag("Stone"))
        {
            audioS.PlayOneShot(stoneHit);
        }
        if (CompareTag("Metal"))
        {
            audioS.PlayOneShot(metalHit);
        }
        if (CompareTag("Enemy"))
        {
            audioS.Stop();
            audioS.PlayOneShot(animalHit);
        }
        if (CompareTag("Passive"))
        {
            audioS.PlayOneShot(animalHit);
        }
    }

    private void DisableNonAudioFunctions()
    {
        this.enabled = false;
    }

    public void EnableAllFunctions()
    {
        this.enabled = true;
        isDead = true;
    }
}
