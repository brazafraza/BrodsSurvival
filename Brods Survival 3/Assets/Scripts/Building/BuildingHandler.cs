using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public Slot slotInUse;
    public Transform offGroundPoint;
    [Space]
    public float range = 5f;
    public Color allowed;
    public Color blocked;
    [Space]
    public BuildGhost ghost;
    public bool canBuild;

    public NPC npc;
    public int buildCount = 0;

    private void Update()
    {
        UpdateBuilding();
    }

    public void UpdateColors()
    {
       // MeshRenderer renderer = null;
        MeshRenderer[] renderers = { };

       // if (ghost.GetComponent<MeshRenderer>() != null)
        //  renderer = ghost.GetComponent<MeshRenderer>();
         if (ghost.GetComponentInChildren<MeshRenderer>() != null)
            renderers = ghost.GetComponentsInChildren<MeshRenderer>();


        //change material code dynamically?

        foreach (MeshRenderer i in renderers)
        {
            if (canBuild && ghost.canBuild)
                i.materials[0].color = allowed;
            else
                i.materials[0].color = blocked;
        }

        //if (renderer.materials.Length > 1)
        // {
        //    for (int i = 0; i < renderer.materials.Length; i++)
        //    {
                
        //        if (canBuild && ghost.canBuild)
        //            renderer.materials[i].color = allowed;
        //        else
        //            renderer.materials[i].color = blocked;
        //    }
        // }

        
        //else if (renderer.materials.Length == 1)
        //{

        //    if (canBuild && ghost.canBuild)
        //    {
              

        //        renderer.material.color = allowed;
        //        Debug.Log("Material set to allowed" + renderer.material.color);
                
        //    }
        //    else
        //    {

        //         renderer.material.color = blocked;
        //        //renderer.material.SetColor("Red", Color.red);
        //        Debug.Log("Material set to blocked" + renderer.material.color);
                

        //    }

        //}

    }

    public void UpdateBuilding()
    {
        if (slotInUse == null)
        {
            if (ghost != null)
            {
                Destroy(ghost.gameObject);
            }
            return;
        }

        if (slotInUse.stackSize <= 0 || slotInUse.data == null)
        {
            if (ghost != null)
            {
                Destroy(ghost.gameObject);
            }
            return;
        }

        if (ghost == null)
        {
            ghost = Instantiate(slotInUse.data.ghost, offGroundPoint.transform.position, GetComponentInParent<Player>().transform.rotation);
        }


        UpdateColors();
       // Debug.Log("Updated Colours");

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.GetComponent<BuildBlocked>() == null)
            {
                ghost.transform.position = hit.point;
                ghost.transform.rotation = GetComponentInParent<Player>().transform.rotation;
                canBuild = true;
            }
            else
            {
                ghost.transform.position = offGroundPoint.position;
                ghost.transform.rotation = GetComponentInParent<Player>().transform.rotation;
                canBuild = false;
            }
        }
    
            else
            {
                ghost.transform.position = offGroundPoint.position;
                ghost.transform.rotation = GetComponentInParent<Player>().transform.rotation;
                canBuild = false;
            }

        if (Input.GetButtonDown("Fire1") && canBuild && ghost.canBuild)
        {

            slotInUse.stackSize--;
            slotInUse.UpdateSlot();

            Instantiate(ghost.buildPrefab, ghost.transform.position, ghost.transform.rotation);

            //flag event\
            if (npc.firstTimeInteraction == false)
            {
                npc.shouldRecordBuild = true;
            }

            if (npc.shouldRecordBuild)
            {
                buildCount++;
                npc.ReceiveBuildCount(buildCount);

            }

        }

    }
}
