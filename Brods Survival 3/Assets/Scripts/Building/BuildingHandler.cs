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
    private float gridSize = 0f; // Grid size is initially 0
    private bool gridSizeSet = false; // Flag to check if the grid size has been set
    private Vector3 gridOrigin; // Origin of the grid

    private void Update()
    {
        UpdateBuilding();
    }

    private void OnDrawGizmos()
    {
        if (gridSize > 0 && gridSizeSet)
        {
            DrawGridGizmos();
        }
    }

    private void DrawGridGizmos()
    {
        Gizmos.color = Color.green;

        float halfRange = range / 2.0f;

        for (float x = gridOrigin.x - halfRange; x <= gridOrigin.x + halfRange; x += gridSize)
        {
            for (float z = gridOrigin.z - halfRange; z <= gridOrigin.z + halfRange; z += gridSize)
            {
                Vector3 pos = new Vector3(x, gridOrigin.y, z);
                Gizmos.DrawWireCube(pos, new Vector3(gridSize, 0.1f, gridSize));
            }
        }
    }

    public void UpdateColors()
    {
        MeshRenderer[] renderers = ghost.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer i in renderers)
        {
            if (canBuild && ghost.canBuild)
                i.materials[0].color = allowed;
            else
                i.materials[0].color = blocked;
        }
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

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.GetComponent<BuildBlocked>() == null)
            {
                Vector3 hitPoint = hit.point;

                // Snap to grid if grid size is set
                Vector3 snappedPosition = gridSizeSet ? SnapToGrid(hitPoint) : hitPoint;
                ghost.transform.position = snappedPosition;
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

            Vector3 buildPosition = ghost.transform.position;
            Quaternion buildRotation = ghost.transform.rotation;

            GameObject newObj = Instantiate(ghost.buildPrefab, buildPosition, buildRotation);

            // Set the grid size if it hasn't been set yet
            if (!gridSizeSet)
            {
                MeshRenderer[] childRenderers = newObj.GetComponentsInChildren<MeshRenderer>();
                if (childRenderers.Length > 0)
                {
                    Bounds combinedBounds = new Bounds(childRenderers[0].bounds.center, Vector3.zero);
                    foreach (MeshRenderer renderer in childRenderers)
                    {
                        combinedBounds.Encapsulate(renderer.bounds);
                    }
                    gridSize = Mathf.Max(combinedBounds.size.x, combinedBounds.size.z); // Assuming the object is rectangular or cube-like
                    gridSizeSet = true;
                    gridOrigin = buildPosition; // Set the origin of the grid
                    Debug.Log($"Grid size set to {gridSize}, origin set to {gridOrigin}");
                }
                else
                {
                    Debug.LogError("Newly instantiated object does not have any child MeshRenderer components.");
                }
            }

            // Flag event
            if (!npc.firstTimeInteraction)
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

    private Vector3 SnapToGrid(Vector3 position)
    {
        if (gridSize == 0) return position; // If gridSize is not set, don't snap

        float x = Mathf.Round((position.x - gridOrigin.x) / gridSize) * gridSize + gridOrigin.x;
        float y = gridOrigin.y; // Ensuring the Y axis is consistent with the grid origin
        float z = Mathf.Round((position.z - gridOrigin.z) / gridSize) * gridSize + gridOrigin.z;
        return new Vector3(x, y, z);
    }
}
