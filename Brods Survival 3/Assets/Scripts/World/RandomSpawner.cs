using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public float repsawnTime = 1200f;
    public int maxEntities = 20;
    public GameObject[] spawnableEntities;
    public List<GameObject> spawnedEntities;
    [Space]
    public float width = 10f;
    public float length = 10f;
    public float height = 10f;
    public Color color = Color.yellow;

    private float currentTimer;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, new Vector3(width, height, length));
    }



    private void Start()
    {
        currentTimer = repsawnTime;
    }

    private void Update()
    {
        if (currentTimer < repsawnTime)
            currentTimer += Time.deltaTime;
        else
        {
            if (spawnableEntities.Length <= 0)
            {
                Debug.Log("Spawnable ents not set");
                return;
            }



            int amountToRespawn = 0;
            amountToRespawn = GetAvailableEntitites();

            for (int i = 0; i < amountToRespawn; i++)
            {
                int chosenEntity = 0;

                chosenEntity = Random.Range(0, spawnableEntities.Length);


                StartCoroutine(SpawnObject(spawnableEntities[chosenEntity]));
            }

            currentTimer = 0;
        }
    }

    public int GetAvailableEntitites()
    {
        if (spawnedEntities.Count > 0)
        {

            for (int i = 0; i < spawnedEntities.Count; i++)
            {


                if (spawnedEntities[i] == null)
                    spawnedEntities.RemoveAt(i);
            }


        }
        return maxEntities - spawnedEntities.Count;
    }

    public IEnumerator SpawnObject(GameObject obj)
    {
        bool foundSpot = false;

        while (!foundSpot)
        {
            Vector3 spawnPos = transform.position;
           // Vector3 rotationPos = transform.rotation;
            
           
            //make random rotation here

            spawnPos.x += Random.Range(-width, width);
            spawnPos.y += height;
            spawnPos.z += Random.Range(-length, length);

            Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);



            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.GetComponent<Terrain>() != null)
                {
                    GameObject spawnedObj = Instantiate(obj, hit.point, randomYRotation);

                    spawnedEntities.Add(spawnedObj);

                    foundSpot = true;
                    
                }
            }
        }

        yield return null;
    }
}
