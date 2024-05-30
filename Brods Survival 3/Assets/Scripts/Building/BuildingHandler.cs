using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public Slot slotInUse;
    public Transform offGroundPoint;
    [Space]
    public float range = 10f;
    public Color allowed;
    public Color blocked;
    [Space]
    public BuildGhost ghost;
    public bool canBuild;

    public NPC npc;
    public int buildCount = 0;

    public AudioSource audioS;
    public AudioClip buildSound;

    public float offset = 1.0f;
    //public float offset = 1.0f;
    public float gridSize = 1.0f;
    private Vector3 currentrot = Vector3.zero;

    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateBuilding();
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
        if (slotInUse == null || slotInUse.stackSize <= 0 || slotInUse.data == null)
        {
            if (ghost != null)
            {
                Destroy(ghost.gameObject);
            }
            return;
        }

        if (ghost == null)
        {
            ghost = Instantiate(slotInUse.data.ghost, offGroundPoint.transform.position, Quaternion.identity);
        }

        UpdateColors();

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, range, layer))
        {
            if (hit.transform.GetComponent<BuildBlocked>() == null)
            {
                Vector3 hitPoint = hit.point;

                // Adjust ghost position
                ghost.AdjustPosition(hitPoint);

                Vector3 adjustedPosition = ghost.transform.position;
                Vector3 currentRotation = ghost.transform.rotation.eulerAngles;

                adjustedPosition = new Vector3(Mathf.Round(adjustedPosition.x), Mathf.Round(adjustedPosition.y), Mathf.Round(adjustedPosition.z)); // Snap to grid
                adjustedPosition *= gridSize; // Snap to grid

                ghost.transform.position = adjustedPosition;
                ghost.transform.rotation = Quaternion.Euler(currentRotation);
                canBuild = true;

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    currentRotation += new Vector3(0, 30, 0);
                    ghost.transform.rotation = Quaternion.Euler(currentRotation);
                }
            }
            else
            {
                ghost.transform.position = offGroundPoint.position;
                ghost.transform.rotation = Quaternion.identity;
                canBuild = false;
            }
        }
        else
        {
            ghost.transform.position = offGroundPoint.position;
            ghost.transform.rotation = Quaternion.identity;
            canBuild = false;
        }

        if (Input.GetButtonDown("Fire1") && canBuild && ghost.canBuild)
        {
            Vector3 buildPosition = ghost.transform.position;
            Quaternion buildRotation = ghost.transform.rotation;

            GameObject newObj = Instantiate(ghost.buildPrefab, buildPosition, buildRotation);

            audioS.PlayOneShot(buildSound);

            slotInUse.stackSize--;
            slotInUse.UpdateSlot();

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


}
