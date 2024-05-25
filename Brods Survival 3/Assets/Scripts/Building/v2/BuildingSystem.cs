using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{

    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentobject;
    private Vector3 currentpos;
    private Vector3 currentrot;
    public Transform currentpreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public float offset = 1.0f;
    public float gridSize = 0.0f;

    public bool isbuilding;

    void Start()
    {
        //currentobject = objects[0];
        ChangeCurrentBuilding(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isbuilding)
            startPreview();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();

        if (Input.GetKeyDown("0") || Input.GetKeyDown("1"))
            SwitchCurrentBuilding();
    }

    public void SwitchCurrentBuilding()
    {
        for(int i = 0; i < 2; i++)
        {
            if (Input.GetKeyDown("" + i))
                ChangeCurrentBuilding(i);
        }
    }

    public void ChangeCurrentBuilding(int cur)
    {
        currentobject = objects[cur];
        if(currentpreview != null)
            Destroy(currentpreview.gameObject);

        GameObject curprev = Instantiate(currentobject.preview, currentpos, Quaternion.Euler(currentrot)) as GameObject;
        currentpreview = curprev.transform;
    }

    public void startPreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, layer))
        {
            if (hit.transform != this.transform)
                showPreview(hit);
        }
    }

    public void showPreview(RaycastHit hit2)
    {
        currentpos = hit2.point;
        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x), Mathf.Round(currentpos.y), Mathf.Round(currentpos.z));
        currentpos *= gridSize;
        currentpos += Vector3.one * offset;
        currentpreview.position = currentpos;

        if (Input.GetKeyDown(KeyCode.Mouse1))
            currentrot += new Vector3(0, 30, 0);
        currentpreview.localEulerAngles = currentrot;
    }

    public void Build()
    {
        PreviewObject PO = currentpreview.GetComponent<PreviewObject>();
        if(PO.isBuildable)
        {
            Instantiate(currentobject.prefab, currentpos, Quaternion.Euler(currentrot));
        }
    
    }

    
}


[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject preview;
    public GameObject prefab;
    public int gold;
}

