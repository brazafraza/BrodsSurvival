using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    public ItemSO respawnItem;
    
    public void Respawn()
    {
        Player player = GetComponentInParent<Player>();

        if (respawnItem != null)
            player.windowHandler.inventory.AddItem(respawnItem, 1);

        PlayerRespawnPoint[] respawnPoints = FindObjectsOfType<PlayerRespawnPoint>();

        int i = Random.Range(0, respawnPoints.Length);

        player.GetComponent<PlayerStats>().health = player.GetComponent<PlayerStats>().maxHealth;
        player.GetComponent<PlayerStats>().hunger = player.GetComponent<PlayerStats>().maxHunger;
        player.GetComponent<PlayerStats>().thirst = player.GetComponent<PlayerStats>().maxThirst;

        player.cam.lockCursor = false;
        player.cam.canMove = false;


        player.transform.position = respawnPoints[i].transform.position;


        player.GetComponent<PlayerStats>().isDead = false;

        gameObject.SetActive(false);

    }
}
