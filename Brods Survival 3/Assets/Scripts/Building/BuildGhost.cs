// BuildGhost.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGhost : MonoBehaviour
{
    public GameObject buildPrefab;
    public bool canBuild;
    public List<Collider> col = new List<Collider>();

    private void Start()
    {
        canBuild = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
            col.Add(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
            canBuild = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
            canBuild = true;
        if (other.gameObject.layer == 10)
            col.Remove(other);
    }

    public void AdjustPosition(Vector3 hitPoint)
    {
        transform.position = hitPoint;
    }
}
