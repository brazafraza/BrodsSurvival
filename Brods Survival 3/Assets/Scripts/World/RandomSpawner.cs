using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public float repsawnTime = 1200f;
    public float originalRespawnTime = 1200f;
    public int maxEntities = 20;
    public GameObject[] spawnableEntities;
    public List<GameObject> spawnedEntities;
    [Space]
    public float width = 10f;
    public float length = 10f;
    public float height = 10f;
    public Color color = Color.yellow;

    private float currentTimer;

    public IslandHappiness islandHappiness;

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
        islandHappy();
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
            // Calculate a random position within the defined bounds
            Vector3 spawnPos = new Vector3(
                Random.Range(transform.position.x - width / 2, transform.position.x + width / 2),
                transform.position.y + height, // Spawn at the top of the height, adjust as necessary
                Random.Range(transform.position.z - length / 2, transform.position.z + length / 2)
            );

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


    public void islandHappy()
    {
        if (islandHappiness.happiness == 10f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 200);
        }
        if (islandHappiness.happiness == 20f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 150);
        }
        if (islandHappiness.happiness == 30f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 130);
        }
        if (islandHappiness.happiness == 40f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 115);
        }
        if (islandHappiness.happiness == 50f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = originalRespawnTime; 
        }
        if (islandHappiness.happiness == 60f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 85);
        }
        if (islandHappiness.happiness == 70f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 70);
        }
        if (islandHappiness.happiness == 80f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 65);
        }
        if (islandHappiness.happiness == 90f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 40);
        }
    
        if (islandHappiness.happiness == 100f && (gameObject.CompareTag("Resource") || gameObject.CompareTag("Tree")))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 25);
        }

        //enemies respawn
        if (islandHappiness.happiness == 10f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 25);
        }
        if (islandHappiness.happiness == 20f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 40);
        }
        if (islandHappiness.happiness == 30f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 65);
        }
        if (islandHappiness.happiness == 40f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 70);
        }
        if (islandHappiness.happiness == 50f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = originalRespawnTime;
        }
        if (islandHappiness.happiness == 60f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 85);
        }
        if (islandHappiness.happiness == 70f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 115);
        }
        if (islandHappiness.happiness == 80f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 115);
        }
        if (islandHappiness.happiness == 90f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 130);
        }

        if (islandHappiness.happiness == 100f && gameObject.CompareTag("Enemy"))
        {
            repsawnTime = repsawnTime - ((repsawnTime / 100) * 150);
        }
    }
}



