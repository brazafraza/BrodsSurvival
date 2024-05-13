using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public bool isDead;

    [Header("Player Stats")]
    public float health;
    public float maxHealth = 100f;
    [Space]
    public float hunger;
    public float maxHunger = 100f;
    [Space]
    public float thirst;
    public float maxThirst = 100f;

    [Header("Stats Depletion")]
    public float hungerDepletion = 0.5f;
    public float thirstDepletion = 0.75f;

    [Header("Stats Damage")]
    public float hungerDamage = 1.5f;
    public float thirstDamage = 2.25f;
    public float drownDamage = 0.2f;

    [Header("UI")]
    public StatsBar healthBar;
    public StatsBar hungerBar;
    public StatsBar thirstBar;

    private void Start()
    {
        //      change this in future
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
    }
    private void Update()
    {
        if (isDead)
            return;

        UpdateStats();
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.numberText.text = health.ToString("f0");
        healthBar.bar.fillAmount = health / 100;

        hungerBar.numberText.text = hunger.ToString("f0");
        hungerBar.bar.fillAmount = hunger / 100;

        thirstBar.numberText.text = thirst.ToString("f0");
        thirstBar.bar.fillAmount = thirst / 100;
    }

    private void UpdateStats()
    {
        if(health <= 0)
        {
            health = 0;
        }
        if(health >= maxHealth)
        {
            health = maxHealth;
        }

        if (thirst <= 0)
        {
            thirst = 0;
        }
        if (thirst >= maxThirst)
        {
            thirst = maxThirst;
        }

        if (hunger <= 0)
        {
            hunger = 0;
        }
        if (hunger >= maxHunger)
        {
            hunger = maxHunger;
        }

        //      damage
        if(hunger <=0)
        {
            health -= hungerDamage * Time.deltaTime;
        }

        if (thirst <= 0)
        {
            health -= thirstDamage * Time.deltaTime;
        }

        if (GetComponentInChildren<PostProcessingHandler>().Water != null)
            health -= drownDamage * Time.deltaTime;

        //      depletion
        if(hunger > 0)
        {
            hunger -= hungerDepletion * Time.deltaTime;
        }

        if (thirst > 0)
        {
            thirst -= thirstDepletion * Time.deltaTime;
        }

    }

}
